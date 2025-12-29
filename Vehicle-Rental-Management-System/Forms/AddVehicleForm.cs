using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class AddVehicleForm : Form
    {
        private string _selectedImagePath = "";

        // Connection String
        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                    ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

        public AddVehicleForm()
        {
            InitializeComponent();

            // --- CRITICAL FIX ---
            // We call this HERE to ensure it always runs, even if the Load event is broken.
            LoadCategories();
            SetDefaultValues();
            SetupPictureBox();
        }

        // We can leave this empty now, since we called everything above.
        private void AddVehicleForm_Load(object sender, EventArgs e) { }

        // ==========================================
        // 1. LOAD DATA & WIPE MANUAL ITEMS
        // ==========================================
        private void LoadCategories()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Using Stored Procedure or Query - both work. Let's use Query for simplicity.
                    string query = "SELECT CategoryId, CategoryName FROM vehiclecategories ORDER BY CategoryName";

                    using (MySqlDataAdapter da = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // FIX: Wipe manual items ("Sedan", "SUV" text)
                        cbCategory.DataSource = null;
                        cbCategory.Items.Clear();

                        if (dt.Rows.Count > 0)
                        {
                            // Bind Database Items (These have hidden IDs)
                            cbCategory.DisplayMember = "CategoryName";
                            cbCategory.ValueMember = "CategoryId";
                            cbCategory.DataSource = dt;

                            cbCategory.SelectedIndex = -1; // Default to empty
                        }
                        else
                        {
                            MessageBox.Show("Error: No categories found in database table 'vehiclecategories'.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message);
                }
            }
        }

        // ==========================================
        // 2. SAVE LOGIC
        // ==========================================
        private void btnSave_Click(object sender, EventArgs e)
        {
            // A. VALIDATION
            if (string.IsNullOrWhiteSpace(txtMake.Text) || string.IsNullOrWhiteSpace(txtModel.Text))
            {
                MessageBox.Show("Make and Model are required.");
                return;
            }

            if (cbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Category.", "Required Field");
                return;
            }

            // B. GET VALID ID
            int categoryId = 0;

            // Safe extraction method
            if (cbCategory.SelectedItem is DataRowView row)
            {
                categoryId = Convert.ToInt32(row["CategoryId"]);
            }
            else
            {
                // Fallback
                try { categoryId = Convert.ToInt32(cbCategory.SelectedValue); } catch { }
            }

            if (categoryId == 0)
            {
                // If this hits, it means you are selecting a MANUAL item, not a DATABASE item.
                MessageBox.Show("Error: Category ID is 0. Please close this form, go to Design View, click the ComboBox, and clear the 'Items' property.");
                return;
            }

            // C. SAVE TO DB
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            int newVehicleId = 0;

                            using (MySqlCommand cmd = new MySqlCommand("sp_AddVehicle", conn, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@p_Make", txtMake.Text.Trim());
                                cmd.Parameters.AddWithValue("@p_Model", txtModel.Text.Trim());
                                cmd.Parameters.AddWithValue("@p_Year", numYear.Value);
                                cmd.Parameters.AddWithValue("@p_Color", txtColor.Text.Trim());
                                cmd.Parameters.AddWithValue("@p_LicensePlate", txtPlate.Text.Trim().ToUpper());
                                cmd.Parameters.AddWithValue("@p_VIN", txtVIN.Text.Trim());
                                cmd.Parameters.AddWithValue("@p_CategoryId", categoryId); // Real ID
                                cmd.Parameters.AddWithValue("@p_Transmission", cbTransmission.Text);
                                cmd.Parameters.AddWithValue("@p_FuelType", cbFuel.Text);
                                cmd.Parameters.AddWithValue("@p_SeatingCapacity", numSeats.Value);
                                cmd.Parameters.AddWithValue("@p_CurrentMileage", numMileage.Value);
                                cmd.Parameters.AddWithValue("@p_DailyRate", numRate.Value);
                                cmd.Parameters.AddWithValue("@p_Status", "Available");

                                object result = cmd.ExecuteScalar();
                                if (result != null) newVehicleId = Convert.ToInt32(result);
                            }

                            if (newVehicleId == 0)
                            {
                                using (MySqlCommand idCmd = new MySqlCommand("SELECT LAST_INSERT_ID();", conn, transaction))
                                {
                                    newVehicleId = Convert.ToInt32(idCmd.ExecuteScalar());
                                }
                            }

                            // Image Saving
                            if (newVehicleId > 0 && !string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                            {
                                string savedPath = SaveVehicleImage(newVehicleId, _selectedImagePath);
                                if (!string.IsNullOrEmpty(savedPath))
                                {
                                    string updateQuery = "UPDATE Vehicles SET ImagePath = @path WHERE VehicleId = @id";
                                    using (MySqlCommand imgCmd = new MySqlCommand(updateQuery, conn, transaction))
                                    {
                                        imgCmd.Parameters.AddWithValue("@path", savedPath);
                                        imgCmd.Parameters.AddWithValue("@id", newVehicleId);
                                        imgCmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            transaction.Commit();
                            MessageBox.Show("Vehicle added successfully!");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Duplicate"))
                        MessageBox.Show("License Plate or VIN already exists.");
                    else if (ex.Message.Contains("foreign key"))
                        MessageBox.Show($"Database Error: The database rejected Category ID {categoryId}. Ensure this ID exists in the 'vehiclecategories' table.");
                    else
                        MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // --- Helpers ---
        private void SetDefaultValues()
        {
            numYear.Value = DateTime.Now.Year;
            numSeats.Value = 5;
            if (cbTransmission.Items.Count > 0) cbTransmission.SelectedIndex = 0;
            if (cbFuel.Items.Count > 0) cbFuel.SelectedIndex = 0;
        }

        private void SetupPictureBox()
        {
            picVehicleImage.BorderStyle = BorderStyle.FixedSingle;
            picVehicleImage.SizeMode = PictureBoxSizeMode.Zoom;
            picVehicleImage.Cursor = Cursors.Hand;
            picVehicleImage.Click += PicVehicleImage_Click;
            ShowDefaultImage();
        }

        private void ShowDefaultImage()
        {
            try
            {
                Bitmap bmp = new Bitmap(picVehicleImage.Width, picVehicleImage.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.WhiteSmoke);
                    using (Font f = new Font("Arial", 10, FontStyle.Bold))
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.DrawString("Click to add image", f, Brushes.Gray, new RectangleF(0, 0, bmp.Width, bmp.Height), sf);
                    }
                }
                picVehicleImage.Image = bmp;
            }
            catch { }
        }

        private string SaveVehicleImage(int vehicleId, string sourcePath)
        {
            try
            {
                string folder = Path.Combine(Application.StartupPath, "VehicleImages");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string ext = Path.GetExtension(sourcePath);
                string newName = $"vehicle_{vehicleId}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                string destPath = Path.Combine(folder, newName);
                File.Copy(sourcePath, destPath, true);
                return destPath;
            }
            catch { return null; }
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = ofd.FileName;
                    picVehicleImage.Image = Image.FromFile(_selectedImagePath);
                }
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            _selectedImagePath = "";
            ShowDefaultImage();
        }

        private void PicVehicleImage_Click(object sender, EventArgs e) { btnSelectImage.PerformClick(); }
        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e) { }
        private void AddVehicleForm_FormClosing(object sender, FormClosingEventArgs e) { if (picVehicleImage.Image != null) picVehicleImage.Image.Dispose(); }
    }
}