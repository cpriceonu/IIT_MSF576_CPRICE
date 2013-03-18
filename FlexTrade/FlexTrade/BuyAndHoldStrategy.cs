using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    class BuyAndHoldStrategy : Strategy
    {
        private List<Product> products;
        private List<BrokerManager> brokers;
        private Dictionary<int, Order> myOpenOrders;
        private int quantityOfEachProduct;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BuyAndHoldStrategy(List<Product> p, List<BrokerManager> b, int quantity)
        {
            myOpenOrders = new Dictionary<int, Order>();
            products = p;
            brokers = b;
            if (quantity > 0)
                quantityOfEachProduct = quantity;
            else
                quantityOfEachProduct = 1;

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
                BrokerManager broker = brokers.First();
                foreach (Product p in products)
                {
                    MarketOrder currentOrder = new MarketOrder(p, quantityOfEachProduct, Order.Side.BUY);             

                    try{
                        int orderId = broker.submitOrder(currentOrder);
                        myOpenOrders.Add(orderId, currentOrder);
                    }catch(Exception e){
                        log.Error("Unable to submit order", e);
                    }
                }
            }
        }

        public void exit()
        {
            BrokerManager broker = brokers.First();
            foreach (Product p in products)
            {
                MarketOrder currentOrder = new MarketOrder(p, quantityOfEachProduct, Order.Side.SELL);

                try
                {
                    int orderId = broker.submitOrder(currentOrder);
                    myOpenOrders.Add(orderId, currentOrder);
                }
                catch (Exception e)
                {
                    log.Error("Unable to submit order", e);
                }
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
            //do nothing
        }
        public void askUpdate(Product p) 
        {
            //do nothing
        }
        public void bidQtyUpdate(Product p) 
        {
            //do nothing
        }
        public void askQtyUpdate(Product p) 
        {
            //do nothing
        }
        public void lastUpdate(Product p) 
        {
            //do nothing
        }
        public void lastQtyUpdate(Product p) 
        {
            //do nothing
        }
    }
}
