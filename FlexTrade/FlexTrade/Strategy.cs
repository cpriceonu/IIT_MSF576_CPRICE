using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    interface Strategy
    {
        void start();
        void exit();

        void bidUpdate(Product p);
        void askUpdate(Product p);
        void bidQtyUpdate(Product p);
        void askQtyUpdate(Product p);
        void lastUpdate(Product p);
        void lastQtyUpdate(Product p);
        void fillReceived(Fill fill);
    }
}
