using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlexTrade
{
    public partial class PairsTradeParmForm : Form
    {
        MainWindow mainWindow;

        public PairsTradeParmForm(MainWindow win)
        {
            mainWindow = win;
            InitializeComponent();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            Dictionary<PARMS, String> parms = new Dictionary<PARMS, String>();
            parms.Add(PARMS.Ticker1, tickerBox1.Text);
            parms.Add(PARMS.Ticker2, tickerBox2.Text);
            parms.Add(PARMS.BuyQty, buyQtyBox.Text);
            parms.Add(PARMS.SellQty, sellQtyBox.Text);
            parms.Add(PARMS.BuySpread, buySpreadBox.Text);
            parms.Add(PARMS.SellSpread, sellSpreadBox.Text);

            mainWindow.currentParms = parms;
            this.Close();
        }
    }
}
