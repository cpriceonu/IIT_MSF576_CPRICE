using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    class UIController : BrokerListener
    {
        private List<Strategy> runningStrategies;

        public List<OrderGridData> orderGrid { get; set; }
        public List<PositionGridData> positionGrid { get; set; }

        //create strategy

        //start strategy

        //exit strategy

        //data update event

        public void fillReceived(Fill fill)
        {
            throw new NotImplementedException();
        }

        public void bidUpdate(Product p)
        {
            throw new NotImplementedException();
        }

        public void askUpdate(Product p)
        {
            throw new NotImplementedException();
        }

        public void bidQtyUpdate(Product p)
        {
            throw new NotImplementedException();
        }

        public void askQtyUpdate(Product p)
        {
            throw new NotImplementedException();
        }

        public void lastUpdate(Product p)
        {
            throw new NotImplementedException();
        }

        public void lastQtyUpdate(Product p)
        {
            throw new NotImplementedException();
        }
    }
}
