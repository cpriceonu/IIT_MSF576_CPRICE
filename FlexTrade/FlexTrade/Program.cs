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
        private static List<BrokerManager> brokers;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Initialize Position Manager
            FIFOTradeMatcher matcher = new FIFOTradeMatcher();
            PositionManager posMgr = new PositionManager(matcher);

            //Initialize Risk Engine
            SimplePositionLimtRiskFilter riskFilter = new SimplePositionLimtRiskFilter(posMgr);

            //Initialize Broker Managers
            brokers = new List<BrokerManager>();

            //Register to get a call back when this broker is ready to receive orders
            IBBrokerManager ibManager = new IBBrokerManager(riskFilter);
            ibManager.AcceptingOrders += new BrokerReadyEventHandler(readyToAcceptOrders);
            brokers.Add(ibManager);
            
            posMgr.addBrokers(brokers);

            ibManager.connect();

            //Create Main Window
            log.Info("Creating UI components");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //MainWindow win = new MainWindow(controller);
            MainWindow win = new MainWindow();

            //Create UI Controller
            UIController controller = new UIController(brokers, posMgr, win);

            Application.Run(win);
        }

        //We should only start our strategy when the broker manager is ready to accept orders
        public static void readyToAcceptOrders(Type name)
        {
            //TODO - REMOVE ALL THE GARBAGE CODE HERE TO PRODUCE ORDERS
            //This is only here because we don't have a UI yet
            Equity eq1 = new Equity("Apple Computer", "AAPL");
            Equity eq2 = new Equity("Google", "GOOG");

            List<Product> products = new List<Product>();
            products.Add(eq1);
            products.Add(eq2);

            BuyAndHoldStrategy strategy = new BuyAndHoldStrategy(products, brokers, 500);

            strategy.start();
            strategy.exit();
        }

    }
}
