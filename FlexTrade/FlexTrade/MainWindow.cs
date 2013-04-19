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
    public delegate void StrategyStartDelegate(int id, Dictionary<PARMS, String> parmMap);
    public delegate void StrategyStopDelegate();

    public partial class MainWindow : Form
    {
        private delegate void UpdateOrderItemDelegate(OrderGridData item);
        private delegate void RemoveOrderItemDelegate(OrderGridData item);
        private delegate void UpdatePositionItemDelegate(PositionGridData item);
        private delegate void UpdatePriceItemDelegate(PositionGridData item);
        private delegate void AddMessagesDelegate(List<String> msgs);

        public event StrategyStartDelegate StrategyStart;
        public event StrategyStopDelegate StrategyStop;

        public BindingList<OrderGridData> orderGrid { get; set; }
        public BindingList<PositionGridData> positionGrid { get; set; }

        public Dictionary<PARMS, String> currentParms { get; set; }

        private Dictionary<int, String> _availableStrategies;
        public Dictionary<int, String> availableStrategies
        {
            get { return _availableStrategies; }
            set
            {
                _availableStrategies = value;
                foreach (String s in _availableStrategies.Values)
                    stategySelection.Items.Add(s);

                stategySelection.Refresh();
            }

        }

        private double cummulativePnL;
        private int seconds;
        private bool started = false;
        System.Windows.Forms.Timer m_Timer;

        public MainWindow()
        {
            InitializeComponent();

            cummulativePnL = 0.0;
            seconds = 0;
            orderGrid = new BindingList<OrderGridData>();
            positionGrid = new BindingList<PositionGridData>();
            _availableStrategies = new Dictionary<int, String>();
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
            orderGridView.DataSource = orderGridSource;
            orderGridView.Columns[0].Width = 5;
            orderGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            BindingSource positionGridSource = new BindingSource();
            positionGridSource.DataSource = positionGrid;
            positionGridView.DataSource = positionGridSource;
            positionGridView.Columns[2].DefaultCellStyle.Format = "C";
            positionGridView.Columns[3].DefaultCellStyle.Format = "C";
            positionGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
            orderGridView.Invoke(new UpdateOrderItemDelegate(this.updateOrderItemToGrid), ord);
        }

        public void removeOrder(OrderGridData ord)
        {
            orderGridView.Invoke(new RemoveOrderItemDelegate(this.removeOrderItemFromGrid), ord);
        }

        public void updatePosition(PositionGridData pos)
        {
            positionGridView.Invoke(new UpdatePositionItemDelegate(this.updatePositionItemInGrid), pos);
        }

        public void updatePrice(PositionGridData pos)
        {
            positionGridView.Invoke(new UpdatePriceItemDelegate(this.updatePriceItemInGrid), pos);
        }

        public void addMessages(List<String> messages)
        {
            positionGridView.Invoke(new AddMessagesDelegate(this.addMessagesToTable), messages);
        }

        //##################################################################3
        //Create a separate methods because the UI thread doesnt like when you access data on its stack
        //Called by a delegate instead by calling the Invoke method above
        private void updateOrderItemToGrid(OrderGridData item) 
        {
            if (orderGrid.Contains(item))
                orderGrid.Remove(item);
            
            orderGrid.Insert(0, item);
            orderGridView.Refresh();
        }
        private void removeOrderItemFromGrid(OrderGridData item) 
        {    
            orderGrid.Remove(item);
            orderGridView.Refresh();
        }
        private void updatePositionItemInGrid(PositionGridData p) 
        {
            if (positionGrid.Contains(p))
            {
                int i = positionGrid.IndexOf(p);
                positionGrid[i].position = p.position;
                positionGrid[i].value = positionGrid[i].last * p.position;
            }
            else
                positionGrid.Insert(0, p);

            positionGridView.Refresh();
        }
        private void updatePriceItemInGrid(PositionGridData p)
        {
            if (positionGrid.Contains(p))
            {   
                int i = positionGrid.IndexOf(p);
                positionGrid[i].last = p.last;
                positionGrid[i].value = positionGrid[i].position * p.last;
            }
            positionGridView.Refresh();
        }
        private void addMessagesToTable(List<String> msgs)
        {
            foreach(DataGridViewRow row in messageGridView.Rows)
                row.Cells[0].Style.BackColor = Color.White;

            foreach (String m in msgs)
            {
                messageGridView.Rows.Add(m);
                int numRows = messageGridView.Rows.Count;
                messageGridView.CurrentCell = messageGridView[0, numRows - 2];
                messageGridView.Rows[numRows - 2].Cells[0].Style.BackColor = Color.Red;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            errorMessageLabel.Visible = false;

            if (!started && stategySelection.SelectedItem != null)
            {
                started = true;

                m_Timer = new System.Windows.Forms.Timer();
                m_Timer.Interval = 1000;
                m_Timer.Tick += new EventHandler(timerTick);
                m_Timer.Enabled = true;

                if (StrategyStart != null)
                    StrategyStart(stategySelection.SelectedIndex, currentParms);

                startButton.Enabled = false;
                exitButton.Enabled = true;
            }
            if(stategySelection.SelectedItem == null)
            {
                errorMessageLabel.Text = "Please select a strategy";
                errorMessageLabel.Visible = true;
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            if (started)
            {
                started = false;
                startButton.Enabled = true;
                exitButton.Enabled = false;

                if (StrategyStop != null)
                    StrategyStop();
            }
        }

        private void parameterButton_Click(object sender, EventArgs e)
        {
            errorMessageLabel.Visible = false;

            if (stategySelection.SelectedItem == null)
            {
                errorMessageLabel.Text = "Please select a strategy";
                errorMessageLabel.Visible = true;
            }
            else
            {
                Form parmWin;
                switch (stategySelection.SelectedIndex)
                {
                    case 0:
                        parmWin = new SimpleOneTickerParmForm(this);
                        parmWin.Visible = true;
                        break;
                    case 1:
                        parmWin = new PairsTradeParmForm(this);
                        parmWin.Visible = true;
                        break;
                }
            }
        }

        private void timerTick(Object o, EventArgs e)
        {
            pnLChart.Series[0].Points.AddXY(seconds, cummulativePnL);
            pnLChart.ChartAreas[0].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;

            while (pnLChart.Series[0].Points.Count > 60)
            {
                pnLChart.Series[0].Points.RemoveAt(0);
            }
            pnLChart.ChartAreas[0].AxisX.Minimum = pnLChart.Series[0].Points[0].XValue;
            pnLChart.ChartAreas[0].AxisX.Maximum = pnLChart.Series[0].Points[0].XValue + 60;
            pnLChart.ChartAreas[0].RecalculateAxesScale();
            pnLChart.Invalidate();

            seconds++;
        }
    }
}
