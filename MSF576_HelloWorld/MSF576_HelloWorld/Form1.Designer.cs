namespace MSF576_HelloWorld
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.shutdownButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.buyMarketButton = new System.Windows.Forms.Button();
            this.sellMarketButton = new System.Windows.Forms.Button();
            this.buyLimitButton = new System.Windows.Forms.Button();
            this.sellLimitButton = new System.Windows.Forms.Button();
            this.bidQtyText = new System.Windows.Forms.TextBox();
            this.bidPxText = new System.Windows.Forms.TextBox();
            this.askPxText = new System.Windows.Forms.TextBox();
            this.askQtyText = new System.Windows.Forms.TextBox();
            this.lastPxText = new System.Windows.Forms.TextBox();
            this.lastQtyText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bidPxLabel = new System.Windows.Forms.Label();
            this.askPxLabel = new System.Windows.Forms.Label();
            this.askQtyLabel = new System.Windows.Forms.Label();
            this.lastPxLabel = new System.Windows.Forms.Label();
            this.lastQtyLabel = new System.Windows.Forms.Label();
            this.cancelAllButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cancelOrderText = new System.Windows.Forms.TextBox();
            this.limitPriceText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.orderLogText = new System.Windows.Forms.TextBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // shutdownButton
            // 
            this.shutdownButton.Location = new System.Drawing.Point(444, 30);
            this.shutdownButton.Name = "shutdownButton";
            this.shutdownButton.Size = new System.Drawing.Size(100, 84);
            this.shutdownButton.TabIndex = 0;
            this.shutdownButton.Text = "Shutdown";
            this.shutdownButton.UseVisualStyleBackColor = true;
            this.shutdownButton.Click += new System.EventHandler(this.shutdown);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(38, 30);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(100, 84);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.startButton_Click);
            this.StartButton.Click += new System.EventHandler(this.drawChart);

            // 
            // buyMarketButton
            // 
            this.buyMarketButton.Location = new System.Drawing.Point(153, 30);
            this.buyMarketButton.Name = "buyMarketButton";
            this.buyMarketButton.Size = new System.Drawing.Size(125, 34);
            this.buyMarketButton.TabIndex = 2;
            this.buyMarketButton.Text = "Buy At Market";
            this.buyMarketButton.UseVisualStyleBackColor = true;
            this.buyMarketButton.Click += new System.EventHandler(this.buyMarket);
            // 
            // sellMarketButton
            // 
            this.sellMarketButton.Location = new System.Drawing.Point(153, 79);
            this.sellMarketButton.Name = "sellMarketButton";
            this.sellMarketButton.Size = new System.Drawing.Size(125, 35);
            this.sellMarketButton.TabIndex = 3;
            this.sellMarketButton.Text = "Sell At Market";
            this.sellMarketButton.UseVisualStyleBackColor = true;
            this.sellMarketButton.Click += new System.EventHandler(this.sellMarket);
            // 
            // buyLimitButton
            // 
            this.buyLimitButton.Location = new System.Drawing.Point(298, 30);
            this.buyLimitButton.Name = "buyLimitButton";
            this.buyLimitButton.Size = new System.Drawing.Size(125, 34);
            this.buyLimitButton.TabIndex = 4;
            this.buyLimitButton.Text = "Buy At Limit";
            this.buyLimitButton.UseVisualStyleBackColor = true;
            this.buyLimitButton.Click += new System.EventHandler(this.buyLimit);
            // 
            // sellLimitButton
            // 
            this.sellLimitButton.Location = new System.Drawing.Point(298, 79);
            this.sellLimitButton.Name = "sellLimitButton";
            this.sellLimitButton.Size = new System.Drawing.Size(125, 34);
            this.sellLimitButton.TabIndex = 5;
            this.sellLimitButton.Text = "Sell At Limit";
            this.sellLimitButton.UseVisualStyleBackColor = true;
            this.sellLimitButton.Click += new System.EventHandler(this.sellLimit);
            // 
            // bidQtyText
            // 
            this.bidQtyText.Location = new System.Drawing.Point(38, 169);
            this.bidQtyText.Name = "bidQtyText";
            this.bidQtyText.Size = new System.Drawing.Size(76, 22);
            this.bidQtyText.TabIndex = 6;
            // 
            // bidPxText
            // 
            this.bidPxText.Location = new System.Drawing.Point(120, 169);
            this.bidPxText.Name = "bidPxText";
            this.bidPxText.Size = new System.Drawing.Size(76, 22);
            this.bidPxText.TabIndex = 7;
            // 
            // askPxText
            // 
            this.askPxText.Location = new System.Drawing.Point(202, 169);
            this.askPxText.Name = "askPxText";
            this.askPxText.Size = new System.Drawing.Size(76, 22);
            this.askPxText.TabIndex = 8;
            // 
            // askQtyText
            // 
            this.askQtyText.Location = new System.Drawing.Point(284, 169);
            this.askQtyText.Name = "askQtyText";
            this.askQtyText.Size = new System.Drawing.Size(76, 22);
            this.askQtyText.TabIndex = 9;
            // 
            // lastPxText
            // 
            this.lastPxText.Location = new System.Drawing.Point(366, 169);
            this.lastPxText.Name = "lastPxText";
            this.lastPxText.Size = new System.Drawing.Size(76, 22);
            this.lastPxText.TabIndex = 10;
            // 
            // lastQtyText
            // 
            this.lastQtyText.Location = new System.Drawing.Point(448, 169);
            this.lastQtyText.Name = "lastQtyText";
            this.lastQtyText.Size = new System.Drawing.Size(76, 22);
            this.lastQtyText.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "Bid Qty";
            // 
            // bidPxLabel
            // 
            this.bidPxLabel.AutoSize = true;
            this.bidPxLabel.Location = new System.Drawing.Point(135, 149);
            this.bidPxLabel.Name = "bidPxLabel";
            this.bidPxLabel.Size = new System.Drawing.Size(47, 17);
            this.bidPxLabel.TabIndex = 13;
            this.bidPxLabel.Text = "Bid Px";
            // 
            // askPxLabel
            // 
            this.askPxLabel.AutoSize = true;
            this.askPxLabel.Location = new System.Drawing.Point(217, 149);
            this.askPxLabel.Name = "askPxLabel";
            this.askPxLabel.Size = new System.Drawing.Size(50, 17);
            this.askPxLabel.TabIndex = 14;
            this.askPxLabel.Text = "Ask Px";
            // 
            // askQtyLabel
            // 
            this.askQtyLabel.AutoSize = true;
            this.askQtyLabel.Location = new System.Drawing.Point(295, 149);
            this.askQtyLabel.Name = "askQtyLabel";
            this.askQtyLabel.Size = new System.Drawing.Size(53, 17);
            this.askQtyLabel.TabIndex = 15;
            this.askQtyLabel.Text = "AskQty";
            // 
            // lastPxLabel
            // 
            this.lastPxLabel.AutoSize = true;
            this.lastPxLabel.Location = new System.Drawing.Point(377, 149);
            this.lastPxLabel.Name = "lastPxLabel";
            this.lastPxLabel.Size = new System.Drawing.Size(54, 17);
            this.lastPxLabel.TabIndex = 16;
            this.lastPxLabel.Text = "Last Px";
            // 
            // lastQtyLabel
            // 
            this.lastQtyLabel.AutoSize = true;
            this.lastQtyLabel.Location = new System.Drawing.Point(454, 149);
            this.lastQtyLabel.Name = "lastQtyLabel";
            this.lastQtyLabel.Size = new System.Drawing.Size(61, 17);
            this.lastQtyLabel.TabIndex = 17;
            this.lastQtyLabel.Text = "Last Qty";
            // 
            // cancelAllButton
            // 
            this.cancelAllButton.Location = new System.Drawing.Point(38, 213);
            this.cancelAllButton.Name = "cancelAllButton";
            this.cancelAllButton.Size = new System.Drawing.Size(126, 34);
            this.cancelAllButton.TabIndex = 18;
            this.cancelAllButton.Text = "Cancel All Orders";
            this.cancelAllButton.UseVisualStyleBackColor = true;
            this.cancelAllButton.Click += new System.EventHandler(this.cancelAll);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(170, 213);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 34);
            this.button1.TabIndex = 19;
            this.button1.Text = "Cancel Order";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.cancelOrder);
            // 
            // cancelOrderText
            // 
            this.cancelOrderText.Location = new System.Drawing.Point(332, 225);
            this.cancelOrderText.Name = "cancelOrderText";
            this.cancelOrderText.Size = new System.Drawing.Size(110, 22);
            this.cancelOrderText.TabIndex = 20;
            // 
            // limitPriceText
            // 
            this.limitPriceText.Location = new System.Drawing.Point(464, 225);
            this.limitPriceText.Name = "limitPriceText";
            this.limitPriceText.Size = new System.Drawing.Size(80, 22);
            this.limitPriceText.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(329, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "Cancel Order ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(471, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 23;
            this.label3.Text = "Limit Price";
            // 
            // orderLogText
            // 
            this.orderLogText.Location = new System.Drawing.Point(38, 272);
            this.orderLogText.Multiline = true;
            this.orderLogText.Name = "orderLogText";
            this.orderLogText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.orderLogText.Size = new System.Drawing.Size(506, 165);
            this.orderLogText.TabIndex = 24;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(38, 452);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(506, 229);
            this.chart1.TabIndex = 25;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 706);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.orderLogText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.limitPriceText);
            this.Controls.Add(this.cancelOrderText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cancelAllButton);
            this.Controls.Add(this.lastQtyLabel);
            this.Controls.Add(this.lastPxLabel);
            this.Controls.Add(this.askQtyLabel);
            this.Controls.Add(this.askPxLabel);
            this.Controls.Add(this.bidPxLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lastQtyText);
            this.Controls.Add(this.lastPxText);
            this.Controls.Add(this.askQtyText);
            this.Controls.Add(this.askPxText);
            this.Controls.Add(this.bidPxText);
            this.Controls.Add(this.bidQtyText);
            this.Controls.Add(this.sellLimitButton);
            this.Controls.Add(this.buyLimitButton);
            this.Controls.Add(this.sellMarketButton);
            this.Controls.Add(this.buyMarketButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.shutdownButton);
            this.Name = "Form1";
            this.Text = "MSF576 Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button shutdownButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button buyMarketButton;
        private System.Windows.Forms.Button sellMarketButton;
        private System.Windows.Forms.Button buyLimitButton;
        private System.Windows.Forms.Button sellLimitButton;
        private System.Windows.Forms.TextBox bidQtyText;
        private System.Windows.Forms.TextBox bidPxText;
        private System.Windows.Forms.TextBox askPxText;
        private System.Windows.Forms.TextBox askQtyText;
        private System.Windows.Forms.TextBox lastPxText;
        private System.Windows.Forms.TextBox lastQtyText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label bidPxLabel;
        private System.Windows.Forms.Label askPxLabel;
        private System.Windows.Forms.Label askQtyLabel;
        private System.Windows.Forms.Label lastPxLabel;
        private System.Windows.Forms.Label lastQtyLabel;
        private System.Windows.Forms.Button cancelAllButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox cancelOrderText;
        private System.Windows.Forms.TextBox limitPriceText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox orderLogText;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;


    }
}

