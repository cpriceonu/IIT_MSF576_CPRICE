using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    public interface TradeMatcher
    {
        List<Trade> match(List<Fill> unmatchedOrders, Dictionary<Order, List<Fill>> orderToFillMap);
    }
}
