using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace FlexTrade
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Initialize Broker Managers

            //Initialize Risk Engine

            //Initialize Portfolio Manager
                //new PortfolioManager(


            //TODO - REMOVE ALL THE GARBAGE CODE HERE TO PRODUCE ORDERS
            //This is only here because we don't have a UI yet
            Equity eq1 = new Equity("Apple Computer","AAPL");
            MarketOrder ord1 = new MarketOrder(eq1, 100, Order.Side.BUY);
            MarketOrder ord2 = new MarketOrder(eq1, 100, Order.Side.SELL);

            Equity eq2 = new Equity("Google", "GOOG");
            MarketOrder ord3 = new MarketOrder(eq2, 100, Order.Side.BUY);
            MarketOrder ord4 = new MarketOrder(eq2, 100, Order.Side.SELL);

            IBBrokerManager manager = new IBBrokerManager();

            //Since this order is being sent so soon after the IB client object is created, 
            //we have to keep trying because the initial order ID may not come back from IB
            //right away. 
            int orderID = -1;
            while(orderID == -1)
                orderID = manager.submitOrder(ord1);

            orderID = manager.submitOrder(ord2);
            orderID = manager.submitOrder(ord3);
            orderID = manager.submitOrder(ord4);

            //Create Main Window
            log.Info("Creating UI components");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
