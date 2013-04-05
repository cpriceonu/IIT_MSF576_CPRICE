using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace FlexTrade
{
    class PositionManager
    {
        private TradeMatcher tradeMatcher { get; set; }
        private Dictionary<Product, int> positions;  //holds a map of Product to position sizes
        private Dictionary<Order, List<Fill>> orderToFillMap;
        private List<Fill> unmatchedOrders;
        private List<BrokerManager> brokers;
        private List<Trade> matchedTrades;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PositionManager(TradeMatcher t)
        {
            positions = new Dictionary<Product, int>();
            orderToFillMap = new Dictionary<Order, List<Fill>>();
            unmatchedOrders = new List<Fill>();
            matchedTrades = new List<Trade>();
            brokers = new List<BrokerManager>();
            tradeMatcher = t;
        }
        
        public void addBrokers(List<BrokerManager>  b)
        {
            //Add all brokers to the list
            brokers.AddRange(b);

            //register to receive events from brokers
            foreach (BrokerManager brk in brokers)
                brk.FillUpdate += new FillEventHandler(fillReceived);
        }

        public int getPositionSizeByProduct(Product p)
        {
            if (p == null || p.symbol == null)
                throw new Exception("Product object with symbol required");
           
            if(positions.ContainsKey(p))
                return positions[p];
            else 
                return 0;
        }

        public void fillReceived(Fill fill)
        {
            if (fill == null && fill.originalOrder == null)
                log.Error("Did not receive a valid fill message");

            //add orders to the list of unmatched orders
            unmatchedOrders.Add(fill);

            //update the positions
            int qty = fill.qty;
            if(fill.originalOrder.side.Equals(Order.Side.SELL))
                qty *= -1;

            if (positions.ContainsKey(fill.originalOrder.product))
            {
                qty += positions[fill.originalOrder.product];
                positions.Remove(fill.originalOrder.product);
            }

            positions.Add(fill.originalOrder.product, qty);


            //Keep track of the fills for each order
            if (orderToFillMap.ContainsKey(fill.originalOrder))
                orderToFillMap[fill.originalOrder].Add(fill);
            else
            {
                List<Fill> fillList = new List<Fill>();
                fillList.Add(fill);
                orderToFillMap.Add(fill.originalOrder, fillList);
            }

            //match the new fills
            List<Trade> trades = tradeMatcher.match(unmatchedOrders);
            matchedTrades.AddRange(trades);

        }
    }
}
