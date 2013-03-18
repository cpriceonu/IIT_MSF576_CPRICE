using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    interface BrokerManager
    {
        event FillEventHandler FillUpdate;
        event BidUpdateEventHandler BidUpdate;
        event AskUpdateEventHandler AskUpdate;
        event BidQtyUpdateEventHandler BidQtyUpdate;
        event AskQtyUpdateEventHandler AskQtyUpdate;
        event LastUpdateEventHandler LastUpdate;
        event LastQtyUpdateEventHandler LastQtyUpdate;
        event BrokerReadyEventHandler AcceptingOrders;

        void connect();
        int submitOrder(Order o);
        void cancelOrder(Order o);
        bool isReadyToTakeOrders();
    }
}
