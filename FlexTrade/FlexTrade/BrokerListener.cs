using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    interface BrokerListener
    {
        void fillReceived(Fill fill);
        void bidUpdate(Product p);
        void askUpdate(Product p);
        void bidQtyUpdate(Product p);
        void askQtyUpdate(Product p);
        void lastUpdate(Product p);
        void lastQtyUpdate(Product p);
    }
}
