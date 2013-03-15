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
        private Dictionary<String, int> positions;
        private List<Order> unmatchedOrders;
        private List<Trade> matchedTrades;

        public PositionManager(TradeMatcher t)
        {
            positions = new Dictionary<String, int>();
            unmatchedOrders = new List<Order>();
            matchedTrades = new List<Trade>();
            tradeMatcher = t;
        }

        public int getPositionSizeByProduct(Product p)
        {
            if (p == null || p.symbol == null)
                throw new Exception("Product object with symbol required");

            return getPositionSizeBySymbol(p.symbol);
        }

        public int getPositionSizeBySymbol(String s)
        {
            if (positions.ContainsKey(s))
                return positions[s];
            else
                return 0;
        }
    }
}
