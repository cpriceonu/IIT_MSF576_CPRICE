using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    public class Trade
    {
        public Guid internalID { get; set; }
        public Dictionary<Fill, int> openingOrders { get; set; }
        public Dictionary<Fill, int> closingOrders { get; set; }
        public Double profitloss { get; set; }
        public Double perSharePnL { get; set; }

        public Trade()
        {
            internalID = System.Guid.NewGuid();
            openingOrders = new Dictionary<Fill, int>();
            closingOrders = new Dictionary<Fill, int>();
            profitloss = 0;
        }

        public Trade(Dictionary<Fill, int> opening, Dictionary<Fill, int> closing, Double pNL)
        {
            internalID = System.Guid.NewGuid();
            openingOrders = opening;
            closingOrders = closing;
            profitloss = pNL;
        }
    }
}
