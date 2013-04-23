using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    class BasicPairTradeStrategy : Strategy, BrokerListener
    {
        private List<Product> products;
        private List<BrokerManager> brokers;
        private Dictionary<StrategyOrderType, Order> myOrders;
        private Dictionary<int, int> myFills;  //order number ==> filled qty
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Strategy specific variables
        private StrategyState state;    //keeps track of the state of the running strategy, very important
        private Product productA, productB; // the two products that will be traded
        private int targetQtyA, targetQtyB, bidQtyA, bidQtyB, askQtyA, askQtyB;
        private double bidA, bidB, askA, askB, buySpread, sellSpread;   //A-B

        //These are the various states that the strategy can be in at any given time
        private enum StrategyState
        {
            CREATED,
            INITIALIZING,
            READY,
            SIGNAL_DETECTED,
            TRADE_ON,
            EXITING
        };

         //These are the various states that the strategy can be in at any given time
        private enum StrategyOrderType
        {
            BUY_A,
            BUY_B,
            SELL_A,
            SELL_B
        };

        public BasicPairTradeStrategy(Product productA, Product productB, List<BrokerManager> b, Int32 targetQtyA, 
            Int32 targetQtyB, Double buySpread, Double sellSpread)
        {
            if(productA == null || productB == null || b == null || targetQtyA == null || targetQtyB == null
                || buySpread == null || sellSpread == null)
            {
                String message = "Invalid parameters";
                log.Error(message);
                throw new Exception(message);
            }

            myOrders = new Dictionary<StrategyOrderType, Order>();
            brokers = b;
            this.state = StrategyState.CREATED;
            this.productA = productA;
            this.productB = productB;
            this.targetQtyA = targetQtyA;
            this.targetQtyB = targetQtyB;
            this.buySpread = buySpread;
            this.sellSpread = sellSpread;

            //register to receive events from broker
            foreach(BrokerManager brk in brokers)
            {
                brk.FillUpdate += new FillEventHandler(fillReceived);
                brk.BidUpdate += new BidUpdateEventHandler(bidUpdate);
                brk.AskUpdate += new AskUpdateEventHandler(askUpdate);
                brk.BidQtyUpdate += new BidQtyUpdateEventHandler(bidQtyUpdate);
                brk.AskQtyUpdate += new AskQtyUpdateEventHandler(askQtyUpdate);
                brk.LastUpdate += new LastUpdateEventHandler(lastUpdate);
                brk.LastQtyUpdate += new LastQtyUpdateEventHandler(lastQtyUpdate);
             }
        }

        public void start()
        {
            if ((brokers != null) && (brokers.Count() > 0))
            {
                //We assume for now that we are working with just one broker. May need to be changed in the future
                BrokerManager broker = brokers.First();

                
            }
        }

        public void exit()
        {
            if ((brokers != null) && (brokers.Count() > 0))
            {
                //We assume for now that we are working with just one broker. May need to be changed in the future
                BrokerManager broker = brokers.First();


            }
        }

        //Sends in the initial orders so that we can begin waiting for the signal
        private void setup()
        {
            if(myOrders != null && myOrders.Count() > 0)
                unwindTrades();

            this.state = StrategyState.INITIALIZING;

            //enter initial trades, two for each product
            BrokerManager brk = getFirstBroker();
            
            //We don't want anyone else submitting orders at this time, so that we limit the time 
            //in between the submission of these four orders.
            lock(brk)
            {
                //enter A side based on B price
                LimitOrder sellAOrd = new LimitOrder(productA, bidQtyA, Order.Side.SELL, askB + sellSpread);
                LimitOrder buyAOrd = new LimitOrder(productA, bidQtyA, Order.Side.BUY, bidB + buySpread);

                //submit A side orders
                brk.submitOrder(sellAOrd);
                brk.submitOrder(buyAOrd);

                //Keep track of the A side orders
                myOrders.Add(StrategyOrderType.SELL_A, sellAOrd);
                myOrders.Add(StrategyOrderType.BUY_A, buyAOrd);
                myFills.Add(sellAOrd.internalId, 0);
                myFills.Add(buyAOrd.internalId, 0);

                //enter B side based on A price
                LimitOrder sellBOrd = new LimitOrder(productB, bidQtyB, Order.Side.SELL, askA + sellSpread);
                LimitOrder buyBOrd = new LimitOrder(productB, bidQtyB, Order.Side.BUY, bidA + buySpread);

                //submit B side orders
                brk.submitOrder(sellAOrd);
                brk.submitOrder(buyAOrd);

                //Keep track of the B side orders
                myOrders.Add(StrategyOrderType.SELL_B, sellBOrd);
                myOrders.Add(StrategyOrderType.BUY_B, buyBOrd);
                myFills.Add(sellBOrd.internalId, 0);
                myFills.Add(buyBOrd.internalId, 0);

                this.state = StrategyState.READY;
            }
        }

        private void unwindTrades()
        {
            //close the positions
           
            //reinitialze data structures to keep track of orders and filles
            myOrders = new Dictionary<StrategyOrderType, Order>();
            myFills = new Dictionary<int, int>();
        }

        //When we detect the signal, we perform these actions
        private void tradeOnSignal(Fill fill)
        {
            LimitOrder hedgeOrder;
            List<Order> ordersToCancel = new List<Order>();

            //determine which of the orders where filled, and add the orders
            //that werent filled to a list. Also, based on the type of fill,
            //determine which order needs to be submitted
            if (fill.originalOrder.product.Equals(productA))
            {
                //This was a fill for product A, so cancel the orders for B
                ordersToCancel.Add(myOrders[StrategyOrderType.BUY_B]);
                ordersToCancel.Add(myOrders[StrategyOrderType.SELL_B]);

                if (fill.originalOrder.side.Equals(Order.Side.BUY))
                {
                    //This was a buy of A, so cancel the sell of A and submit a hedge to buy B
                    hedgeOrder = new LimitOrder(productB, fill.qty, Order.Side.SELL, fill.price - sellSpread);
                    ordersToCancel.Add(myOrders[StrategyOrderType.SELL_A]);
                }
                else
                {
                    //This was a sell of A, so cancel the buy of A and submit a hedge to sell B
                    hedgeOrder = new LimitOrder(productB, fill.qty, Order.Side. BUY, fill.price - buySpread);
                    ordersToCancel.Add(myOrders[StrategyOrderType.BUY_A]);
                }
            }
            else
            {
                //This was a fill for product B, so cancel the orders for A
                ordersToCancel.Add(myOrders[StrategyOrderType.BUY_A]);
                ordersToCancel.Add(myOrders[StrategyOrderType.SELL_A]);

                if (fill.originalOrder.side.Equals(Order.Side.BUY))
                {
                    //This was a buy of B, so cancel the sell of B and submit a hedge to buy A
                    hedgeOrder = new LimitOrder(productA, fill.qty, Order.Side.SELL, fill.price - sellSpread);
                    ordersToCancel.Add(myOrders[StrategyOrderType.SELL_B]);
                }
                else
                {
                    //This was a sell of B, so cancel the buy of B and submit a hedge to sell A
                    hedgeOrder = new LimitOrder(productA, fill.qty, Order.Side.BUY, fill.price - buySpread);
                    ordersToCancel.Add(myOrders[StrategyOrderType.BUY_B]);
                }
            }

            //cancel the orders that aren't a part of this trade
            cancelOrders(ordersToCancel);

            //TODO: What happens if this was a partial fill?

            //submit hedge orders
            getFirstBroker().submitOrder(hedgeOrder);
        }

        private void checkForExitCondition()
        {
            if(true) //TODO: replace with real condition
            {
                state = StrategyState.EXITING;
                unwindTrades();

                //Reinitialize system to begining state
                setup();
            }
        }

        //Since we don't care about multiple brokers at the moment, we just get the first in the list
        private BrokerManager getFirstBroker()
        {
            if ((brokers != null) && (brokers.Count() > 0))
                return brokers.First();
            else
                return null;
        }

        private void cancelOrders(List<Order> ords)
        {
            BrokerManager brk = getFirstBroker();

            foreach (Order ord in ords)
                brk.cancelOrder(ord);
        }

        //##################################
        //The following events will be called be called
        //when the broker manager triggers an event
        //##################################

        public void fillReceived(Fill fill) 
        {
            Order key = null;
            
            if(fill != null && fill.originalOrder != null)
                key = fill.originalOrder;
            else
                log.Error("Did not receive a valid fill message");

            //if (myFills.Contains(key.internalId))
            //{
            //    myFills.
            //}
        }
        public void bidUpdate(Product p) 
        {
            if (productA.Equals(p))
                this.bidA = p.bid;
            else if (productB.Equals(p))
                this.bidB = p.bid;
        }
        public void askUpdate(Product p) 
        {
            if (productA.Equals(p))
                this.askA = p.ask;
            else if (productB.Equals(p))
                this.askB = p.ask;
        }
        public void bidQtyUpdate(Product p) 
        {
            if (productA.Equals(p))
                this.bidQtyA = p.bidQty;
            else if (productB.Equals(p))
                this.bidQtyB = p.bidQty;
        }
        public void askQtyUpdate(Product p) 
        {
            if (productA.Equals(p))
                this.askQtyA = p.askQty;
            else if (productB.Equals(p))
                this.askQtyB = p.askQty;
        }
        public void lastUpdate(Product p) 
        {
            //do nothing
        }
        public void lastQtyUpdate(Product p) 
        {
            //do nothing
        }

        public void orderConfirmed(Order ord)
        {
            //do nothing
        }
    }
}
