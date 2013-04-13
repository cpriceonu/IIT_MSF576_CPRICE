using System.Windows.Forms;
using FlexTrade;
namespace FlexTrade
{
    partial class MainWindow 
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.orderGridView = new System.Windows.Forms.DataGridView();
            this.positionGridView = new System.Windows.Forms.DataGridView();
            this.startButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.stategySelection = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.parameterButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pnLChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.errorMessageLabel = new System.Windows.Forms.Label();
            this.mainWindowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.orderGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnLChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainWindowBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // orderGridView
            // 
            this.orderGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.orderGridView.Location = new System.Drawing.Point(32, 355);
            this.orderGridView.Name = "orderGridView";
            this.orderGridView.RowTemplate.Height = 24;
            this.orderGridView.Size = new System.Drawing.Size(545, 243);
            this.orderGridView.TabIndex = 0;
            this.orderGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick_1);
            // 
            // positionGridView
            // 
            this.positionGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.positionGridView.Location = new System.Drawing.Point(622, 355);
            this.positionGridView.Name = "positionGridView";
            this.positionGridView.RowTemplate.Height = 24;
            this.positionGridView.Size = new System.Drawing.Size(601, 243);
            this.positionGridView.TabIndex = 1;
            this.positionGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(52, 113);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(116, 71);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(203, 113);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(116, 71);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // stategySelection
            // 
            this.stategySelection.FormattingEnabled = true;
            this.stategySelection.Location = new System.Drawing.Point(53, 69);
            this.stategySelection.Name = "stategySelection";
            this.stategySelection.Size = new System.Drawing.Size(246, 24);
            this.stategySelection.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select a strategy:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // parameterButton
            // 
            this.parameterButton.Location = new System.Drawing.Point(315, 69);
            this.parameterButton.Name = "parameterButton";
            this.parameterButton.Size = new System.Drawing.Size(172, 24);
            this.parameterButton.TabIndex = 6;
            this.parameterButton.Text = "Enter parameters";
            this.parameterButton.UseVisualStyleBackColor = true;
            this.parameterButton.Click += new System.EventHandler(this.parameterButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 17);
            this.label2.TabIndex = 7;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // pnLChart
            // 
            chartArea1.Name = "ChartArea1";
            this.pnLChart.ChartAreas.Add(chartArea1);
            this.pnLChart.Location = new System.Drawing.Point(622, 69);
            this.pnLChart.Name = "pnLChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "$ P/L";
            this.pnLChart.Series.Add(series1);
            this.pnLChart.Size = new System.Drawing.Size(601, 229);
            this.pnLChart.TabIndex = 8;
            this.pnLChart.TabStop = false;
            this.pnLChart.Text = "chart1";
            this.pnLChart.Click += new System.EventHandler(this.chart1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(29, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 25);
            this.label3.TabIndex = 9;
            this.label3.Text = "Orders";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(619, 320);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 25);
            this.label4.TabIndex = 10;
            this.label4.Text = "Positions";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(617, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(359, 25);
            this.label5.TabIndex = 11;
            this.label5.Text = "$ P/L per Share (one second resolution)";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // errorMessageLabel
            // 
            this.errorMessageLabel.AutoSize = true;
            this.errorMessageLabel.BackColor = System.Drawing.SystemColors.Control;
            this.errorMessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorMessageLabel.ForeColor = System.Drawing.Color.Red;
            this.errorMessageLabel.Location = new System.Drawing.Point(48, 201);
            this.errorMessageLabel.Name = "errorMessageLabel";
            this.errorMessageLabel.Size = new System.Drawing.Size(47, 18);
            this.errorMessageLabel.TabIndex = 12;
            this.errorMessageLabel.Text = "Error";
            this.errorMessageLabel.Visible = false;
            this.errorMessageLabel.Click += new System.EventHandler(this.label6_Click);
            // 
            // mainWindowBindingSource
            // 
            this.mainWindowBindingSource.DataSource = typeof(FlexTrade.MainWindow);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 633);
            this.Controls.Add(this.errorMessageLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pnLChart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.parameterButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stategySelection);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.positionGridView);
            this.Controls.Add(this.orderGridView);
            this.Name = "MainWindow";
            this.Text = "FlexTrade by BackToTheFutures";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.orderGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnLChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainWindowBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView orderGridView;
        private System.Windows.Forms.DataGridView positionGridView;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.ComboBox stategySelection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button parameterButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataVisualization.Charting.Chart pnLChart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private BindingSource mainWindowBindingSource;
        private Label errorMessageLabel;

    }
}

