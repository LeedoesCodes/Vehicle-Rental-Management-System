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
    public partial class CustomersView : UserControl
    {
        private int _selectedCustomerId = -1;
        private string _newPhotoTempPath = "";
        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                    ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

        public CustomersView()
        {
            InitializeComponent();

            // 1. Grid Events
            if (dgvCustomers != null)
            {
                dgvCustomers.SelectionChanged -= DgvCustomers_SelectionChanged;
                dgvCustomers.SelectionChanged += DgvCustomers_SelectionChanged;
            }

            // 2. Photo Buttons
            if (btnUploadPhoto != null) btnUploadPhoto.Click += BtnUploadPhoto_Click;
            if (btnCamera != null) btnCamera.Click += BtnCamera_Click;

            // 3. Age Calculation
            if (dtpDOB != null)
                dtpDOB.ValueChanged += (s, e) => CalculateAge(dtpDOB.Value);

            // 4. SEARCH SETUP (Live Search Only - Button Removed)
            if (txtSearch != null)
            {
                // Trigger search automatically when typing
                txtSearch.TextChanged += PerformSearch;

                // Placeholder UX
                txtSearch.GotFocus += (s, e) => { if (txtSearch.Text == "Search Customer...") txtSearch.Text = ""; };
                txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) txtSearch.Text = "Search Customer..."; };
            }

            // 5. Load
            LoadCustomers();
            SetupDropdowns();
        }

        private void SetupDropdowns()
        {
            if (cbCustomerType != null && cbCustomerType.Items.Count == 0)
            {
                cbCustomerType.Items.AddRange(new object[] { "Individual", "Corporate" });
                cbCustomerType.SelectedIndex = 0;
            }
        }

        // ==========================================================
        // 1. SEARCH LOGIC (Renamed for clarity)
        // ==========================================================
        private void PerformSearch(object sender, EventArgs e)
        {
            string term = txtSearch?.Text.Trim();

            // If empty or placeholder, reset list (show all)
            if (string.IsNullOrEmpty(term) || term == "Search Customer...")
            {
                LoadCustomers();
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_SearchCustomers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_SearchTerm", term);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            if (dgvCustomers != null)
                            {
                                dgvCustomers.DataSource = dt;
                                FormatGrid();
                            }
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error searching: " + ex.Message); }
            }
        }

        // ==========================================================
        // 2. DATA LOADING
        // ==========================================================
        public void LoadCustomers()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetAllCustomers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            if (dgvCustomers != null)
                            {
                                dgvCustomers.DataSource = dt;
                                FormatGrid();
                            }
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error loading: " + ex.Message); }
            }
        }

        private void FormatGrid()
        {
            if (dgvCustomers == null) return;
            string[] hide = { "CustomerId", "LicenseNumber", "LicenseExpiry", "DOB", "Address",
                              "CustomerType", "EmergencyContactName", "EmergencyContactPhone",
                              "IsBlacklisted", "CreatedDate", "PhotoPath" };

            foreach (var col in hide)
                if (dgvCustomers.Columns.Contains(col)) dgvCustomers.Columns[col].Visible = false;

            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ==========================================================
        // 3. SELECTION & UI LOGIC
        // ==========================================================
        private void DgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers != null && dgvCustomers.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvCustomers.SelectedRows[0];
                if (row.Cells["CustomerId"].Value != null)
                    _selectedCustomerId = Convert.ToInt32(row.Cells["CustomerId"].Value);

                // Tab 1
                if (txtFirstName != null) txtFirstName.Text = row.Cells["FirstName"].Value?.ToString();
                if (txtLastName != null) txtLastName.Text = row.Cells["LastName"].Value?.ToString();
                if (txtEmail != null) txtEmail.Text = row.Cells["Email"].Value?.ToString();
                if (txtPhone != null) txtPhone.Text = row.Cells["Phone"].Value?.ToString();
                if (txtAddress != null) txtAddress.Text = row.Cells["Address"].Value?.ToString();
                if (txtEmergencyName != null) txtEmergencyName.Text = row.Cells["EmergencyContactName"].Value?.ToString();
                if (txtEmergencyPhone != null) txtEmergencyPhone.Text = row.Cells["EmergencyContactPhone"].Value?.ToString();

                if (dtpDOB != null && row.Cells["DOB"].Value != DBNull.Value)
                    dtpDOB.Value = Convert.ToDateTime(row.Cells["DOB"].Value);

                string photoPath = "";
                if (dgvCustomers.Columns.Contains("PhotoPath") && row.Cells["PhotoPath"].Value != DBNull.Value)
                    photoPath = row.Cells["PhotoPath"].Value.ToString();
                LoadCustomerImage(photoPath);

                // Tab 2
                if (txtLicenseNum != null) txtLicenseNum.Text = row.Cells["LicenseNumber"].Value?.ToString();
                if (dtpExpiryDate != null && row.Cells["LicenseExpiry"].Value != DBNull.Value)
                    dtpExpiryDate.Value = Convert.ToDateTime(row.Cells["LicenseExpiry"].Value);

                // Tab 3
                if (cbCustomerType != null) cbCustomerType.Text = row.Cells["CustomerType"].Value?.ToString();
                if (chkBlacklist != null) chkBlacklist.Checked = Convert.ToBoolean(row.Cells["IsBlacklisted"].Value);

                LoadCustomerStats(_selectedCustomerId);

                if (btnSave != null) btnSave.Text = "Update Customer";
                _newPhotoTempPath = "";
            }
        }

        private void CalculateAge(DateTime dob)
        {
            int age = DateTime.Today.Year - dob.Year;
            if (dob.Date > DateTime.Today.AddYears(-age)) age--;

            if (lblAgeCheck != null)
            {
                lblAgeCheck.Text = $"Age: {age}";
                if (age < 21) { lblAgeCheck.ForeColor = Color.Red; lblAgeCheck.Text += " (Restricted)"; }
                else { lblAgeCheck.ForeColor = Color.Green; lblAgeCheck.Text += " (Verified)"; }
            }
        }

        private void LoadCustomerImage(string path)
        {
            if (picCustomerPhoto == null) return;
            if (picCustomerPhoto.Image != null) { picCustomerPhoto.Image.Dispose(); picCustomerPhoto.Image = null; }

            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        picCustomerPhoto.Image = Image.FromStream(fs);
                        picCustomerPhoto.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                catch { picCustomerPhoto.Image = null; }
            }
            else picCustomerPhoto.Image = null;
        }

        private void LoadCustomerStats(int customerId)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetCustomerStats", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_CustomerId", customerId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (lblTotalRentals != null) lblTotalRentals.Text = "Total Trips: " + reader["TotalRentals"];
                                if (lblTotalSpent != null) lblTotalSpent.Text = "Total Spent: " + Convert.ToDecimal(reader["TotalSpent"]).ToString("C2");
                            }
                        }
                    }
                }
                catch { }
            }
        }

        // ==========================================================
        // 4. PHOTO EVENTS
        // ==========================================================
        private void BtnUploadPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadCustomerImage(ofd.FileName);
                _newPhotoTempPath = ofd.FileName;
            }
        }

        private void BtnCamera_Click(object sender, EventArgs e)
        {
            CameraForm cam = new CameraForm();
            if (cam.ShowDialog() == DialogResult.OK)
            {
                string tempFolder = Path.Combine(Application.StartupPath, "TempCaptures");
                if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);

                string tempFile = Path.Combine(tempFolder, $"Capture_{DateTime.Now.Ticks}.jpg");
                cam.CapturedImage.Save(tempFile, System.Drawing.Imaging.ImageFormat.Jpeg);

                LoadCustomerImage(tempFile);
                _newPhotoTempPath = tempFile;
            }
        }

        // ==========================================================
        // 5. MAIN BUTTONS
        // ==========================================================
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtFirstName == null || string.IsNullOrWhiteSpace(txtFirstName.Text)) { MessageBox.Show("First Name required."); return; }

            string finalPhotoPath = "";
            if (_selectedCustomerId != -1 && string.IsNullOrEmpty(_newPhotoTempPath))
            {
                if (dgvCustomers.SelectedRows.Count > 0 && dgvCustomers.Columns.Contains("PhotoPath"))
                    finalPhotoPath = dgvCustomers.SelectedRows[0].Cells["PhotoPath"].Value?.ToString();
            }
            if (!string.IsNullOrEmpty(_newPhotoTempPath))
            {
                try
                {
                    string folder = Path.Combine(Application.StartupPath, "CustomerImages");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    string destPath = Path.Combine(folder, $"Cust_{Guid.NewGuid()}.jpg");
                    File.Copy(_newPhotoTempPath, destPath, true);
                    finalPhotoPath = destPath;
                }
                catch (Exception ex) { MessageBox.Show("Photo error: " + ex.Message); }
            }

            // DB Save
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (_selectedCustomerId == -1) cmd = new MySqlCommand("sp_AddCustomer", conn);
                    else
                    {
                        cmd = new MySqlCommand("sp_UpdateCustomer", conn);
                        cmd.Parameters.AddWithValue("@p_CustomerId", _selectedCustomerId);
                        cmd.Parameters.AddWithValue("@p_IsBlacklisted", chkBlacklist?.Checked ?? false);
                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_FirstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@p_LastName", txtLastName?.Text ?? "");
                    cmd.Parameters.AddWithValue("@p_Email", txtEmail?.Text ?? "");
                    cmd.Parameters.AddWithValue("@p_Phone", txtPhone?.Text ?? "");
                    cmd.Parameters.AddWithValue("@p_Address", txtAddress?.Text ?? "");
                    cmd.Parameters.AddWithValue("@p_LicenseNumber", txtLicenseNum?.Text ?? "");
                    cmd.Parameters.AddWithValue("@p_LicenseExpiry", dtpExpiryDate?.Value ?? DateTime.Now);
                    cmd.Parameters.AddWithValue("@p_DOB", dtpDOB?.Value ?? DateTime.Now);
                    cmd.Parameters.AddWithValue("@p_CustomerType", cbCustomerType?.Text ?? "Individual");
                    cmd.Parameters.AddWithValue("@p_EmergencyName", txtEmergencyName?.Text ?? "");
                    cmd.Parameters.AddWithValue("@p_EmergencyPhone", txtEmergencyPhone?.Text ?? "");
                    cmd.Parameters.AddWithValue("@p_PhotoPath", finalPhotoPath);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Saved successfully!");
                    LoadCustomers();
                    ClearForm();
                }
                catch (Exception ex) { MessageBox.Show("Save error: " + ex.Message); }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId == -1) { MessageBox.Show("Select a customer first."); return; }
            if (MessageBox.Show("Delete this customer?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand("sp_DeleteCustomer", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_CustomerId", _selectedCustomerId);
                            cmd.ExecuteNonQuery();
                        }
                        LoadCustomers();
                        ClearForm();
                    }
                    catch (Exception ex) { MessageBox.Show("Delete error: " + ex.Message); }
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e) => ClearForm();

        private void ClearForm()
        {
            _selectedCustomerId = -1;
            _newPhotoTempPath = "";
            if (txtFirstName != null) txtFirstName.Clear();
            if (txtLastName != null) txtLastName.Clear();
            if (txtEmail != null) txtEmail.Clear();
            if (txtPhone != null) txtPhone.Clear();
            if (txtAddress != null) txtAddress.Clear();
            if (txtLicenseNum != null) txtLicenseNum.Clear();
            if (txtEmergencyName != null) txtEmergencyName.Clear();
            if (txtEmergencyPhone != null) txtEmergencyPhone.Clear();
            if (chkBlacklist != null) chkBlacklist.Checked = false;
            if (cbCustomerType != null) cbCustomerType.SelectedIndex = 0;
            if (dtpDOB != null) dtpDOB.Value = DateTime.Now;
            if (dtpExpiryDate != null) dtpExpiryDate.Value = DateTime.Now;
            if (picCustomerPhoto != null) picCustomerPhoto.Image = null;
            if (dgvCustomers != null) dgvCustomers.ClearSelection();
            if (btnSave != null) btnSave.Text = "Add New Customer";
            if (lblAgeCheck != null) lblAgeCheck.Text = "Age: --";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}