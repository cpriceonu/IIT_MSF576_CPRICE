namespace FlexTrade
{
    partial class PairsTradeParmForm
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
            this.submitButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tickerBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tickerBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buyQtyBox = new System.Windows.Forms.TextBox();
            this.buySpreadBox = new System.Windows.Forms.TextBox();
            this.sellQtyBox = new System.Windows.Forms.TextBox();
            this.sellSpreadBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(20, 228);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(129, 37);
            this.submitButton.TabIndex = 0;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ticker 1";
            this.label1.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // tickerBox1
            // 
            this.tickerBox1.Location = new System.Drawing.Point(20, 48);
            this.tickerBox1.Name = "tickerBox1";
            this.tickerBox1.Size = new System.Drawing.Size(100, 22);
            this.tickerBox1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ticker 2";
            // 
            // tickerBox2
            // 
            this.tickerBox2.Location = new System.Drawing.Point(140, 48);
            this.tickerBox2.Name = "tickerBox2";
            this.tickerBox2.Size = new System.Drawing.Size(100, 22);
            this.tickerBox2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Buy Qty";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Sell Qty";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(138, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Buy Spread";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(137, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Sell Spread";
            // 
            // buyQtyBox
            // 
            this.buyQtyBox.Location = new System.Drawing.Point(20, 109);
            this.buyQtyBox.Name = "buyQtyBox";
            this.buyQtyBox.Size = new System.Drawing.Size(100, 22);
            this.buyQtyBox.TabIndex = 9;
            // 
            // buySpreadBox
            // 
            this.buySpreadBox.Location = new System.Drawing.Point(140, 109);
            this.buySpreadBox.Name = "buySpreadBox";
            this.buySpreadBox.Size = new System.Drawing.Size(100, 22);
            this.buySpreadBox.TabIndex = 10;
            // 
            // sellQtyBox
            // 
            this.sellQtyBox.Location = new System.Drawing.Point(20, 172);
            this.sellQtyBox.Name = "sellQtyBox";
            this.sellQtyBox.Size = new System.Drawing.Size(100, 22);
            this.sellQtyBox.TabIndex = 11;
            // 
            // sellSpreadBox
            // 
            this.sellSpreadBox.Location = new System.Drawing.Point(140, 172);
            this.sellSpreadBox.Name = "sellSpreadBox";
            this.sellSpreadBox.Size = new System.Drawing.Size(100, 22);
            this.sellSpreadBox.TabIndex = 12;
            // 
            // PairsTradeParmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 305);
            this.Controls.Add(this.sellSpreadBox);
            this.Controls.Add(this.sellQtyBox);
            this.Controls.Add(this.buySpreadBox);
            this.Controls.Add(this.buyQtyBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tickerBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tickerBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.submitButton);
            this.Name = "PairsTradeParmForm";
            this.Text = "PairsTradeParmForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tickerBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tickerBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox buyQtyBox;
        private System.Windows.Forms.TextBox buySpreadBox;
        private System.Windows.Forms.TextBox sellQtyBox;
        private System.Windows.Forms.TextBox sellSpreadBox;
    }
}