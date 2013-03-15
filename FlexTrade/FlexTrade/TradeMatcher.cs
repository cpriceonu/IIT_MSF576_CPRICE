using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    interface TradeMatcher
    {
        List<Trade> match(List<Order> unmatchedOrders);
    }
}
