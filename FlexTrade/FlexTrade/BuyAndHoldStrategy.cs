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
        private int quantityOfEachProduct;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BuyAndHoldStrategy(List<Product> p, List<BrokerManager> b, int quantity)
        {
            products = p;
            brokers = b;
            if (quantity > 0)
                quantityOfEachProduct = quantity;
            else
                quantityOfEachProduct = 1;
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
                        broker.submitOrder(currentOrder);
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
                    broker.submitOrder(currentOrder);
                }
                catch (Exception e)
                {
                    log.Error("Unable to submit order", e);
                }
            }
        }
    }
}
