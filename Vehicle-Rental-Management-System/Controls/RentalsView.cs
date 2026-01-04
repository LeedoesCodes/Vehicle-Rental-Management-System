using System;
using System.Data;
using System.Drawing;
using System.IO; // Required for file operations
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class RentalsView : UserControl
    {
        public RentalsView()
        {
            InitializeComponent();

            // Link the selection event
            if (dgvRentals != null)
                dgvRentals.SelectionChanged += DgvRentals_SelectionChanged;

            LoadRentals();
        }

        // ==========================================================
        // 1. DATA LOADING
        // ==========================================================
        public void LoadRentals()
        {
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetAllRentals", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvRentals.DataSource = dt;
                            FormatGrid();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading rentals: " + ex.Message);
                }
            }
        }

        private void FormatGrid()
        {
            
            if (dgvRentals.Columns.Contains("TotalAmount"))
                dgvRentals.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";

            
            if (dgvRentals.Columns.Contains("RentalDate"))
            {
                dgvRentals.Columns["RentalDate"].DefaultCellStyle.Format = "d"; 
                dgvRentals.Columns["RentalDate"].HeaderText = "Pickup Date";
            }


            if (dgvRentals.Columns.Contains("ScheduledReturn"))
            {
                dgvRentals.Columns["ScheduledReturn"].DefaultCellStyle.Format = "d";
                dgvRentals.Columns["ScheduledReturn"].HeaderText = "Scheduled Return"; // Expected Date
            }

            if (dgvRentals.Columns.Contains("ActualReturn"))
            {
                dgvRentals.Columns["ActualReturn"].DefaultCellStyle.Format = "d";
                dgvRentals.Columns["ActualReturn"].HeaderText = "Actual Return"; // Real Date
            }

            // 3. Hide Technical Columns
            string[] colsToHide = { "RentalId", "VehicleId", "CustomerId", "ImagePath" };
            foreach (string col in colsToHide)
            {
                if (dgvRentals.Columns.Contains(col))
                    dgvRentals.Columns[col].Visible = false;
            }
        }



        private void DgvRentals_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRentals.SelectedRows.Count > 0)
            {
                var row = dgvRentals.SelectedRows[0];

                
                if (lblDetailVehicle != null)
                    lblDetailVehicle.Text = row.Cells["VehicleName"].Value?.ToString() ?? "Unknown";

                if (lblDetailCustomer != null)
                    lblDetailCustomer.Text = "Renter: " + (row.Cells["CustomerName"].Value?.ToString() ?? "Unknown");

                if (lblDetailAmount != null)
                    lblDetailAmount.Text = "Total: " + (Convert.ToDecimal(row.Cells["TotalAmount"].Value).ToString("C2"));

                
                if (lblDetailDates != null && row.Cells["RentalDate"].Value != DBNull.Value)
                {
                    string start = Convert.ToDateTime(row.Cells["RentalDate"].Value).ToShortDateString();
                    string end = "-";
                    if (row.Cells["ScheduledReturn"].Value != DBNull.Value)
                        end = Convert.ToDateTime(row.Cells["ScheduledReturn"].Value).ToShortDateString();

                    lblDetailDates.Text = $"{start} to {end}";
                }

                
                string imagePath = "";
               
                if (dgvRentals.Columns.Contains("ImagePath") && row.Cells["ImagePath"].Value != DBNull.Value)
                {
                    imagePath = row.Cells["ImagePath"].Value.ToString();
                }

                ShowVehiclePreview(imagePath);
            }
        }

        private void ShowVehiclePreview(string path)
        {
           
            if (pbVehicle.Image != null)
            {
                pbVehicle.Image.Dispose();
                pbVehicle.Image = null;
            }

            
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                try
                {
                    
                    pbVehicle.Image = Image.FromFile(path);
                }
                catch
                {
                    ShowDefaultImage();
                }
            }
            else
            {
                ShowDefaultImage();
            }
        }

        private void ShowDefaultImage()
        {
            // Draw a simple placeholder if no image found
            Bitmap bmp = new Bitmap(Math.Max(1, pbVehicle.Width), Math.Max(1, pbVehicle.Height));
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.WhiteSmoke);
                g.DrawString("No Image", new Font("Arial", 10, FontStyle.Bold),
                             Brushes.Gray, new RectangleF(0, 0, bmp.Width, bmp.Height),
                             new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            pbVehicle.Image = bmp;
        }



        private void BtnNewRental_Click(object sender, EventArgs e)
        {
            Forms.NewRentalForm form = new Forms.NewRentalForm();
            if (form.ShowDialog() == DialogResult.OK) LoadRentals();
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            if (dgvRentals.SelectedRows.Count == 0) return;

            string status = dgvRentals.SelectedRows[0].Cells["Status"].Value.ToString();
            if (status == "Returned")
            {
                MessageBox.Show("Already returned.");
                return;
            }

            int rentalId = Convert.ToInt32(dgvRentals.SelectedRows[0].Cells["RentalId"].Value);
            int vehicleId = Convert.ToInt32(dgvRentals.SelectedRows[0].Cells["VehicleId"].Value);
            string vName = lblDetailVehicle.Text;
            string cName = lblDetailCustomer.Text.Replace("Renter: ", "");

            Forms.ReturnVehicleForm form = new Forms.ReturnVehicleForm(rentalId, vehicleId, vName, cName);
            if (form.ShowDialog() == DialogResult.OK) LoadRentals();
        }
    }
}