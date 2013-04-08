using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlexTrade
{
    public class UIController : BrokerListener
    {
        //private List<Strategy> runningStrategies;
        private MainWindow win;

        public UIController(List<BrokerManager> brokers, MainWindow w)
        {
            win = w;

            //register to receive events from brokers
            foreach (BrokerManager brk in brokers)
            {
                brk.FillUpdate += new FillEventHandler(fillReceived);
                brk.OrderConfirmed += new OrderConfirmEventHandler(orderConfirmed);
            }
        }

        //create strategy

        //start strategy

        //exit strategy

        //data update event

        public void fillReceived(Fill fill)
        {
            win.addUpdateOrder(createOrderGridItem(fill.originalOrder));
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

        public void orderConfirmed(Order ord)
        {
            win.addUpdateOrder(createOrderGridItem(ord));
        }

        private OrderGridData createOrderGridItem(Order ord)
        {
            OrderGridData data = new OrderGridData();
            data.side = ord.side.ToString();
            data.symbol = ord.product.symbol;
            data.orderNum = ord.internalId;
            data.qty = ord.orderQuantity;
            data.filled = ord.fillQuantities.Sum();

            return data;
        }
    }
}
