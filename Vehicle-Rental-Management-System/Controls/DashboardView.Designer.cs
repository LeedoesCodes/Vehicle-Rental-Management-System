namespace Vehicle_Rental_Management_System.Controls
{
    partial class DashboardView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pnlCards = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCardTotal = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.pnlCardAvailable = new System.Windows.Forms.Panel();
            this.labeltitle = new System.Windows.Forms.Label();
            this.lblAvailableValue = new System.Windows.Forms.Label();
            this.pnlCardRented = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRentedValue = new System.Windows.Forms.Label();
            this.pnlCardRevenue = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblRevenueValue = new System.Windows.Forms.Label();
            this.pnlCardOverdue = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.lblOverdueValue = new System.Windows.Forms.Label();
            this.bottomSplit = new System.Windows.Forms.TableLayoutPanel();
            this.chartContainer = new System.Windows.Forms.Panel();
            this.chartLayout = new System.Windows.Forms.TableLayoutPanel();
            this.chartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.gridContainer = new System.Windows.Forms.Panel();
            this.dgvOverdue = new System.Windows.Forms.DataGridView();
            this.lblGridTitle = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pnlCards.SuspendLayout();
            this.pnlCardTotal.SuspendLayout();
            this.pnlCardAvailable.SuspendLayout();
            this.pnlCardRented.SuspendLayout();
            this.pnlCardRevenue.SuspendLayout();
            this.pnlCardOverdue.SuspendLayout();
            this.bottomSplit.SuspendLayout();
            this.chartContainer.SuspendLayout();
            this.chartLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).BeginInit();
            this.gridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOverdue)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCards
            // 
            this.pnlCards.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCards.BackColor = System.Drawing.Color.Transparent;
            this.pnlCards.Controls.Add(this.pnlCardTotal);
            this.pnlCards.Controls.Add(this.pnlCardAvailable);
            this.pnlCards.Controls.Add(this.pnlCardRented);
            this.pnlCards.Controls.Add(this.pnlCardRevenue);
            this.pnlCards.Controls.Add(this.pnlCardOverdue);
            this.pnlCards.Location = new System.Drawing.Point(13, 12);
            this.pnlCards.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlCards.Name = "pnlCards";
            this.pnlCards.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCards.Size = new System.Drawing.Size(1040, 172);
            this.pnlCards.TabIndex = 0;
            // 
            // pnlCardTotal
            // 
            this.pnlCardTotal.BackColor = System.Drawing.Color.DarkGray;
            this.pnlCardTotal.Controls.Add(this.label1);
            this.pnlCardTotal.Controls.Add(this.lblTotalValue);
            this.pnlCardTotal.ForeColor = System.Drawing.Color.White;
            this.pnlCardTotal.Location = new System.Drawing.Point(26, 24);
            this.pnlCardTotal.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCardTotal.Name = "pnlCardTotal";
            this.pnlCardTotal.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCardTotal.Size = new System.Drawing.Size(160, 123);
            this.pnlCardTotal.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Total Fleet";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalValue.Location = new System.Drawing.Point(52, 31);
            this.lblTotalValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(46, 54);
            this.lblTotalValue.TabIndex = 0;
            this.lblTotalValue.Text = "0";
            this.lblTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCardAvailable
            // 
            this.pnlCardAvailable.BackColor = System.Drawing.Color.Green;
            this.pnlCardAvailable.Controls.Add(this.labeltitle);
            this.pnlCardAvailable.Controls.Add(this.lblAvailableValue);
            this.pnlCardAvailable.ForeColor = System.Drawing.Color.White;
            this.pnlCardAvailable.Location = new System.Drawing.Point(212, 24);
            this.pnlCardAvailable.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCardAvailable.Name = "pnlCardAvailable";
            this.pnlCardAvailable.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCardAvailable.Size = new System.Drawing.Size(160, 123);
            this.pnlCardAvailable.TabIndex = 1;
            // 
            // labeltitle
            // 
            this.labeltitle.AutoSize = true;
            this.labeltitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labeltitle.Location = new System.Drawing.Point(8, 10);
            this.labeltitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labeltitle.Name = "labeltitle";
            this.labeltitle.Size = new System.Drawing.Size(145, 23);
            this.labeltitle.TabIndex = 1;
            this.labeltitle.Text = "Vehicle Available";
            this.labeltitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAvailableValue
            // 
            this.lblAvailableValue.AutoSize = true;
            this.lblAvailableValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableValue.Location = new System.Drawing.Point(59, 31);
            this.lblAvailableValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAvailableValue.Name = "lblAvailableValue";
            this.lblAvailableValue.Size = new System.Drawing.Size(46, 54);
            this.lblAvailableValue.TabIndex = 0;
            this.lblAvailableValue.Text = "0";
            this.lblAvailableValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCardRented
            // 
            this.pnlCardRented.BackColor = System.Drawing.Color.Gray;
            this.pnlCardRented.Controls.Add(this.label2);
            this.pnlCardRented.Controls.Add(this.lblRentedValue);
            this.pnlCardRented.ForeColor = System.Drawing.Color.White;
            this.pnlCardRented.Location = new System.Drawing.Point(398, 24);
            this.pnlCardRented.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCardRented.Name = "pnlCardRented";
            this.pnlCardRented.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCardRented.Size = new System.Drawing.Size(160, 123);
            this.pnlCardRented.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(48, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rented";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRentedValue
            // 
            this.lblRentedValue.AutoSize = true;
            this.lblRentedValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRentedValue.Location = new System.Drawing.Point(56, 31);
            this.lblRentedValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRentedValue.Name = "lblRentedValue";
            this.lblRentedValue.Size = new System.Drawing.Size(46, 54);
            this.lblRentedValue.TabIndex = 0;
            this.lblRentedValue.Text = "0";
            this.lblRentedValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCardRevenue
            // 
            this.pnlCardRevenue.BackColor = System.Drawing.Color.Blue;
            this.pnlCardRevenue.Controls.Add(this.label5);
            this.pnlCardRevenue.Controls.Add(this.lblRevenueValue);
            this.pnlCardRevenue.ForeColor = System.Drawing.Color.White;
            this.pnlCardRevenue.Location = new System.Drawing.Point(584, 24);
            this.pnlCardRevenue.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCardRevenue.Name = "pnlCardRevenue";
            this.pnlCardRevenue.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCardRevenue.Size = new System.Drawing.Size(169, 123);
            this.pnlCardRevenue.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(147, 23);
            this.label5.TabIndex = 1;
            this.label5.Text = "Revenue (Month)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRevenueValue
            // 
            this.lblRevenueValue.AutoSize = true;
            this.lblRevenueValue.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRevenueValue.Location = new System.Drawing.Point(61, 48);
            this.lblRevenueValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRevenueValue.Name = "lblRevenueValue";
            this.lblRevenueValue.Size = new System.Drawing.Size(43, 32);
            this.lblRevenueValue.TabIndex = 0;
            this.lblRevenueValue.Text = "₱0";
            this.lblRevenueValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRevenueValue.Click += new System.EventHandler(this.lblRevenueValue_Click);
            // 
            // pnlCardOverdue
            // 
            this.pnlCardOverdue.BackColor = System.Drawing.Color.Red;
            this.pnlCardOverdue.Controls.Add(this.label7);
            this.pnlCardOverdue.Controls.Add(this.lblOverdueValue);
            this.pnlCardOverdue.ForeColor = System.Drawing.Color.White;
            this.pnlCardOverdue.Location = new System.Drawing.Point(779, 24);
            this.pnlCardOverdue.Margin = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCardOverdue.Name = "pnlCardOverdue";
            this.pnlCardOverdue.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.pnlCardOverdue.Size = new System.Drawing.Size(160, 123);
            this.pnlCardOverdue.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(39, 10);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 23);
            this.label7.TabIndex = 1;
            this.label7.Text = "Overdue";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOverdueValue
            // 
            this.lblOverdueValue.AutoSize = true;
            this.lblOverdueValue.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverdueValue.Location = new System.Drawing.Point(41, 31);
            this.lblOverdueValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOverdueValue.Name = "lblOverdueValue";
            this.lblOverdueValue.Size = new System.Drawing.Size(72, 54);
            this.lblOverdueValue.TabIndex = 0;
            this.lblOverdueValue.Text = "₱0";
            this.lblOverdueValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bottomSplit
            // 
            this.bottomSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomSplit.BackColor = System.Drawing.Color.White;
            this.bottomSplit.ColumnCount = 2;
            this.bottomSplit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bottomSplit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bottomSplit.Controls.Add(this.chartContainer, 0, 0);
            this.bottomSplit.Controls.Add(this.gridContainer, 1, 0);
            this.bottomSplit.Location = new System.Drawing.Point(13, 197);
            this.bottomSplit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bottomSplit.Name = "bottomSplit";
            this.bottomSplit.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.bottomSplit.RowCount = 1;
            this.bottomSplit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bottomSplit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bottomSplit.Size = new System.Drawing.Size(1040, 517);
            this.bottomSplit.TabIndex = 1;
            // 
            // chartContainer
            // 
            this.chartContainer.BackColor = System.Drawing.Color.White;
            this.chartContainer.Controls.Add(this.chartLayout);
            this.chartContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartContainer.Location = new System.Drawing.Point(17, 16);
            this.chartContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartContainer.Name = "chartContainer";
            this.chartContainer.Size = new System.Drawing.Size(499, 485);
            this.chartContainer.TabIndex = 0;
            // 
            // chartLayout
            // 
            this.chartLayout.ColumnCount = 1;
            this.chartLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.chartLayout.Controls.Add(this.chartRevenue, 0, 1);
            this.chartLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartLayout.Location = new System.Drawing.Point(0, 0);
            this.chartLayout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartLayout.Name = "chartLayout";
            this.chartLayout.Padding = new System.Windows.Forms.Padding(27, 0, 27, 0);
            this.chartLayout.RowCount = 3;
            this.chartLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.chartLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.chartLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.chartLayout.Size = new System.Drawing.Size(499, 485);
            this.chartLayout.TabIndex = 0;
            // 
            // chartRevenue
            // 
            chartArea1.Name = "ChartArea1";
            this.chartRevenue.ChartAreas.Add(chartArea1);
            this.chartRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartRevenue.Legends.Add(legend1);
            this.chartRevenue.Location = new System.Drawing.Point(31, 76);
            this.chartRevenue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartRevenue.Name = "chartRevenue";
            series1.ChartArea = "ChartArea1";
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            series1.CustomProperties = "PointWidth=0.6";
            series1.IsValueShownAsLabel = true;
            series1.LabelFormat = "C0";
            series1.Legend = "Legend1";
            series1.LegendText = "MainArea";
            series1.Name = "MainArea";
            series2.ChartArea = "ChartArea1";
            series2.CustomProperties = "PointWidth=0.6";
            series2.IsValueShownAsLabel = true;
            series2.LabelFormat = "C0";
            series2.Legend = "Legend1";
            series2.Name = "Revenue";
            this.chartRevenue.Series.Add(series1);
            this.chartRevenue.Series.Add(series2);
            this.chartRevenue.Size = new System.Drawing.Size(437, 331);
            this.chartRevenue.TabIndex = 0;
            this.chartRevenue.Text = "chart1";
            // 
            // gridContainer
            // 
            this.gridContainer.Controls.Add(this.dgvOverdue);
            this.gridContainer.Controls.Add(this.lblGridTitle);
            this.gridContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridContainer.Location = new System.Drawing.Point(524, 16);
            this.gridContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gridContainer.Name = "gridContainer";
            this.gridContainer.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.gridContainer.Size = new System.Drawing.Size(499, 485);
            this.gridContainer.TabIndex = 1;
            // 
            // dgvOverdue
            // 
            this.dgvOverdue.AllowUserToAddRows = false;
            this.dgvOverdue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOverdue.BackgroundColor = System.Drawing.Color.White;
            this.dgvOverdue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOverdue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOverdue.Location = new System.Drawing.Point(13, 40);
            this.dgvOverdue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvOverdue.Name = "dgvOverdue";
            this.dgvOverdue.ReadOnly = true;
            this.dgvOverdue.RowHeadersVisible = false;
            this.dgvOverdue.RowHeadersWidth = 51;
            this.dgvOverdue.Size = new System.Drawing.Size(473, 433);
            this.dgvOverdue.TabIndex = 1;
            // 
            // lblGridTitle
            // 
            this.lblGridTitle.AutoSize = true;
            this.lblGridTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGridTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGridTitle.ForeColor = System.Drawing.Color.IndianRed;
            this.lblGridTitle.Location = new System.Drawing.Point(13, 12);
            this.lblGridTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGridTitle.Name = "lblGridTitle";
            this.lblGridTitle.Size = new System.Drawing.Size(205, 28);
            this.lblGridTitle.TabIndex = 0;
            this.lblGridTitle.Text = "⚠️ Overdue Returns";
            this.lblGridTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // DashboardView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.bottomSplit);
            this.Controls.Add(this.pnlCards);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(1067, 738);
            this.Name = "DashboardView";
            this.Size = new System.Drawing.Size(1067, 738);
            this.pnlCards.ResumeLayout(false);
            this.pnlCardTotal.ResumeLayout(false);
            this.pnlCardTotal.PerformLayout();
            this.pnlCardAvailable.ResumeLayout(false);
            this.pnlCardAvailable.PerformLayout();
            this.pnlCardRented.ResumeLayout(false);
            this.pnlCardRented.PerformLayout();
            this.pnlCardRevenue.ResumeLayout(false);
            this.pnlCardRevenue.PerformLayout();
            this.pnlCardOverdue.ResumeLayout(false);
            this.pnlCardOverdue.PerformLayout();
            this.bottomSplit.ResumeLayout(false);
            this.chartContainer.ResumeLayout(false);
            this.chartLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).EndInit();
            this.gridContainer.ResumeLayout(false);
            this.gridContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOverdue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlCards;
        private System.Windows.Forms.TableLayoutPanel bottomSplit;
        private System.Windows.Forms.Panel chartContainer;
        private System.Windows.Forms.TableLayoutPanel chartLayout;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue;
        private System.Windows.Forms.Panel gridContainer;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lblGridTitle;
        private System.Windows.Forms.DataGridView dgvOverdue;
        private System.Windows.Forms.Panel pnlCardTotal;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlCardAvailable;
        private System.Windows.Forms.Label labeltitle;
        private System.Windows.Forms.Label lblAvailableValue;
        private System.Windows.Forms.Panel pnlCardRented;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRentedValue;
        private System.Windows.Forms.Panel pnlCardRevenue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblRevenueValue;
        private System.Windows.Forms.Panel pnlCardOverdue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblOverdueValue;
    }
}
