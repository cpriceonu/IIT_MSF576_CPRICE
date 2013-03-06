using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraderAPI;

namespace MSF576_HelloWorld
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private InstrNotifyClass m_Notify;
        private InstrObjClass m_Instr;
        private OrderSetClass m_OrderSet;
        Random m_random;
        Timer m_Timer;

        private void startButton_Click(System.Object sender, System.EventArgs e)
        {
            // Create a new InstrObjClass object
            m_Instr = new InstrObjClass();
            // Create a new InstrNotifyClass object from the InstrObjClass object.
            m_Notify = ( InstrNotifyClass )m_Instr.CreateNotifyObj;
            // Enable price updates.
            m_Notify.EnablePriceUpdates = true;
            // Set UpdateFilter so event will fire anytime any one of these changes in the
            // associated InstrObjClass object.
            m_Notify.UpdateFilter = "BIDQTY,BID,ASK,ASKQTY,LAST,LASTQTY";
            // Subscribe to the OnNotifyUpdate event.
            m_Notify.OnNotifyUpdate += new
            InstrNotifyClass.OnNotifyUpdateEventHandler(this.OnUpdate);
            // Set the exchange, product, contract and product type.
            m_Instr.Exchange = "CME";
            m_Instr.Product = "ES";
            m_Instr.Contract = "Dec12";
            m_Instr.ProdType = "FUTURE";
            // Open m_Instr.
            m_Instr.Open(true);
            // Create a new OrderSetClass object.
            m_OrderSet = new OrderSetClass();
            // Set the limits accordingly. If any of these limits is reached,
            // trading through the API will be shut down automatically.
            m_OrderSet.set_Set("MAXORDERS", 1000);
            m_OrderSet.set_Set("MAXORDERQTY", 1000);
            m_OrderSet.set_Set("MAXWORKING", 1000);
            m_OrderSet.set_Set("MAXPOSITION", 1000);
            // Enable deleting of orders. Enable the OnOrderFillData event. Enable order sending.
            m_OrderSet.EnableOrderAutoDelete = true;
            m_OrderSet.EnableOrderFillData = true;
            m_OrderSet.EnableOrderSend = true;
            // Subscribe to the OnOrderFillData event.
            m_OrderSet.OnOrderFillData += new
            OrderSetClass.OnOrderFillDataEventHandler(this.OnFill);
            // Open the m_OrderSet.
            m_OrderSet.Open(true);
            // Associate m_OrderSet with m_Instr.
            m_Instr.OrderSet = m_OrderSet;
        }
        private void OnFill( FillObj pFill )
        {
            //Get fill data here with chatty calls. Chunky calls is faster.
            orderLogText.Text += String.Concat( "FILL RECEIVED: ",
            pFill.get_Get( "FFT3" ).ToString(), " ID#: ",
            pFill.get_Get( "KEY" ).ToString(), " Price: ",
            pFill.get_Get( "PRICE" ).ToString(), " B/S: ",
            pFill.get_Get( "BUYSELL" ).ToString(), " QTY: ",
            pFill.get_Get( "QTY" ).ToString(), Environment.NewLine );
        }
        private void OnUpdate( InstrNotifyClass pNotify, InstrObjClass pInstr )
        {
            // Get new data from the InstrObjClass using chatty calls here.
            bidQtyText.Text = pInstr.get_Get( "BIDQTY" ).ToString();
            bidPxText.Text = pInstr.get_Get("BID").ToString();
            askPxText.Text = pInstr.get_Get("ASK").ToString();
            askQtyText.Text = pInstr.get_Get("ASKQTY").ToString();
            lastPxText.Text = pInstr.get_Get("LAST").ToString();
            lastQtyText.Text = pInstr.get_Get("LASTQTY").ToString();
        }
        private void SendMarketOrder( String m_BS )
        {
            // Create an OrderProfileClass object to contain information about a market order.
            OrderProfileClass m_Profile = new OrderProfileClass();
            m_Profile.Instrument = m_Instr;
            m_Profile.set_Set( "ACCT", "12345" );
            m_Profile.set_Set( "BUYSELL", m_BS );
            m_Profile.set_Set( "ORDERTYPE", "M" );
            m_Profile.set_Set( "ORDERQTY", Convert.ToString( 6 ) );
            m_Profile.set_Set( "FFT3", "MKT ORDER" );
            // Send the order through m_OrderSet.
            Int64 m_Result = m_OrderSet.SendOrder( m_Profile );
        }
        private void SendLimitOrder( String m_BS, String m_Px )
        {
            // Send a limit order here.
            OrderProfileClass m_Profile = new OrderProfileClass();
            m_Profile.Instrument = m_Instr;
            m_Profile.set_Set("ACCT", "12345");
            m_Profile.set_Set("BUYSELL", m_BS);
            m_Profile.set_Set("ORDERTYPE", "L");
            m_Profile.set_Set("LIMIT", m_Px);
            m_Profile.set_Set("ORDERQTY", Convert.ToString(6));
            m_Profile.set_Set("FFT3", "LMT ORDER");
            Int64 m_Result = m_OrderSet.SendOrder(m_Profile);
        }
        private void shutdown(object sender, EventArgs e)
        {
            // Shut down should include explicit object destruction.
            m_Notify.OnNotifyUpdate -= new InstrNotifyClass.OnNotifyUpdateEventHandler(this.OnUpdate);
            m_OrderSet.OnOrderFillData -= new OrderSetClass.OnOrderFillDataEventHandler(this.OnFill);
            m_Instr = null;
            m_OrderSet = null;
            m_Notify = null;
            GC.Collect();
        }
        private void cancelAll(object sender, EventArgs e)
        {
            // Delete all working buys and sells.
            Int64 m_Long = m_OrderSet.DeleteOrders(true);
            m_Long = m_OrderSet.DeleteOrders(false);
        }
        private void sellLimit(object sender, EventArgs e)
        {
        SendLimitOrder("S", orderLogText.Text);
        }
        private void buyLimit(object sender, EventArgs e)
        {
        SendLimitOrder("B", orderLogText.Text);
        }
        private void buyMarket(object sender, EventArgs e)
        {
        SendMarketOrder("B");
        }
        private void sellMarket(object sender, EventArgs e)
        {
        SendMarketOrder("S");
        }
        private void cancelOrder(object sender, EventArgs e)
        {
        m_OrderSet.Cancel(orderLogText.Text);
        }
        private void drawChart(object sender, EventArgs e)
        {
            m_Timer = new Timer();
            m_Timer.Interval = 1000;
            m_Timer.Tick += new EventHandler(m_Timer_Tick);
            m_Timer.Enabled = true;
            m_random = new Random();
        }
        private void m_Timer_Tick(Object o, EventArgs e)
        {
            chart1.Series[0].Points.AddXY(DateTime.Now.ToOADate(), m_random.Next( 100 ) );
            chart1.ChartAreas[0].AxisY.Maximum = 100;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            double removeBefore = DateTime.Now.AddSeconds(-45.0).ToOADate();
            while( chart1.Series[0].Points[0].XValue < removeBefore )
            {
            chart1.Series[0].Points.RemoveAt(0);
            }
            chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points[0].XValue;
            chart1.ChartAreas[0].AxisX.Maximum =
            DateTime.FromOADate(chart1.Series[0].Points[0].XValue).AddSeconds(50).ToOADate();
            chart1.Invalidate();
        }
    }
}
