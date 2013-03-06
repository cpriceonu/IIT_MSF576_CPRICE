using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    class Fill
    {
        public Order originalOrder { get; set; }
        public Double price { get; set; }
        public int qty { get; set; }
        public DateTime time { get; set; }

        public Fill()
        {
            originalOrder = null;
            price = 0;
            qty = 0;
            time = DateTime.UtcNow;
        }

    }
}
