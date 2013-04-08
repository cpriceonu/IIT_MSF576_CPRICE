using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    public interface RiskFilter
    {
        bool isAcceptable(Order o);
    }
}
