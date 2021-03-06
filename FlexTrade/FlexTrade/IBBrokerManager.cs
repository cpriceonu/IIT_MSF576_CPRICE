﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krs.Ats.IBNet;
using Krs.Ats.IBNet.Contracts;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using FlexTrade;

namespace FlexTrade
{
    public delegate void FillEventHandler(Fill x);
    public delegate void OrderConfirmEventHandler(Order x);
    public delegate void RiskFilterFailureEventHandler(List<String> msgs);
    public delegate void BrokerReadyEventHandler(Type brokerName);
    public delegate void BidUpdateEventHandler(Product p);
    public delegate void AskUpdateEventHandler(Product p);
    public delegate void BidQtyUpdateEventHandler(Product p);
    public delegate void AskQtyUpdateEventHandler(Product p);
    public delegate void LastUpdateEventHandler(Product p);
    public delegate void LastQtyUpdateEventHandler(Product p);

    //This is the only class that should interact with the IB client directly. The purpose
    //of a broker manager object is to encapsulate all of the broker specific details so 
    //that the remainder of the application can work with one canonical model
    public class IBBrokerManager : BrokerManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const String host = "127.0.0.1";
        private const int port = 7496;
        private bool readyToTakeOrders = false;

        private IBClient ibClient;
        private RiskFilter riskFilter;

        //incoming orders are placed here
        private List<Krs.Ats.IBNet.Order> orderInputQueue;

        //sent orders are stored here waiting to be filled
        private Dictionary<int, Krs.Ats.IBNet.Order> openOrders;

        //used to keep track of the contract objects associated with each order
        private Dictionary<int, Krs.Ats.IBNet.Contract> openOrderContracts;

        //the original order object are also stored here so we don't need to re-create them
        private Dictionary<int, FlexTrade.Order> ordersOrgFormat;

        //ticker ID to product map to keep track of current ticker values for each product
        private Dictionary<int, FlexTrade.Product> tickerIdsToProduct;

        private int orderIdCounter;
        private int tickerIdCounter;

        public event FillEventHandler FillUpdate;
        public event OrderConfirmEventHandler OrderConfirmed;
        public event RiskFilterFailureEventHandler RiskFilterFailure;
        public event BidUpdateEventHandler BidUpdate;
        public event AskUpdateEventHandler AskUpdate;
        public event BidQtyUpdateEventHandler BidQtyUpdate;
        public event AskQtyUpdateEventHandler AskQtyUpdate;
        public event LastUpdateEventHandler LastUpdate;
        public event LastQtyUpdateEventHandler LastQtyUpdate;
        public event BrokerReadyEventHandler AcceptingOrders;

        //Constructor
        public IBBrokerManager(RiskFilter r)
        {
            riskFilter = r;

            tickerIdsToProduct = new Dictionary<int, FlexTrade.Product>();
            openOrders = new Dictionary<int, Krs.Ats.IBNet.Order>();
            orderInputQueue = new List<Krs.Ats.IBNet.Order>();
            openOrderContracts = new Dictionary<int, Krs.Ats.IBNet.Contract>();
            ordersOrgFormat = new Dictionary<int, FlexTrade.Order>();

            //The interface with Interactive Brokers is a singleton. We do this to control the message
            //flow. Having multiple client connections may be better for performance and should be 
            //explored in the future.
            if (ibClient == null)
            {
                //Create the IB Client which is the main interface for interactining the broker system
                log.Info("Creating new IB Client object");
                ibClient = new IBClient();
                
                //We need to be listening for all of the events coming back from IB. This class will
                //catch all of the messages and exceptions for this broker so that other classes
                //don't need to be concerned with broker specific issues.
                ibClient.ThrowExceptions = true;
                ibClient.TickPrice += new EventHandler<TickPriceEventArgs>(priceChangeTick);
                ibClient.TickSize += new EventHandler<TickSizeEventArgs>(sizeChangeTick);
                ibClient.Error += new EventHandler<ErrorEventArgs>(errorReceived);
                ibClient.NextValidId += new EventHandler<NextValidIdEventArgs>(nextValidId);
                ibClient.ExecDetails += new EventHandler<ExecDetailsEventArgs>(fillReceived);
                ibClient.OpenOrder += new EventHandler<OpenOrderEventArgs>(openOrder);
            }
        }

        //Deconstructor
        ~IBBrokerManager()
        {
            log.Info("Disconnecting from the IB Trader workstation");

            //Disconnect and unregister for event notifications
            ibClient.Disconnect();
            ibClient.ReadThread.Abort();
            ibClient.ReadThread.Join();
            ibClient.TickPrice -= new EventHandler<TickPriceEventArgs>(priceChangeTick);
            ibClient.TickSize -= new EventHandler<TickSizeEventArgs>(sizeChangeTick);
            ibClient.Error -= new EventHandler<ErrorEventArgs>(errorReceived);
            ibClient.NextValidId -= new EventHandler<NextValidIdEventArgs>(nextValidId);
            ibClient.ExecDetails -= new EventHandler<ExecDetailsEventArgs>(fillReceived);
            ibClient.OpenOrder -= new EventHandler<OpenOrderEventArgs>(openOrder);

            //Destory the reference to the client
            ibClient = null;
        }

        public void connect()
        {
            //Attempt to connect to local Interactive Brokers Trader Workstation application. 
            //IB TWS must be running. It is responsible for actually connecting to the IB
            //server and opening a special port that is used for making API calls.
            //Of course, the assumption is that the TWS app is running on local host, may want
            //to put this in a configuration file.
            log.Info("Connecting to the IB Trader workstation at " + host + " on port " + port);
            ibClient.Connect(host, port, 0);

            //This ID is meant to represent the unique order ID with Interactive Brokers. 
            tickerIdCounter = 1;
            orderIdCounter = -1;
            ibClient.RequestIds(1); //initializing the ID
        }

        //Take in an order in the canonical format, translate it, and add to the internal queue
        public int submitOrder(FlexTrade.Order o) 
        {
            int currentID = orderIdCounter;
            //Must pass all orders through the risk filter to ensure that it is compliant
            try
            {
                log.Debug("Running order #" + o.internalId + " through risk filter.");
                riskFilter.isAcceptable(o);
            }
            catch(UnacceptableRiskException e)
            {
                log.Error("Order " + o.internalId + " rejected by risk filter");
                
                //Alert any interested in hearing about orders caught by the risk filter 
                if(RiskFilterFailure != null)
                    RiskFilterFailure(e.riskFilterMessages);
                
                //Rethrow the messages so that the sender of the order knows it failed
                throw;
            }

            //If the order ID is still set to -1, then we haven't received the intial ID value from IB
            //We must wait until this is received.
            if (!isReadyToTakeOrders())
            {
                log.Error("Manager not fully initialized.");
                return -1;
            }
            else
            {
                Contract contract = null;

                //Translate to the internal representation of an order
                Krs.Ats.IBNet.Order internalOrder = new Krs.Ats.IBNet.Order();
                internalOrder.Action = (o.side.Equals(FlexTrade.Order.Side.BUY) ? ActionSide.Buy : ActionSide.Sell);
                internalOrder.OutsideRth = false;
                internalOrder.TotalQuantity = o.orderQuantity;
                internalOrder.OrderId = currentID;
                o.internalId = currentID;

                contract = createContractFromProduct(o.product);

                if (o is FlexTrade.LimitOrder)
                {
                    internalOrder.OrderType = OrderType.Limit;
                    internalOrder.LimitPrice = (decimal)((FlexTrade.LimitOrder)o).limitPrice;
                    internalOrder.AuxPrice = 0;
                }
                else if (o is FlexTrade.MarketOrder)
                {
                    internalOrder.OrderType = OrderType.Market;
                    internalOrder.AuxPrice = 0;
                }
                else
                {
                    //throw exception!! We don't support other order types
                }

                //Keeping track of all the objects involved
                orderInputQueue.Add(internalOrder);
                openOrderContracts.Add(currentID, contract);
                ordersOrgFormat.Add(currentID, o);

                //TODO For now this will be done on the same thread. It was created as a separate method to remind me that it would be
                //better to execute this asynchronously for improved performance in the future.
                log.Info("Sending order #" + internalOrder.OrderId + " for " + internalOrder.TotalQuantity + " of " + contract.Symbol);
                placeOrder(internalOrder, contract);
                requestMarketDataForProduct(o.product);
                o.status = Order.OrderStatus.SENT;

                //increment the order and ticker IDs for the next order and contract
                orderIdCounter++;

                return currentID;
            }
        }

        public bool isReadyToTakeOrders()
        {
            return readyToTakeOrders;
        }

        public void requestMarketDataForProduct(Product p)
        {
            Contract contract = createContractFromProduct(p);

            //Ensure we dont have ticker IDs for dup contracts. If we are not receiving market data for this product, then add it.
            if (!tickerIdsToProduct.ContainsValue(p))
            {
                tickerIdsToProduct.Add(tickerIdCounter, p);
                ibClient.RequestMarketData(tickerIdCounter, contract, null, false, false);
            }

            tickerIdCounter++;
        }

        private void placeOrder(Krs.Ats.IBNet.Order order, Krs.Ats.IBNet.Contract contract)
        {
            orderInputQueue.Remove(order);
            openOrders.Add(order.OrderId, order);

            //Send the order to Interactive Brokers
            ibClient.PlaceOrder(order.OrderId, contract, order);
        }

        public void cancelOrder(FlexTrade.Order o)
        {
            log.Info("Cancelling order #" + o.internalId);

            ibClient.CancelOrder(Convert.ToInt32(o.exchangeId));

            if(ordersOrgFormat.ContainsKey(o.internalId))
            {
                Order ord = ordersOrgFormat[o.internalId];
                ord.status = Order.OrderStatus.CANCELLED;
                
                if(openOrders.ContainsKey(ord.internalId))
                {   
                    orderInputQueue.Remove(openOrders[ord.internalId]);
                    openOrders.Remove(ord.internalId);
                }
                ordersOrgFormat.Remove(ord.internalId);
                openOrderContracts.Remove(ord.internalId);
            }
           
        }

        //cancelReplace

        public void errorReceived(Object sender, Krs.Ats.IBNet.ErrorEventArgs e)
        {
            log.Error("ERROR " + e.ErrorCode + " for ticker " + e.TickerId + ": " + e.ErrorMsg);
        }

        public void nextValidId(Object sender, NextValidIdEventArgs e)
        {
            orderIdCounter = e.OrderId;
            if (!readyToTakeOrders)
            {
                readyToTakeOrders = true;
                AcceptingOrders(this.GetType());
            }
        }

        public void connectionMade(Object sender)
        {
        }

        public void sizeChangeTick(Object sender, TickSizeEventArgs e)
        {
            Product p = tickerIdsToProduct[e.TickerId];
            p.asOf = DateTime.UtcNow;
            log.Debug("Size change for " + p.symbol + " on " + e.TickType.ToString() + " New size = " + Convert.ToString(e.Size));

            switch (e.TickType)
            {
                case Krs.Ats.IBNet.TickType.BidSize:
                    p.bidQty = e.Size;
                    if (BidQtyUpdate != null)
                        BidQtyUpdate(p);
                    break;
                case Krs.Ats.IBNet.TickType.AskSize:
                    p.askQty = e.Size;
                    if (AskQtyUpdate != null)
                        AskQtyUpdate(p);
                    break;
                case Krs.Ats.IBNet.TickType.LastSize:
                    p.lastQty = e.Size;
                    if (LastQtyUpdate != null)
                        LastQtyUpdate(p);
                    break;
                default:
                    break;
            }
        }

        public void priceChangeTick(Object sender, TickPriceEventArgs e)
        {
            Product p = tickerIdsToProduct[e.TickerId];
            p.asOf = DateTime.UtcNow;
            log.Debug("Price change for " + p.symbol + " on " + e.TickType.ToString() + " New price = " + Convert.ToString(e.Price));

            switch (e.TickType)
            {
                case Krs.Ats.IBNet.TickType.BidPrice:
                    p.bid = Convert.ToDouble(e.Price);
                    if (BidUpdate != null)
                        BidUpdate(p);
                    break;
                case Krs.Ats.IBNet.TickType.AskPrice:
                    p.ask = Convert.ToDouble(e.Price);
                    if (AskUpdate != null)
                        AskUpdate(p);
                    break;
                case Krs.Ats.IBNet.TickType.LastPrice:
                    p.last = Convert.ToDouble(e.Price);
                    if (LastUpdate != null)
                        LastUpdate(p);
                    break;
                default:
                    break;
            }
        }


        public void fillReceived(Object sender, ExecDetailsEventArgs e)
        {
            if(openOrderContracts.ContainsKey(e.OrderId))
            {
                FlexTrade.Fill fill = new FlexTrade.Fill();
               
                //Get the open orders based on the order ID
                Krs.Ats.IBNet.Order krsOrder = openOrders[e.OrderId];
                FlexTrade.Order ftOrder = ordersOrgFormat[e.OrderId];

                ftOrder.status = Order.OrderStatus.FILL;
                ftOrder.fillPrices.Add(e.Execution.Price);
                ftOrder.fillQuantities.Add(e.Execution.Shares);

                //if completely filled, remove the orders from the list of open orders
                if (ftOrder.fillQuantities.Sum() == ftOrder.orderQuantity)
                {
                    ordersOrgFormat[e.OrderId].status = Order.OrderStatus.FILL;
                    openOrders.Remove(e.OrderId);
                    ordersOrgFormat.Remove(e.OrderId);
                    openOrderContracts.Remove(e.OrderId);
                }
                else
                    ordersOrgFormat[e.OrderId].status = Order.OrderStatus.PARTIAL_FILL;

                //Set the execution values in the order
                fill.originalOrder = ftOrder;
                fill.price = e.Execution.Price;
                fill.qty = e.Execution.Shares;
                fill.time = DateTime.UtcNow;

                if (FillUpdate != null && fill != null)
                    FillUpdate(fill);
            }
        }

        public void openOrder(Object sender, OpenOrderEventArgs e)
        {
            Order ord = null;

            if (ordersOrgFormat.ContainsKey(e.OrderId))
            {
                ord = ordersOrgFormat[e.OrderId];
                ord.status = Order.OrderStatus.CONFIRM;
            }

            if (OrderConfirmed != null && ord != null)
                OrderConfirmed(ord);
        }

        private Contract createContractFromProduct(FlexTrade.Product p)
        {
            Contract c = null;

            if (p is FlexTrade.Equity)
            {
                c = new Krs.Ats.IBNet.Contracts.Equity(p.symbol);
            }
            else
            {
                //throw exception!! We don't support other products
            }

            return c;
        }


        /* Lets implement this later so that we can chart data
        public void GetHistoricalData()
        {
            Krs.Ats.IBNet.BarSize barSizeSetting = Krs.Ats.IBNet.BarSize.FifteenMinutes;
            Krs.Ats.IBNet.HistoricalDataType whatToShow = Krs.Ats.IBNet.HistoricalDataType.Trades;

            DateTime endDateTime = new DateTime(2012, 4, 6, 3, 00, 0);

            ibClient.RequestHistoricalData(orderIdCounter, m_Contract, endDateTime, "1 M", barSizeSetting, whatToShow, 0);

        }
        */
    }
}
