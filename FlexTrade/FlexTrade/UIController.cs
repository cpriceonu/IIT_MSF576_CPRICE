using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlexTrade
{
    public class UIController : BrokerListener, PositionListener
    {
        //private List<Strategy> runningStrategies;
        private MainWindow win;
        private double cummulativePnL;

        public UIController(List<BrokerManager> brokers, PositionManager pos, MainWindow w)
        {
            cummulativePnL = 0;
            win = w;
            pos.PositionChange += new PositionChangedEventHandler(positionChange);
            pos.TradeMatched += new TradeMatchedEventHandler(tradeMatched);

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

        public void tradeMatched(List<Trade> tradeList)
        {
            foreach(Trade t in tradeList)
            {
                cummulativePnL += t.profitloss;
            }
            win.newPnLValue(cummulativePnL);
        }

        public void positionChange(Product p, int size)
        {

        }
    }
}
