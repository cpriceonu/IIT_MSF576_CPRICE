using System;
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
    delegate void UpdateEventHandler(double x);
    delegate void FillEventHandler(Fill x);
    delegate void DataUpdateEventHandler(Product p);
    delegate void FillUpdateEventHandler(Object sender, ExecDetailsEventArgs e);
    delegate void PriceUpdateEventHandler(Object sender, TickPriceEventArgs e);
    delegate void SizeUpdateEventHandler(Object sender, TickSizeEventArgs e);


    //This is the only class that should interact with the IB client directly. The purpose
    //of a broker manager object is to encapsulate all of the broker specific details so 
    //that the remainder of the application can work with one canonical model
    class IBBrokerManager : BrokerManager
    {
        private const String host = "127.0.0.1";
        private const int port = 7496;

        private static IBClient ibClient;
        private static int id;
        private PriceUpdateEventHandler OnPriceUpdateDelegate;
        private SizeUpdateEventHandler OnSizeUpdateDelegate;
        private FillUpdateEventHandler OnFillUpdateDelegate;

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
        public event DataUpdateEventHandler BidAskUpdate;

        //Constructor
        public IBBrokerManager()
        {
            tickerIdsToProduct = new Dictionary<int, FlexTrade.Product>();
            orderInputQueue = new List<Krs.Ats.IBNet.Order>();
            openOrderContracts = new Dictionary<int, Krs.Ats.IBNet.Contract>();
            ordersOrgFormat = new Dictionary<int, FlexTrade.Order>();

            //The interface with Interactive Brokers is a singleton. We do this to control the message
            //flow. Having multiple client connections may be better for performance and should be 
            //explored in the future.
            if (ibClient == null)
            {
                //This ID is meant to represent the unique order ID with Interactive Brokers. 
                orderIdCounter = 0;

                //Create the IB Client which is the main interface for interactining the broker system
                ibClient = new IBClient();

                //We need to be listening for all of the events coming back from IB. This class will
                //catch all of the messages and exceptions for this broker so that other classes
                //don't need to be concerned with broker specific issues.
                ibClient.ThrowExceptions = true;
                ibClient.TickPrice += new EventHandler<TickPriceEventArgs>(S_OnPriceDataUpdate);
                ibClient.TickSize += new EventHandler<TickSizeEventArgs>(S_OnSizeDataUpdate);
                ibClient.Error += new EventHandler<ErrorEventArgs>(S_OnError);
                ibClient.NextValidId += new EventHandler<NextValidIdEventArgs>(S_OnNextValidId);
                ibClient.ExecDetails += new EventHandler<ExecDetailsEventArgs>(S_OnFill);
                ibClient.HistoricalData += new EventHandler<HistoricalDataEventArgs>(OnHistoricalDataUpdate);

                //Attempt to connect to local Interactive Brokers Trader Workstation application. 
                //IB TWS must be running. It is responsible for actually connecting to the IB
                //server and opening a special port that is used for making API calls.
                //Of course, the assumption is that the TWS app is running on local host, may want
                //to put this in a configuration file.
                ibClient.Connect(host, port, 0);
            }

            ///////////////////////////////////////////////////////////////////
            ////////////  These delegates perform cross-thread operation ///////
            ////////////////////////////////////////////////////////////////////
            OnPriceUpdateDelegate = new PriceUpdateEventHandler(Client_TickPrice);
            OnSizeUpdateDelegate = new SizeUpdateEventHandler(Client_TickSize);
            OnFillUpdateDelegate = new FillUpdateEventHandler(Client_Fill);
            ////////////////////////////////////////////////////////////////////
        }

        //Deconstructor
        ~IBBrokerManager()
        {
            //Disconnect and unregister for event notifications
            ibClient.Disconnect();
            ibClient.ReadThread.Abort();
            ibClient.ReadThread.Join();
            ibClient.TickPrice -= new EventHandler<TickPriceEventArgs>(S_OnPriceDataUpdate);
            ibClient.TickSize -= new EventHandler<TickSizeEventArgs>(S_OnSizeDataUpdate);
            ibClient.Error -= new EventHandler<ErrorEventArgs>(S_OnError);
            ibClient.NextValidId -= new EventHandler<NextValidIdEventArgs>(S_OnNextValidId);
            ibClient.ExecDetails -= new EventHandler<ExecDetailsEventArgs>(S_OnFill);
            
            //Destory the reference to the client
            ibClient = null;
        }

        //Take in an order in the canonical format, translate it, and add to the internal queue
        public int submitOrder(FlexTrade.Order o)
        {
            int internalId = orderIdCounter;
            int tickerId = tickerIdCounter;
            orderIdCounter++;
            tickerIdCounter++;

            Contract contract = null;
            
            //Translate to the internal representation of an order
            Krs.Ats.IBNet.Order internalOrder = new Krs.Ats.IBNet.Order();
            internalOrder.Action = ( o.side.Equals(FlexTrade.Order.Side.BUY) ? ActionSide.Buy : ActionSide.Sell );
	        internalOrder.OutsideRth = false;
	        internalOrder.TotalQuantity = o.orderQuantity;

            if (o.product is FlexTrade.Equity)
            {
                contract = new Krs.Ats.IBNet.Contracts.Equity(o.product.symbol);
            }
            else 
            {
                //throw exception!! We don't support other products
            }

            if (o is FlexTrade.LimitOrder)
            {
                internalOrder.OrderType = OrderType.Limit;
            }
            else if (o is FlexTrade.MarketOrder)
            {
                internalOrder.OrderType = OrderType.Market;
            }
            else
            {
                //throw exception!! We don't support other order types
            }

            tickerIdsToProduct.Add(tickerId, o.product);
            orderInputQueue.Add(internalOrder);
            openOrderContracts.Add(internalId, contract);
            ordersOrgFormat.Add(internalId, o);
            ibClient.RequestMarketData(tickerId, contract, null, false, false);
         
            //TODO For now this will be done on the same thread. It was created as a separate method so that it would be
            //easier to execute this asynchronously for improved performance in the future.
            placeOrder(internalOrder);

            return internalId;
        }


        public void cancelOrder(FlexTrade.Order o)
        {
            ibClient.CancelOrder(Convert.ToInt32(o.exchangeId));
        }

        public void cancelOrder(int id)
        {
            //FlexTrade.Order tempOrd = new FlexTrade.Order();
           // tempOrd.exchangeId = id;
            //cancelOrder(tempOrd);
        }

        public void S_OnError(Object sender, Krs.Ats.IBNet.ErrorEventArgs e)
        {
            //MessageBox.Show( e.ErrorMsg );
        }

        public void S_OnNextValidId(Object sender, NextValidIdEventArgs e)
        {
            orderIdCounter = e.OrderId;
        }

        ///////////////////////////////////////////////////////////////////////////////////
        //////////////////////// Real time tick data //////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////
        ///////  Switch event update to main thread ///////////////////////////////////////

        public void S_OnPriceDataUpdate(Object sender, TickPriceEventArgs e)
        {
            //Product p = new Product(e.TickerId, e.TickerId);
            //p.I_OnPriceDataUpdate(sender, e);
        }
        public void I_OnPriceDataUpdate(Object sender, TickPriceEventArgs e)
        {
            //this.BeginInvoke(OnPriceUpdateDelegate, sender, e);
        }

        public void S_OnSizeDataUpdate(Object sender, TickSizeEventArgs e)
        {
           // Instrument p = contracts[e.TickerId];
            //p.I_OnSizeDataUpdate(sender, e);
        }
        public void S_OnFill(Object sender, ExecDetailsEventArgs e)
        {
            //foreach (KeyValuePair<int, Instrument> x in contracts)
           // {
           //     if (e.Contract.Symbol == x.Value.Symbol)
           //     {
           //         x.Value.I_OnFill(sender, e);
           //         break;
           ////     }
           /// }
        }
        public void I_OnSizeDataUpdate(Object sender, TickSizeEventArgs e)
        {
            //this.BeginInvoke(OnSizeUpdateDelegate, sender, e);
        }
        public void I_OnFill(Object sender, ExecDetailsEventArgs e)
        {
            //this.BeginInvoke(OnFillUpdateDelegate, sender, e);
        }
        /////////  Update form from the main thread ////////////////////////////////////////

        public void Client_TickSize(Object sender, TickSizeEventArgs e)
        {
            Product p = tickerIdsToProduct[e.TickerId];
                     
            switch (e.TickType)
            {
                case Krs.Ats.IBNet.TickType.BidSize:
                    p.bidQty = e.Size;
                    break;
                case Krs.Ats.IBNet.TickType.AskSize:
                    p.askQty = e.Size;
                    break;
                case Krs.Ats.IBNet.TickType.LastSize:
                    p.lastQty = e.Size;
                    break;
                default:
                    break;
            }

            p.asOf = DateTime.UtcNow;
            BidAskUpdate(p);
        }

        public void Client_TickPrice(Object sender, TickPriceEventArgs e)
        {
            Product p = tickerIdsToProduct[e.TickerId];

            switch (e.TickType)
            {
                case Krs.Ats.IBNet.TickType.BidPrice:
                    p.bid = Convert.ToDouble(e.Price);
                    break;
                case Krs.Ats.IBNet.TickType.AskPrice:
                    p.ask = Convert.ToDouble(e.Price);
                    break;
                case Krs.Ats.IBNet.TickType.LastPrice:
                    p.last = Convert.ToDouble(e.Price);
                    break;
                default:
                    break;
            }

            p.asOf = DateTime.UtcNow;
            BidAskUpdate(p);
        }


        public void Client_Fill(Object sender, ExecDetailsEventArgs e)
        {
            if(openOrderContracts.ContainsKey(e.OrderId))
            {
                FlexTrade.Fill fill = new FlexTrade.Fill();

                //Get the open orders based on the order ID
                Krs.Ats.IBNet.Order krsOrder = openOrders[e.OrderId];
                FlexTrade.Order ftOrder = ordersOrgFormat[e.OrderId];

                //remove the orders from the list of open orders
                ordersOrgFormat.Remove(e.OrderId);
                openOrderContracts.Remove(e.OrderId);

                //Set the execution values in the order
                fill.originalOrder = ftOrder;
                fill.price = e.Execution.Price;
                fill.qty = e.Execution.Shares;
                fill.time = DateTime.UtcNow;

                FillUpdate(fill);
            }
        }

        private void placeOrder(Krs.Ats.IBNet.Order order)
        {           
            //remove order from input queue first to avoid duplicate submissions
            /*IEnumerable<Krs.Ats.IBNet.Order> OrderQuery =
                from orderRes in orderInputQueue
                where orderRes.OrderId == order.OrderId
                select orderRes;
            foreach(Krs.Ats.IBNet.Order orderRes in OrderQuery)
            {
                orderInputQueue.Remove(orderRes);
            }*/

            orderInputQueue.Remove(order);

            //Send the order to Interactive Brokers
            ibClient.PlaceOrder(order.OrderId, openOrderContracts[order.OrderId], order);
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
        public void OnHistoricalDataUpdate(Object o, HistoricalDataEventArgs m_Args)
        {

            Debug.WriteLine(m_Args.Open.ToString());

        }

        public double get_TickSize()
        {
            return 10.0;//m_Contract.Multiplier;
        }
    }
}
