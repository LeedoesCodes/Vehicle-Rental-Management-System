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

            // 4. Search
            if (txtSearch != null)
            {
                txtSearch.TextChanged += (s, e) => PerformSearch();
                txtSearch.GotFocus += (s, e) => { if (txtSearch.Text == "Search Customer...") txtSearch.Text = ""; };
                txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) txtSearch.Text = "Search Customer..."; };
            }

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
        // 🛠️ HELPER: Get the 'Assets' Folder in your Project
        // ==========================================================
        private string GetProjectImagesFolder()
        {
            // This trick finds your Source Code folder by going Up 2 levels from 'bin/Debug'
            // NOTE: This works perfectly in Visual Studio. 
            string projectPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;

            // Define the structure: Assets -> Images -> Customers
            string targetFolder = Path.Combine(projectPath, "Assets", "Images", "Customers");

            // Create it if it doesn't exist
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            return targetFolder;
        }

        // ==========================================================
        // 1. DATA LOADING & SEARCH
        // ==========================================================
        private void PerformSearch()
        {
            string term = txtSearch?.Text.Trim();
            if (string.IsNullOrEmpty(term) || term == "Search Customer...") { LoadCustomers(); return; }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_SearchCustomers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_SearchTerm", term);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd)) { FillGrid(adapter); }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Search Error: " + ex.Message); }
            }
        }

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
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd)) { FillGrid(adapter); }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Load Error: " + ex.Message); }
            }
        }

        private void FillGrid(MySqlDataAdapter adapter)
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            if (dgvCustomers != null)
            {
                dgvCustomers.DataSource = dt;
                FormatGrid();
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
        // 2. SELECTION & IMAGE LOADING
        // ==========================================================
        private void DgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers != null && dgvCustomers.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvCustomers.SelectedRows[0];
                if (row.Cells["CustomerId"].Value != null)
                    _selectedCustomerId = Convert.ToInt32(row.Cells["CustomerId"].Value);

                // --- Text Fields ---
                if (txtFirstName != null) txtFirstName.Text = row.Cells["FirstName"].Value?.ToString();
                if (txtLastName != null) txtLastName.Text = row.Cells["LastName"].Value?.ToString();
                if (txtEmail != null) txtEmail.Text = row.Cells["Email"].Value?.ToString();
                if (txtPhone != null) txtPhone.Text = row.Cells["Phone"].Value?.ToString();
                if (txtAddress != null) txtAddress.Text = row.Cells["Address"].Value?.ToString();
                if (txtEmergencyName != null) txtEmergencyName.Text = row.Cells["EmergencyContactName"].Value?.ToString();
                if (txtEmergencyPhone != null) txtEmergencyPhone.Text = row.Cells["EmergencyContactPhone"].Value?.ToString();
                if (txtLicenseNum != null) txtLicenseNum.Text = row.Cells["LicenseNumber"].Value?.ToString();

                // --- Dates ---
                if (dtpDOB != null && row.Cells["DOB"].Value != DBNull.Value)
                    dtpDOB.Value = Convert.ToDateTime(row.Cells["DOB"].Value);

                if (dtpExpiryDate != null && row.Cells["LicenseExpiry"].Value != DBNull.Value)
                    dtpExpiryDate.Value = Convert.ToDateTime(row.Cells["LicenseExpiry"].Value);

                // --- Status ---
                if (cbCustomerType != null) cbCustomerType.Text = row.Cells["CustomerType"].Value?.ToString();
                if (chkBlacklist != null) chkBlacklist.Checked = Convert.ToBoolean(row.Cells["IsBlacklisted"].Value);

                // --- 📸 SMART IMAGE LOADING ---
                string fileName = "";
                if (dgvCustomers.Columns.Contains("PhotoPath") && row.Cells["PhotoPath"].Value != DBNull.Value)
                {
                    fileName = row.Cells["PhotoPath"].Value.ToString();
                }

                // If the DB has "my_pic.jpg", we combine it with the project folder path
                if (!string.IsNullOrEmpty(fileName))
                {
                    string fullPath = Path.Combine(GetProjectImagesFolder(), fileName);
                    LoadCustomerImage(fullPath);
                }
                else
                {
                    LoadCustomerImage(null); // Clear image
                }

                LoadCustomerStats(_selectedCustomerId);

                if (btnSave != null) btnSave.Text = "Update Customer";
                _newPhotoTempPath = ""; // Reset temp
            }
        }

        private void LoadCustomerImage(string path)
        {
            if (picCustomerPhoto == null) return;

            // Cleanup old image to free memory
            if (picCustomerPhoto.Image != null) { picCustomerPhoto.Image.Dispose(); picCustomerPhoto.Image = null; }

            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                try
                {
                    // Stream method prevents file locking
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

        private void LoadCustomerStats(int customerId)
        {
            // (Keep your stats logic same as before, simplified for brevity here)
            // ...
        }

        // ==========================================================
        // 3. PHOTO UPLOAD / CAMERA
        // ==========================================================
        private void BtnUploadPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Images|*.jpg;*.jpeg;*.png" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadCustomerImage(ofd.FileName);
                _newPhotoTempPath = ofd.FileName; // Remember this path to save later
            }
        }

        private void BtnCamera_Click(object sender, EventArgs e)
        {
            CameraForm cam = new CameraForm();
            if (cam.ShowDialog() == DialogResult.OK)
            {
                // Save temporarily to AppData or Temp so we can see preview
                string tempFile = Path.Combine(Path.GetTempPath(), $"Capture_{DateTime.Now.Ticks}.jpg");
                cam.CapturedImage.Save(tempFile, System.Drawing.Imaging.ImageFormat.Jpeg);

                LoadCustomerImage(tempFile);
                _newPhotoTempPath = tempFile;
            }
        }

       
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtFirstName == null || string.IsNullOrWhiteSpace(txtFirstName.Text))
            { MessageBox.Show("First Name required."); return; }

            // 1. Determine Image Filename
            string finalFileName = "";

            // If editing and didn't change photo, keep old filename
            if (_selectedCustomerId != -1 && string.IsNullOrEmpty(_newPhotoTempPath))
            {
                if (dgvCustomers.SelectedRows.Count > 0 && dgvCustomers.Columns.Contains("PhotoPath"))
                    finalFileName = dgvCustomers.SelectedRows[0].Cells["PhotoPath"].Value?.ToString();
            }

            // 2. If NEW photo, copy it to Assets folder
            if (!string.IsNullOrEmpty(_newPhotoTempPath))
            {
                try
                {
                    string assetsFolder = GetProjectImagesFolder(); 

                   
                    string safeName = $"{txtFirstName.Text}_{txtLastName.Text}_{DateTime.Now.Ticks}".Replace(" ", "");
                    finalFileName = $"Cust_{safeName}.jpg";

                    string destPath = Path.Combine(assetsFolder, finalFileName);

                   
                    File.Copy(_newPhotoTempPath, destPath, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Photo Save Error: " + ex.Message);
                    return;
                }
            }

            
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

                    // KEY CHANGE: Saving just "photo.jpg", not "C:\Users\..."
                    cmd.Parameters.AddWithValue("@p_PhotoPath", finalFileName);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Customer saved successfully!");
                    LoadCustomers();
                    ClearForm();
                }
                catch (Exception ex) { MessageBox.Show("DB Error: " + ex.Message); }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId == -1) return;
            if (MessageBox.Show("Delete this customer?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        using (var cmd = new MySqlCommand("sp_DeleteCustomer", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_CustomerId", _selectedCustomerId);
                            cmd.ExecuteNonQuery();
                        }
                        LoadCustomers();
                        ClearForm();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
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
    }
}