using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    public class Fill
    {
        public Order originalOrder { get; set; }
        public Double price { get; set; }
        public int qty { get; set; }
        public DateTime time { get; set; }
        public Int32 matchedQuantity { get; set; }

        public Fill()
        {
            originalOrder = null;
            price = 0;
            qty = 0;
            time = DateTime.UtcNow;
            matchedQuantity = 0;
        }

    }
}
