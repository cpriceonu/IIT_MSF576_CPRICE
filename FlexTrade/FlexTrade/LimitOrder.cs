using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    class LimitOrder : Order
    {
        public Double limitPrice { get; set; }

        public LimitOrder(Product p, int quantity, Side s) : base(p, quantity, s) { }
    }
}
