using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlexTrade
{
    static class Program
    {
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

            Equity eq = new Equity("Apple Computer","AAPL");
            MarketOrder ord = new MarketOrder(eq, 100, Order.Side.BUY);
 
            IBBrokerManager manager = new IBBrokerManager();
            manager.submitOrder(ord);

            //Create Main Window
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
