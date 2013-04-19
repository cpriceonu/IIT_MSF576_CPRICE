using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    public class OrderGridData : IEquatable<OrderGridData>
    {
        [DisplayName("Order #")]
        public int orderNum { get; set; }
        [DisplayName("Type")]
        public String type { get; set; }
        [DisplayName("Side")]
        public String side { get; set; }
        [DisplayName("Symbol")]
        public String symbol { get; set; }
        [DisplayName("Quantity")]
        public int qty { get; set; }
        [DisplayName("Filled")]
        public int filled { get; set; }

        public bool Equals(OrderGridData other)
        {
            if (orderNum == other.orderNum)
                return true;
            else
                return false;
        }
    }
}
