using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krs.Ats.IBNet;
using Krs.Ats.IBNet.Contracts;


namespace MSF576_HelloWorld
{
    static class Program
    {
        


        private static int NextOrderId = 1;
        private static Contract AAPL;
        private static Contract TF;
        private static Contract TickNasdaq;
        private static Contract VolNasdaq;
        private static Contract AdNasdaq;
        private static Contract TickNyse;
        private static Contract VolNyse;
        private static Contract AdNyse;
        private static Contract YmEcbot;
        private static Contract ES;
        private static Contract SPY;
        private static Contract ZN;
        private static Contract ZB;
        private static Contract ZT;
        private static Contract ZF;
        private static IBClient client;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main(string[] args)
        {
            client = new IBClient();
            client.ThrowExceptions = true;

            client.TickPrice += client_TickPrice;
            client.TickSize += client_TickSize;
            client.Error += client_Error;
            client.NextValidId += client_NextValidId;
            client.UpdateMarketDepth += client_UpdateMktDepth;
            client.RealTimeBar += client_RealTimeBar;
            client.OrderStatus += client_OrderStatus;
            client.ExecDetails += client_ExecDetails;

            Console.WriteLine("Connecting to IB.");
            client.Connect("127.0.0.1", 7496, 0);
            TF = new Contract("TF", "NYBOT", SecurityType.Future, "USD", "200909");
            YmEcbot = new Contract("YM", "ECBOT", SecurityType.Future, "USD", "200909");
            ES = new Contract("ES", "GLOBEX", SecurityType.Future, "USD", "200909");
            SPY = new Contract("SPY", "GLOBEX", SecurityType.Future, "USD", "200909");
            ZN = new Contract("ZN", "ECBOT", SecurityType.Future, "USD", "200909");
            ZB = new Contract("ZB", "ECBOT", SecurityType.Future, "USD", "200909");
            ZT = new Contract("ZT", "ECBOT", SecurityType.Future, "USD", "200909");
            AAPL = new Contract("ZF", "ECBOT", SecurityType.Future, "USD", "200909");

            //TickNasdaq = new Contract("TICK-NASD", "NASDAQ", SecurityType.Index, "USD");
            //VolNasdaq = new Contract("VOL-NASD", "NASDAQ", SecurityType.Index, "USD");
            //AdNasdaq = new Contract("AD-NASD", "NASDAQ", SecurityType.Index, "USD");


            //TickNyse = new Contract("TICK-NYSE", "NYSE", SecurityType.Index, "USD");
            //VolNyse = new Contract("VOL-NYSE", "NYSE", SecurityType.Index, "USD");
            //AdNyse = new Contract("AD-NYSE", "NYSE", SecurityType.Index, "USD");

            //New Contract Creation Features
            Equity Google = new Equity("GOOG");

            client.RequestMarketData(14, Google, null, false, false);
            client.RequestMarketDepth(15, Google, 5);
            client.RequestRealTimeBars(16, Google, 5, RealTimeBarType.Trades, false);

            Order BuyContract = new Order();
            BuyContract.Action = ActionSide.Sell;
            BuyContract.OutsideRth = false;
            BuyContract.LimitPrice = 560;
            BuyContract.OrderType = OrderType.Market;
            BuyContract.TotalQuantity = 1;
            BuyContract.AuxPrice = 560;       
            
            
            client.PlaceOrder(NextOrderId, Google, BuyContract);
            client.RequestIds(1);

            client.RequestExecutions(34, new ExecutionFilter());

            client.RequestAllOpenOrders();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            while (true)
            {
                Thread.Sleep(100);
            }
        }

        static void client_ExecDetails(object sender, ExecDetailsEventArgs e)
        {
            Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                e.Contract.Symbol, e.Execution.AccountNumber, e.Execution.ClientId, e.Execution.Exchange, e.Execution.ExecutionId,
                e.Execution.Liquidation, e.Execution.OrderId, e.Execution.PermId, e.Execution.Price, e.Execution.Shares, e.Execution.Side, e.Execution.Time);
        }

        static void client_RealTimeBar(object sender, RealTimeBarEventArgs e)
        {
            Console.WriteLine("Received Real Time Bar: " + e.Close);
        }

        static void client_OrderStatus(object sender, OrderStatusEventArgs e)
        {
            Console.WriteLine("Order Placed.");
        }

        static void client_UpdateMktDepth(object sender, UpdateMarketDepthEventArgs e)
        {
            Console.WriteLine("Tick ID: " + e.TickerId + " Tick Side: " + EnumDescConverter.GetEnumDescription(e.Side) +
                              " Tick Size: " + e.Size + " Tick Price: " + e.Price + " Tick Position: " + e.Position +
                              " Operation: " + EnumDescConverter.GetEnumDescription(e.Operation));
        }

        static void client_NextValidId(object sender, NextValidIdEventArgs e)
        {
            Console.WriteLine("Next Valid Id: " + e.OrderId);
            NextOrderId = e.OrderId;
        }

        static void client_TickSize(object sender, TickSizeEventArgs e)
        {
            Console.WriteLine("Tick Size: " + e.Size + " Tick Type: " + EnumDescConverter.GetEnumDescription(e.TickType));
        }

        static void client_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("Error: " + e.ErrorMsg);
        }

        static void client_TickPrice(object sender, TickPriceEventArgs e)
        {
            Console.WriteLine("Price: " + e.Price + " Tick Type: " + EnumDescConverter.GetEnumDescription(e.TickType));
        }

    }
}
