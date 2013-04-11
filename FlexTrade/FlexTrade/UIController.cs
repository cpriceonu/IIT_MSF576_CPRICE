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
        private Strategy runningStrategy;
        private MainWindow win;
        private double cummulativePnL;
        private List<BrokerManager> brokers;

        public UIController(List<BrokerManager> b, PositionManager pos, MainWindow w)
        {
            runningStrategy = null;
            cummulativePnL = 0;
            win = w;
            brokers = b;
            pos.PositionChange += new PositionChangedEventHandler(positionChange);
            pos.TradeMatched += new TradeMatchedEventHandler(tradeMatched);
            win.StrategyStart += new StrategyStartDelegate(startStrategy);
            win.StrategyStop += new StrategyStopDelegate(stopStrategy);

            //register to receive events from brokers
            foreach (BrokerManager brk in brokers)
            {
                brk.FillUpdate += new FillEventHandler(fillReceived);
                brk.OrderConfirmed += new OrderConfirmEventHandler(orderConfirmed);
                brk.LastUpdate += new LastUpdateEventHandler(lastUpdate);
            }
        }

        //start strategy
        private void startStrategy(String name)
        {
            if (runningStrategy == null)
            {
                //TODO - REMOVE ALL THE GARBAGE CODE HERE TO PRODUCE ORDERS
                //This is only here because we don't have a UI yet
                Equity eq1 = new Equity("Apple Computer", "AAPL");
                Equity eq2 = new Equity("Google", "GOOG");

                List<Product> products = new List<Product>();
                products.Add(eq1);
                products.Add(eq2);

                runningStrategy = new BuyAndHoldStrategy(products, brokers, 500);

                runningStrategy.start();
            }
        }

        //exit strategy
        private void stopStrategy()
        {
            runningStrategy.exit();
            runningStrategy = null;
        }

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
            PositionGridData pos = new PositionGridData();
            pos.symbol = p.symbol;
            pos.last = p.last;
            win.updatePrice(pos);
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
            PositionGridData pos = new PositionGridData();
            pos.position = size;
            pos.symbol = p.symbol;
            win.updatePosition(pos);
        }
    }
}
