using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    interface RiskFilter
    {
        bool isAcceptable(Order o);
    }
}
