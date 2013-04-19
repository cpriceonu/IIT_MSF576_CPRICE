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
    public partial class SimpleOneTickerParmForm : Form
    {
        MainWindow mainWindow;

        public SimpleOneTickerParmForm(MainWindow win)
        {
            mainWindow = win;
            InitializeComponent();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            errorMessage.Visible = false;

            if (tickerBox.Text == null || tickerBox.Text.Length == 0 ||
                qtyBox.Text == null || qtyBox.Text.Length == 0)
            {
                errorMessage.Visible = true;
                errorMessage.Text = "Please enter all parameters";
            }
            else
            {
                Dictionary<PARMS, String> parms = new Dictionary<PARMS, String>();
                parms.Add(PARMS.Ticker1, tickerBox.Text);
                parms.Add(PARMS.BuyQty, qtyBox.Text);

                mainWindow.currentParms = parms;
                this.Close();
            }
        }
    }
}
