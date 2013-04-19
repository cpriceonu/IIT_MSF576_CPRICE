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
        private Dictionary<int, Order> myOpenOrders;
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

            myOpenOrders = new Dictionary<int, Order>();
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

        //##################################
        //The following events will be called be called
        //when the broker manager triggers an event
        //##################################

        public void fillReceived(Fill fill) 
        {
            int key = -1;
            
            if(fill != null && fill.originalOrder != null)
                key = fill.originalOrder.internalId;
            else
                log.Error("Did not receive a valid fill message");

            //make sure the fill is for one of my orders
            if(myOpenOrders.ContainsKey(key))
            {
                log.Info("Received a fill for order " + key);
                myOpenOrders.Remove(key);
            }
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
