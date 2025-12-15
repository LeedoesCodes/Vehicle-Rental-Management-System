using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            
            // Configure chart axes
            ConfigureChartAxes();
            
            // Initialize card hover effects
            InitializeCardEvents();
            
            // Load data from database
            LoadData();
        }

        private void ConfigureChartAxes()
        {
            try
            {
                // Configure X-Axis
                if (chartRevenue.ChartAreas.Count > 0)
                {
                    ChartArea area = chartRevenue.ChartAreas[0];
                    
                    // X-Axis Configuration
                    area.AxisX.Title = "Month";
                    area.AxisX.TitleFont = new Font("Segoe UI", 10, FontStyle.Bold);
                    area.AxisX.Interval = 1;
                    area.AxisX.MajorGrid.LineColor = Color.LightGray;
                    area.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                    area.AxisX.LabelStyle.Font = new Font("Segoe UI", 9);
                    
                    // Y-Axis Configuration
                    area.AxisY.Title = "Amount (PHP)";
                    area.AxisY.TitleFont = new Font("Segoe UI", 10, FontStyle.Bold);
                    area.AxisY.LabelStyle.Format = "C0";
                    area.AxisY.MajorGrid.LineColor = Color.LightGray;
                    area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
                    area.AxisY.LabelStyle.Font = new Font("Segoe UI", 9);
                }
                
                // Ensure series exists and is configured
                if (chartRevenue.Series.Count > 0 && chartRevenue.Series["Revenue"] != null)
                {
                    chartRevenue.Series["Revenue"].Color = Color.FromArgb(0, 120, 215);
                    chartRevenue.Series["Revenue"].IsValueShownAsLabel = true;
                    chartRevenue.Series["Revenue"].LabelFormat = "C0";
                    chartRevenue.Series["Revenue"]["PointWidth"] = "0.6";
                    chartRevenue.Series["Revenue"].Font = new Font("Segoe UI", 9);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error configuring chart: " + ex.Message);
            }
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

                    // --- 1. LOAD STATS AND UPDATE MANUAL CARDS ---
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetDashboardStats", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Update each manual card with database values
                                lblTotalValue.Text = reader["TotalVehicles"].ToString();
                                lblAvailableValue.Text = reader["AvailableVehicles"].ToString();
                                lblRentedValue.Text = reader["RentedVehicles"].ToString();
                                lblRevenueValue.Text = $"₱{Convert.ToDecimal(reader["RevenueMonth"]):N0}";
                                lblOverdueValue.Text = reader["OverdueCount"].ToString();
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
                        dgvOverdue.DataSource = dt;
                        StyleDataGridView(); // Style after data loads
                    }

                    // --- 3. LOAD CHART DATA ---
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetMonthlyRevenue", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        // Clear existing chart data
                        if (chartRevenue.Series["Revenue"] != null)
                        {
                            chartRevenue.Series["Revenue"].Points.Clear();
                        }
                        
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            bool hasData = false;
                            while (reader.Read())
                            {
                                hasData = true;
                                string month = reader["MonthName"].ToString();
                                decimal amount = Convert.ToDecimal(reader["TotalRevenue"]);
                                chartRevenue.Series["Revenue"].Points.AddXY(month, amount);
                            }

                            if (!hasData)
                            {
                                if (chartRevenue.Titles.Count > 0)
                                {
                                    chartRevenue.Titles[0].Text += " (No Data Yet)";
                                }
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

        // Remove the AddCard method since we're using manual cards
        // private void AddCard(FlowLayoutPanel panel, string title, string value, Color color) { }

        // For rounded corners (keep if you want rounded cards)
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        // Add hover effect to manual cards
        private void InitializeCardEvents()
        {
            try
            {
                // List of all card panels (use the names from your designer)
                Panel[] cards = { 
                    pnlCardTotal, 
                    pnlCardAvailable, 
                    pnlCardRented, 
                    pnlCardRevenue, 
                    pnlCardOverdue 
                };
                
                foreach (Panel card in cards)
                {
                    if (card != null)
                    {
                        Color originalColor = card.BackColor;
                        
                        card.MouseEnter += (sender, e) =>
                        {
                            card.BackColor = ControlPaint.Light(originalColor, 0.1f);
                            card.Cursor = Cursors.Hand;
                        };
                        
                        card.MouseLeave += (sender, e) =>
                        {
                            card.BackColor = originalColor;
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent fail - cards might not be initialized yet
                Console.WriteLine("Error initializing card events: " + ex.Message);
            }
        }

        // Style the DataGridView
        private void StyleDataGridView()
        {
            if (dgvOverdue != null && dgvOverdue.Columns.Count > 0)
            {
                // Style headers
                dgvOverdue.EnableHeadersVisualStyles = false;
                dgvOverdue.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
                dgvOverdue.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvOverdue.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                dgvOverdue.ColumnHeadersHeight = 40;

                // Style rows
                dgvOverdue.RowTemplate.Height = 35;
                dgvOverdue.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

                // Style selection
                dgvOverdue.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 229, 255);
                dgvOverdue.DefaultCellStyle.SelectionForeColor = Color.Black;

                // Center text in cells
                foreach (DataGridViewColumn column in dgvOverdue.Columns)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        // Refresh button click (add a refresh button to your form if needed)
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // Optional: Add this if you want rounded corners on cards
        private void ApplyRoundedCornersToCards()
        {
            try
            {
                Panel[] cards = { pnlCardTotal, pnlCardAvailable, pnlCardRented, pnlCardRevenue, pnlCardOverdue };
                
                foreach (Panel card in cards)
                {
                    if (card != null)
                    {
                        card.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, card.Width, card.Height, 15, 15));
                    }
                }
            }
            catch
            {
                // If it fails, cards will have square corners (no problem)
            }
        }

        private void DashboardView_Load(object sender, EventArgs e)
        {
            // Apply styling when control loads
            StyleDataGridView();
            ApplyRoundedCornersToCards(); // Optional
        }
    }
}