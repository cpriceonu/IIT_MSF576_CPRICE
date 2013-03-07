using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    class MarketOrder : Order
    {
        public MarketOrder(Product p, int quantity, Side s) : base(p, quantity, s) { }
    }
}
