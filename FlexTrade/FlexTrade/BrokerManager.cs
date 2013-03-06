using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    interface BrokerManager
    {
        public bool submitOrder(Order o);
        public bool cancelOrder(Order o);
    }
}
