using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    interface PositionListener
    {
        void tradeMatched(List<Trade> t);
        void positionChange(Product p, int size);
    }
}
