using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Works after adding reference
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.WhiteSmoke;
            this.AutoScroll = true;

            // 1. TOP PANEL (Cards)
            FlowLayoutPanel cardsPanel = new FlowLayoutPanel();
            cardsPanel.Dock = DockStyle.Top;
            cardsPanel.Height = 160;
            cardsPanel.Padding = new Padding(10);
            cardsPanel.FlowDirection = FlowDirection.LeftToRight;
            cardsPanel.AutoSize = false;
            cardsPanel.Name = "pnlCards";
            this.Controls.Add(cardsPanel);

            // 2. BOTTOM PANEL (Split: Chart Left, Grid Right)
            TableLayoutPanel bottomSplit = new TableLayoutPanel();
            bottomSplit.Dock = DockStyle.Fill;
            bottomSplit.ColumnCount = 2;
            bottomSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            bottomSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            bottomSplit.Padding = new Padding(10);
            this.Controls.Add(bottomSplit);

            // --- CHART SECTION (FIXED) ---
            Panel chartContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10) };

            Chart revenueChart = new Chart();
            revenueChart.Dock = DockStyle.Fill;
            revenueChart.Name = "chartRevenue";

            // A. Add Chart Area
            ChartArea area = new ChartArea("MainArea");
            revenueChart.ChartAreas.Add(area);

            // B. FIX: Turn on the Legend
            Legend legend = new Legend("MainLegend");
            legend.Docking = Docking.Top; // Put legend at the top
            revenueChart.Legends.Add(legend);

            // C. Add Series and configure Labels
            Series series = new Series("Revenue");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.FromArgb(0, 120, 215);
            series.IsValueShownAsLabel = true; // Show the numbers on top of the bars
            series.LabelFormat = "C0";         // Format labels as Currency (₱)
            revenueChart.Series.Add(series);

            // D. FIX: Add Axis Titles
            area.AxisX.Title = "Month";
            area.AxisX.Interval = 1; // Ensure every month shows up
            area.AxisY.Title = "Amount (PHP)";
            area.AxisY.LabelStyle.Format = "C0"; // Format Y-axis numbers as currency

            // Add Title
            revenueChart.Titles.Add("Monthly Revenue Trend (This Year)");

            chartContainer.Controls.Add(revenueChart);
            bottomSplit.Controls.Add(chartContainer, 0, 0);

            // --- OVERDUE GRID SECTION ---
            Panel gridContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10) };
            Label lblGridTitle = new Label { Text = "⚠️ Overdue Returns", Dock = DockStyle.Top, Font = new Font("Segoe UI", 12, FontStyle.Bold), Height = 30, ForeColor = Color.IndianRed };

            DataGridView dgvOverdue = new DataGridView();
            dgvOverdue.Name = "dgvOverdue";
            dgvOverdue.Dock = DockStyle.Fill;
            dgvOverdue.BackgroundColor = Color.White;
            dgvOverdue.BorderStyle = BorderStyle.None;
            dgvOverdue.RowHeadersVisible = false;
            dgvOverdue.AllowUserToAddRows = false;
            dgvOverdue.ReadOnly = true;
            dgvOverdue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridContainer.Controls.Add(dgvOverdue);
            gridContainer.Controls.Add(lblGridTitle);
            bottomSplit.Controls.Add(gridContainer, 1, 0);

            // Ensure cards panel is on top
            cardsPanel.BringToFront();
        }

        private void LoadData()
        {
            string connString = "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
            if (ConfigurationManager.ConnectionStrings["MySqlConnection"] != null)
                connString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // --- 1. LOAD STATS (Counters) ---
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetDashboardStats", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var pnlCards = this.Controls["pnlCards"] as FlowLayoutPanel;
                                pnlCards.Controls.Clear();

                                AddCard(pnlCards, "Total Fleet", reader["TotalVehicles"].ToString(), Color.FromArgb(64, 64, 64));
                                AddCard(pnlCards, "Available", reader["AvailableVehicles"].ToString(), Color.FromArgb(40, 167, 69));
                                AddCard(pnlCards, "Rented Out", reader["RentedVehicles"].ToString(), Color.FromArgb(0, 120, 215));
                                AddCard(pnlCards, "Revenue (Month)", $"₱{reader["RevenueMonth"]:N0}", Color.FromArgb(108, 117, 125));
                                AddCard(pnlCards, "Overdue", reader["OverdueCount"].ToString(), Color.FromArgb(220, 53, 69));
                            }
                        }
                    }

                    // --- 2. LOAD OVERDUE LIST ---
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetDashboardOverdue", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Find the grid inside the nested panels
                        var bottomSplit = this.Controls[1] as TableLayoutPanel;
                        var gridContainer = bottomSplit.Controls[1] as Panel;
                        var dgv = gridContainer.Controls["dgvOverdue"] as DataGridView;
                        dgv.DataSource = dt;
                    }

                    // --- 3. REAL DATA FOR CHART (Replaced Dummy Data) ---
                    var chartPanel = (this.Controls[1] as TableLayoutPanel).Controls[0] as Panel;
                    var chart = chartPanel.Controls["chartRevenue"] as Chart;

                    // Clear old points
                    chart.Series["Revenue"].Points.Clear();

                    using (MySqlCommand cmd = new MySqlCommand("sp_GetMonthlyRevenue", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            bool hasData = false;
                            while (reader.Read())
                            {
                                hasData = true;
                                string month = reader["MonthName"].ToString();
                                decimal amount = Convert.ToDecimal(reader["TotalRevenue"]); // Matches the fix I gave you

                                // Add real point
                                chart.Series["Revenue"].Points.AddXY(month, amount);
                            }

                            if (!hasData)
                            {
                                // Optional: Add a "No Data" placeholder or just leave empty
                                chart.Titles[0].Text += " (No Data Yet)";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading dashboard: " + ex.Message);
                }
            }
        }

        private void AddCard(FlowLayoutPanel panel, string title, string value, Color color)
        {
            Panel card = new Panel();
            card.Size = new Size(200, 120);
            card.BackColor = color;
            card.Margin = new Padding(10);

            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblValue.ForeColor = Color.White;
            lblValue.AutoSize = false;
            lblValue.Dock = DockStyle.Fill;
            lblValue.TextAlign = ContentAlignment.MiddleCenter;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblTitle.ForeColor = Color.WhiteSmoke;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 30;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            card.Controls.Add(lblValue);
            card.Controls.Add(lblTitle);
            panel.Controls.Add(card);
        }
    }
}