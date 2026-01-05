using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using Vehicle_Rental_Management_System.Forms;

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class RentalsView : UserControl
    {
        private DataTable _rentalsTable;
        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                    ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

        public RentalsView()
        {
            InitializeComponent();
            SetupUI();
            LoadRentals();
        }

        private void SetupUI()
        {
            if (dgvRentals != null)
            {
                dgvRentals.SelectionChanged += DgvRentals_SelectionChanged;
                dgvRentals.CellFormatting += DgvRentals_CellFormatting;
            }

            if (txtSearch != null)
            {
                txtSearch.TextChanged += (s, e) => ApplyFilters();
                txtSearch.GotFocus += (s, e) => { if (txtSearch.Text == "Search...") txtSearch.Text = ""; };
                txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) txtSearch.Text = "Search..."; };
                txtSearch.Text = "Search...";
            }

            if (cbStatusFilter != null)
            {
                // UPDATED: Matches your DB status (Ongoing, Completed)
                cbStatusFilter.Items.Clear();
                cbStatusFilter.Items.AddRange(new object[] { "All", "Ongoing", "Completed", "Overdue" });
                cbStatusFilter.SelectedIndex = 0;
                cbStatusFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
            }
        }

        public void LoadRentals()
        {
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
                            _rentalsTable = new DataTable();
                            adapter.Fill(_rentalsTable);

                            // Add logic for Overdue calculation
                            if (!_rentalsTable.Columns.Contains("IsOverdue"))
                            {
                                _rentalsTable.Columns.Add("IsOverdue", typeof(bool));
                            }

                            foreach (DataRow row in _rentalsTable.Rows)
                            {
                                if (row["ScheduledReturn"] != DBNull.Value)
                                {
                                    DateTime scheduled = Convert.ToDateTime(row["ScheduledReturn"]);
                                    string status = row["Status"].ToString();

                                    // UPDATED LOGIC: If "Ongoing" and past due date -> It's Overdue
                                    row["IsOverdue"] = (status == "Ongoing" && scheduled.Date < DateTime.Now.Date);
                                }
                                else
                                {
                                    row["IsOverdue"] = false;
                                }
                            }

                            dgvRentals.DataSource = _rentalsTable;
                            FormatGrid();
                            ApplyFilters();
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error loading rentals: " + ex.Message); }
            }
        }

        private void ApplyFilters()
        {
            if (_rentalsTable == null) return;

            string filter = "";
            string search = txtSearch != null && txtSearch.Text != "Search..." ? txtSearch.Text.Trim() : "";
            string status = cbStatusFilter != null ? cbStatusFilter.SelectedItem.ToString() : "All";

            // 1. Search Filter
            if (!string.IsNullOrEmpty(search))
            {
                // Ensure your SP returns CustomerName and VehicleName, or change these to match your DataTable columns
                filter = $"(CustomerName LIKE '%{search}%' OR VehicleName LIKE '%{search}%')";
            }

            // 2. Status Filter
            if (status != "All")
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";

                if (status == "Overdue")
                    filter += "IsOverdue = True";
                else
                    // Filter matches "Ongoing" or "Completed" exactly
                    filter += $"Status = '{status}'";
            }

            _rentalsTable.DefaultView.RowFilter = filter;
            if (dgvRentals.Rows.Count == 0) ClearDetails();
        }

        private void FormatGrid()
        {
            if (dgvRentals == null) return;

            if (dgvRentals.Columns.Contains("TotalAmount"))
                dgvRentals.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";

            if (dgvRentals.Columns.Contains("RentalDate"))
                dgvRentals.Columns["RentalDate"].HeaderText = "Pickup Date";

            string[] colsToHide = { "RentalId", "VehicleId", "CustomerId", "ImagePath", "IsOverdue" };
            foreach (string col in colsToHide)
                if (dgvRentals.Columns.Contains(col)) dgvRentals.Columns[col].Visible = false;

            dgvRentals.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void DgvRentals_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvRentals.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();

                // Calculate overdue based on the hidden column
                bool isOverdue = false;
                if (dgvRentals.Rows[e.RowIndex].Cells["IsOverdue"].Value != DBNull.Value)
                    isOverdue = (bool)dgvRentals.Rows[e.RowIndex].Cells["IsOverdue"].Value;

                if (isOverdue)
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dgvRentals.Font, FontStyle.Bold);
                }
                else if (status == "Ongoing") // UPDATED to match your DB
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (status == "Completed") // UPDATED to match your DB
                {
                    e.CellStyle.ForeColor = Color.Gray;
                }
            }
        }

        private void DgvRentals_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRentals.SelectedRows.Count > 0)
            {
                var row = dgvRentals.SelectedRows[0];

                if (lblDetailVehicle != null) lblDetailVehicle.Text = row.Cells["VehicleName"].Value?.ToString();
                if (lblDetailCustomer != null) lblDetailCustomer.Text = "Renter: " + row.Cells["CustomerName"].Value?.ToString();
                if (lblDetailAmount != null) lblDetailAmount.Text = "Total: " + Convert.ToDecimal(row.Cells["TotalAmount"].Value).ToString("C2");

                if (lblDetailDates != null)
                {
                    string start = Convert.ToDateTime(row.Cells["RentalDate"].Value).ToShortDateString();
                    string end = Convert.ToDateTime(row.Cells["ScheduledReturn"].Value).ToShortDateString();
                    lblDetailDates.Text = $"{start} - {end}";
                }

                string imagePath = "";
                if (dgvRentals.Columns.Contains("ImagePath") && row.Cells["ImagePath"].Value != DBNull.Value)
                    imagePath = row.Cells["ImagePath"].Value.ToString();

                ShowVehiclePreview(imagePath);
            }
            else
            {
                ClearDetails();
            }
        }

        private void ClearDetails()
        {
            if (lblDetailVehicle != null) lblDetailVehicle.Text = "Select a Rental";
            if (lblDetailCustomer != null) lblDetailCustomer.Text = "";
            if (lblDetailAmount != null) lblDetailAmount.Text = "";
            if (lblDetailDates != null) lblDetailDates.Text = "";
            ShowVehiclePreview(null);
        }

        private void ShowVehiclePreview(string fileName)
        {
            if (pbVehicle == null) return;
            if (pbVehicle.Image != null) { pbVehicle.Image.Dispose(); pbVehicle.Image = null; }

            if (!string.IsNullOrEmpty(fileName))
            {
                try
                {
                    string projectPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string fullPath = Path.Combine(projectPath, "Assets", "Images", "Vehicles", fileName);

                    if (File.Exists(fullPath))
                    {
                        using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                        {
                            pbVehicle.Image = Image.FromStream(fs);
                            pbVehicle.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                        return;
                    }
                }
                catch { }
            }
            ShowDefaultImage();
        }

        private void ShowDefaultImage()
        {
            Bitmap bmp = new Bitmap(Math.Max(1, pbVehicle.Width), Math.Max(1, pbVehicle.Height));
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.WhiteSmoke);
                g.DrawString("No Image", new Font("Arial", 10), Brushes.Gray, new RectangleF(0, 0, bmp.Width, bmp.Height), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
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

            // UPDATED: Check for "Completed" instead of "Returned"
            if (status == "Completed")
            {
                MessageBox.Show("This vehicle has already been returned.");
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