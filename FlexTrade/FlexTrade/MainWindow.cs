using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlexTrade
{
    delegate void UpdateOrderItemDelegate(OrderGridData item);
    delegate void RemoveOrderItemDelegate(OrderGridData item);
    delegate void UpdatePositionItemDelegate(PositionGridData item);

    public partial class MainWindow : Form
    {
        public BindingList<OrderGridData> orderGrid { get; set; }
        public BindingList<PositionGridData> positionGrid { get; set; }

        private double cummulativePnL;
        private int seconds;
        Random m_random;
        System.Windows.Forms.Timer m_Timer;

        public MainWindow()
        {
            InitializeComponent();

            cummulativePnL = 0.0;
            seconds = 0;
            orderGrid = new BindingList<OrderGridData>();
            positionGrid = new BindingList<PositionGridData>();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void positionManagerBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //Bind data elements to the tables on the UI so that the tables automatically update when the data changes
            BindingSource orderGridSource = new BindingSource();
            orderGridSource.DataSource = orderGrid;
            dataGridView1.DataSource = orderGridSource;

            BindingSource positionGridSource = new BindingSource();
            positionGridSource.DataSource = positionGridSource;
            dataGridView2.DataSource = positionGridSource;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void uIControllerBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void uIControllerBindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        public void newPnLValue(double val)
        {
            cummulativePnL = val;
        }

        public void addUpdateOrder(OrderGridData ord)
        {
            dataGridView1.Invoke(new UpdateOrderItemDelegate(this.updateOrderItemToGrid), ord);
        }

        public void removeOrder(OrderGridData ord)
        {
            dataGridView1.Invoke(new RemoveOrderItemDelegate(this.removeOrderItemFromGrid), ord);
        }

        public void updatePosition(PositionGridData pos)
        {
            dataGridView2.Invoke(new UpdatePositionItemDelegate(this.updatePositionItemInGrid), pos);
        }

        //##################################################################3
        //Create a separate methods because the UI thread doesnt like when you access data on its stack
        //Called by a delegate instead by calling the Invoke method above
        private void updateOrderItemToGrid(OrderGridData item) 
        {
            if (orderGrid.Contains(item))
            {
                orderGrid.Remove(item);
                orderGrid.Insert(0, item);
            }
            else
                orderGrid.Insert(0, item);
        }
        private void removeOrderItemFromGrid(OrderGridData item) {    orderGrid.Remove(item); }
        private void updatePositionItemInGrid(PositionGridData p) 
        {
            if (positionGrid.Contains(p))
                positionGrid.Remove(p);

            positionGrid.Add(p);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            m_Timer = new System.Windows.Forms.Timer();
            m_Timer.Interval = 1000;
            m_Timer.Tick += new EventHandler(timerTick);
            m_Timer.Enabled = true;    
        }

        private void exitButton_Click(object sender, EventArgs e)
        {

        }

        private void timerTick(Object o, EventArgs e)
        {
            chart1.Series[0].Points.AddXY(seconds, cummulativePnL);
            chart1.ChartAreas[0].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
           //  chart1.ChartAreas[0].AxisY.Minimum = 0;
            //chart1.ChartAreas[0].AxisX.Minimum = 0;
            //double removeBefore = DateTime.Now.AddSeconds(-45.0).ToOADate();
            //while( chart1.Series[0].Points[0].XValue < removeBefore )
            while (chart1.Series[0].Points.Count > 50)
            {
                chart1.Series[0].Points.RemoveAt(0);
            }
            chart1.ChartAreas[0].AxisX.Minimum = chart1.Series[0].Points[0].XValue;
            chart1.ChartAreas[0].AxisX.Maximum = chart1.Series[0].Points[0].XValue + 50;
            chart1.Invalidate();

            seconds++;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

    }
}
