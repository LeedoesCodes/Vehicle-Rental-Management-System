using System;
using System.Data;
using System.Drawing;
using System.IO; // Required for file operations
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class ReservationsView : UserControl
    {
        // Connection String
        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                    ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

        public ReservationsView()
        {
            InitializeComponent(); // Loads your Designer (Copy-Pasted) Layout

            // Link Events (Safety Check)
           

            LoadReservations();
        }

        // ==========================================================
        // 1. DATA LOADING
        // ==========================================================
        public void LoadReservations()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetAllReservations", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            if (dgvReservations != null)
                            {
                                dgvReservations.DataSource = dt;
                                FormatGrid();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading reservations: " + ex.Message);
                }
            }
        }

        private void FormatGrid()
        {
            if (dgvReservations == null) return;

            // Format Money
            if (dgvReservations.Columns.Contains("TotalAmount"))
                dgvReservations.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";

            // Format Dates
            if (dgvReservations.Columns.Contains("StartDate"))
                dgvReservations.Columns["StartDate"].DefaultCellStyle.Format = "d";

            if (dgvReservations.Columns.Contains("EndDate"))
                dgvReservations.Columns["EndDate"].DefaultCellStyle.Format = "d";

            // Hide IDs
            string[] colsToHide = { "ReservationId", "VehicleId", "CustomerId", "ImagePath" };
            foreach (string col in colsToHide)
            {
                if (dgvReservations.Columns.Contains(col))
                    dgvReservations.Columns[col].Visible = false;
            }

            dgvReservations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ==========================================================
        // 2. SELECTION LOGIC (Update Details Panel)
        // ==========================================================
        private void DgvReservations_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count > 0)
            {
                var row = dgvReservations.SelectedRows[0];

                // Update Labels (Ensure these match your Designer names)
                // You copied from Rentals, so names like 'lblDetailVehicle' should exist.

                if (lblDetailVehicle != null)
                    lblDetailVehicle.Text = row.Cells["VehicleName"].Value?.ToString() ?? "Unknown";

                if (lblDetailCustomer != null)
                    lblDetailCustomer.Text = "Customer: " + (row.Cells["CustomerName"].Value?.ToString() ?? "Unknown");

                if (lblDetailAmount != null)
                    lblDetailAmount.Text = "Total: " + (Convert.ToDecimal(row.Cells["TotalAmount"].Value).ToString("C2"));

                // Date Logic
                if (lblDetailDates != null && row.Cells["StartDate"].Value != DBNull.Value)
                {
                    string start = Convert.ToDateTime(row.Cells["StartDate"].Value).ToShortDateString();
                    string end = Convert.ToDateTime(row.Cells["EndDate"].Value).ToShortDateString();
                    lblDetailDates.Text = $"{start} to {end}";
                }

                // Image Logic
                string imagePath = "";
                if (dgvReservations.Columns.Contains("ImagePath") && row.Cells["ImagePath"].Value != DBNull.Value)
                {
                    imagePath = row.Cells["ImagePath"].Value.ToString();
                }
                ShowVehiclePreview(imagePath);
            }
        }

        private void ShowVehiclePreview(string path)
        {
            if (pbVehicle == null) return;

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
                    pbVehicle.SizeMode = PictureBoxSizeMode.Zoom;
                }
                catch { ShowDefaultImage(); }
            }
            else
            {
                ShowDefaultImage();
            }
        }

        private void ShowDefaultImage()
        {
            if (pbVehicle == null) return;

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

        // ==========================================================
        // 3. BUTTON EVENTS
        // ==========================================================

        // Make sure to double-click your "Add Reservation" button in designer to link this!
        private void BtnNewReservation_Click(object sender, EventArgs e)
        {
            Forms.AddReservationForm form = new Forms.AddReservationForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadReservations();
            }
        }

        // Make sure to double-click your "Cancel" button in designer to link this!
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0) return;

            // Check Status
            string status = "";
            if (dgvReservations.Columns.Contains("Status"))
                status = dgvReservations.SelectedRows[0].Cells["Status"].Value.ToString();

            if (status == "Cancelled")
            {
                MessageBox.Show("This reservation is already cancelled.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to cancel this reservation?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int resId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["ReservationId"].Value);
                CancelReservation(resId);
            }
        }

        private void CancelReservation(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_CancelReservation", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_ReservationId", id);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Reservation cancelled.");
                    LoadReservations();
                }
                catch (Exception ex) { MessageBox.Show("Error cancelling: " + ex.Message); }
            }
        }
    }
}