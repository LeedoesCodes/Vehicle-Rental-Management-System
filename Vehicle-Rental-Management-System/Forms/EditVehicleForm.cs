using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class EditVehicleForm : Form
    {
        private int _vehicleId;
        private string _selectedImagePath = "";
        private string _originalImagePath = "";
        private bool _imageChanged = false;

        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                    ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

        public EditVehicleForm(int vehicleId)
        {
            InitializeComponent();

            _vehicleId = vehicleId;
            this.Text = "Edit Vehicle";

            // Setup PictureBox
            SetupPictureBox();

            // Load data immediately when form opens
            LoadCategories();
            LoadVehicleDetails();
        }

        private void SetupPictureBox()
        {
            // Make PictureBox look nicer
            picVehicle.BorderStyle = BorderStyle.FixedSingle;
            picVehicle.SizeMode = PictureBoxSizeMode.Zoom;
            picVehicle.Cursor = Cursors.Hand;
            picVehicle.Click += PicVehicle_Click;
        }

        private void EditVehicleForm_Load(object sender, EventArgs e) { }

        private void LoadCategories()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CategoryId, CategoryName FROM vehiclecategories ORDER BY CategoryName";

                    using (MySqlDataAdapter da = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        cbCategory.DataSource = dt;
                        cbCategory.DisplayMember = "CategoryName";
                        cbCategory.ValueMember = "CategoryId";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message);
                }
            }
        }

        private void LoadVehicleDetails()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetVehicleById", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_VehicleId", _vehicleId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtMake.Text = reader["Make"].ToString();
                                txtModel.Text = reader["Model"].ToString();
                                txtColor.Text = reader["Color"].ToString();
                                txtPlate.Text = reader["LicensePlate"].ToString();
                                txtVIN.Text = reader["VIN"].ToString();

                                if (reader["Year"] != DBNull.Value)
                                    numYear.Value = Convert.ToDecimal(reader["Year"]);

                                if (reader["DailyRate"] != DBNull.Value)
                                    numRate.Value = Convert.ToDecimal(reader["DailyRate"]);

                                if (reader["SeatingCapacity"] != DBNull.Value)
                                    numSeats.Value = Convert.ToDecimal(reader["SeatingCapacity"]);

                                if (reader["CurrentMileage"] != DBNull.Value)
                                    numMileage.Value = Convert.ToDecimal(reader["CurrentMileage"]);

                                if (reader["Transmission"] != DBNull.Value)
                                    cbTransmission.SelectedItem = reader["Transmission"].ToString();

                                if (reader["FuelType"] != DBNull.Value)
                                    cbFuel.SelectedItem = reader["FuelType"].ToString();

                                if (reader["CategoryId"] != DBNull.Value)
                                    cbCategory.SelectedValue = reader["CategoryId"];

                                // Load image if exists
                                if (reader["ImagePath"] != DBNull.Value)
                                {
                                    string path = reader["ImagePath"].ToString();
                                    _originalImagePath = path;

                                    if (File.Exists(path))
                                    {
                                        try
                                        {
                                            Image img = Image.FromFile(path);
                                            picVehicle.Image = img;
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
                                else
                                {
                                    ShowDefaultImage();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Vehicle not found!");
                                this.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading details: " + ex.Message);
                }
            }
        }

        private void ShowDefaultImage()
        {
            try
            {
                Bitmap defaultImage = new Bitmap(picVehicle.Width, picVehicle.Height);
                using (Graphics g = Graphics.FromImage(defaultImage))
                {
                    g.Clear(Color.WhiteSmoke);
                    using (Font font = new Font("Arial", 12, FontStyle.Bold))
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        g.DrawString("No Vehicle Image", font, Brushes.Gray,
                                    new RectangleF(0, 0, defaultImage.Width, defaultImage.Height), sf);
                    }
                }
                picVehicle.Image = defaultImage;
            }
            catch
            {
                picVehicle.Image = null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMake.Text) || string.IsNullOrWhiteSpace(txtModel.Text))
            {
                MessageBox.Show("Make and Model are required.");
                return;
            }

            // Handle image
            string finalImagePath = _originalImagePath;

            if (_imageChanged)
            {
                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    try
                    {
                        // Create VehicleImages folder if it doesn't exist
                        string folder = Path.Combine(Application.StartupPath, "VehicleImages");
                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);

                        // Generate unique filename
                        string ext = Path.GetExtension(_selectedImagePath);
                        string newFileName = $"vehicle_{_vehicleId}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                        string destinationPath = Path.Combine(folder, newFileName);

                        // Copy the image
                        File.Copy(_selectedImagePath, destinationPath, true);
                        finalImagePath = destinationPath;

                        // Optionally delete old image if it exists and is different
                        if (!string.IsNullOrEmpty(_originalImagePath) &&
                            _originalImagePath != finalImagePath &&
                            File.Exists(_originalImagePath))
                        {
                            try
                            {
                                File.Delete(_originalImagePath);
                            }
                            catch
                            {
                                // Ignore deletion errors
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving image: " + ex.Message);
                        return;
                    }
                }
                else if (string.IsNullOrEmpty(_selectedImagePath) && !string.IsNullOrEmpty(_originalImagePath))
                {
                    // User removed the image - delete the file
                    try
                    {
                        if (File.Exists(_originalImagePath))
                            File.Delete(_originalImagePath);
                    }
                    catch
                    {
                        // Ignore deletion errors
                    }
                    finalImagePath = null; // Set to null to delete from database
                }
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Use the NEW stored procedure with image parameter
                    using (MySqlCommand cmd = new MySqlCommand("sp_UpdateVehicleWithImage", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Vehicle parameters including image
                        cmd.Parameters.AddWithValue("@p_VehicleId", _vehicleId);
                        cmd.Parameters.AddWithValue("@p_Make", txtMake.Text);
                        cmd.Parameters.AddWithValue("@p_Model", txtModel.Text);
                        cmd.Parameters.AddWithValue("@p_Year", (int)numYear.Value);
                        cmd.Parameters.AddWithValue("@p_Color", txtColor.Text);
                        cmd.Parameters.AddWithValue("@p_LicensePlate", txtPlate.Text);
                        cmd.Parameters.AddWithValue("@p_VIN", txtVIN.Text);
                        cmd.Parameters.AddWithValue("@p_CategoryId", cbCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@p_Transmission", cbTransmission.Text);
                        cmd.Parameters.AddWithValue("@p_FuelType", cbFuel.Text);
                        cmd.Parameters.AddWithValue("@p_SeatingCapacity", (int)numSeats.Value);
                        cmd.Parameters.AddWithValue("@p_CurrentMileage", numMileage.Value);
                        cmd.Parameters.AddWithValue("@p_DailyRate", numRate.Value);
                        cmd.Parameters.AddWithValue("@p_ImagePath", finalImagePath ?? (object)DBNull.Value);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Vehicle updated successfully!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating: " + ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                ofd.Title = "Select Vehicle Image";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _selectedImagePath = ofd.FileName;
                        _imageChanged = true;

                        // Load and display the image
                        Image img = Image.FromFile(_selectedImagePath);
                        picVehicle.Image = img;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message);
                        ShowDefaultImage();
                    }
                }
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Remove current image?", "Confirm",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _selectedImagePath = "";
                _imageChanged = true;
                ShowDefaultImage();

                // Mark for deletion from database
                _originalImagePath = "";
            }
        }

        private void PicVehicle_Click(object sender, EventArgs e)
        {
            // When user clicks on the picture box, open image selection
            btnSelectImage.PerformClick();
        }

        private void EditVehicleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up image resources
            if (picVehicle.Image != null)
            {
                // Only dispose if it's the default image (created by us)
                // or if we're closing without saving
                if (this.DialogResult != DialogResult.OK)
                {
                    picVehicle.Image.Dispose();
                }
            }
        }
    }
}