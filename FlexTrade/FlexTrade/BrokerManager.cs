using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    interface BrokerManager
    {
        int submitOrder(Order o);
        void cancelOrder(Order o);
    }
}
