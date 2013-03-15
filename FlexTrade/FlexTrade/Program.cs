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
            //TODO - REMOVE ALL THE GARBAGE CODE HERE TO PRODUCE ORDERS
            //This is only here because we don't have a UI yet
            Equity eq1 = new Equity("Apple Computer","AAPL");
            Equity eq2 = new Equity("Google", "GOOG");

            //Initialize Position Manager
            FIFOTradeMatcher matcher = new FIFOTradeMatcher();
            PositionManager posMgr = new PositionManager(matcher);

            //Initialize Risk Engine
            SimplePositionLimtRiskFilter riskFilter = new SimplePositionLimtRiskFilter(posMgr);
            
            //Initialize Broker Managers
            IBBrokerManager ibManager = new IBBrokerManager(riskFilter);
            ibManager.BidAskUpdate += new DataUpdateEventHandler(updatePriceData);

            List<Product> products = new List<Product>();
            products.Add(eq1);
            products.Add(eq2);

            List<BrokerManager> brokers = new List<BrokerManager>();
            brokers.Add(ibManager);

            BuyAndHoldStrategy strategy = new BuyAndHoldStrategy(products, brokers, 100);

            strategy.start();
            strategy.exit();
         
            //Create Main Window
            log.Info("Creating UI components");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        public static void updatePriceData(Product p)
        {
        }
    }
}
