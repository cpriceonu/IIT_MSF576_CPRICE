using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    class Trade
    {
        public Guid internalID { get; set; }
        public List<Order> openingOrders { get; set; }
        public List<Order> closingOrders { get; set; }
        public Double profitloss { get; set; }

        public Trade(List<Order> opening, List<Order> closing, Double pNL)
        {
            internalID = System.Guid.NewGuid();
            openingOrders = opening;
            closingOrders = closing;
            profitloss = pNL;
        }
    }
}
