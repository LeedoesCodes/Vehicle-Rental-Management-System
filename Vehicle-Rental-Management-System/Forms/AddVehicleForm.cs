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
        private DataTable _categoriesTable;

        public AddVehicleForm()
        {
            InitializeComponent();
        }

        private void AddVehicleForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            SetDefaultValues();
            SetupPictureBox();
        }

        private void SetupPictureBox()
        {
            // Make PictureBox look nicer
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
                Bitmap defaultImage = new Bitmap(picVehicleImage.Width, picVehicleImage.Height);
                using (Graphics g = Graphics.FromImage(defaultImage))
                {
                    g.Clear(Color.WhiteSmoke);
                    using (Font font = new Font("Arial", 10, FontStyle.Bold))
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        g.DrawString("Click to add vehicle image", font, Brushes.Gray,
                                    new RectangleF(0, 0, defaultImage.Width, defaultImage.Height), sf);
                    }
                }
                picVehicleImage.Image = defaultImage;
            }
            catch
            {
                picVehicleImage.Image = null;
            }
        }

        private void SetDefaultValues()
        {
            numYear.Value = DateTime.Now.Year;
            numSeats.Value = 5;

            // Pre-select first item if available to avoid null errors
            if (cbTransmission.Items.Count > 0) cbTransmission.SelectedIndex = 0;
            if (cbFuel.Items.Count > 0) cbFuel.SelectedIndex = 0;

            // Select the placeholder (index 0) instead of -1 so the text "-- Select Category --" shows up
            if (cbCategory.Items.Count > 0)
                cbCategory.SelectedIndex = 0;
        }

        private void LoadCategories()
        {
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Ensure this stored procedure returns columns named exactly 'CategoryId' and 'CategoryName'
                    MySqlDataAdapter da = new MySqlDataAdapter("sp_GetAllCategories", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    _categoriesTable = new DataTable();
                    da.Fill(_categoriesTable);

                    if (_categoriesTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No categories found in database. Please add categories first.",
                                      "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Create a display table to avoid modifying the original data source if used elsewhere
                    DataTable displayTable = _categoriesTable.Copy();

                    // Create a new row with the same schema as the table
                    DataRow emptyRow = displayTable.NewRow();
                    // Explicitly set ID to 0 for the placeholder
                    emptyRow["CategoryId"] = 0;
                    emptyRow["CategoryName"] = "-- Select Category --";

                    // Insert at top
                    displayTable.Rows.InsertAt(emptyRow, 0);

                    // Bind data
                    cbCategory.DataSource = displayTable;
                    cbCategory.DisplayMember = "CategoryName";
                    cbCategory.ValueMember = "CategoryId";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateForm()
        {
            // Check required text fields
            if (string.IsNullOrWhiteSpace(txtMake.Text))
            {
                MessageBox.Show("Make is required.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMake.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtModel.Text))
            {
                MessageBox.Show("Model is required.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtModel.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPlate.Text))
            {
                MessageBox.Show("License Plate is required.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPlate.Focus();
                return false;
            }

            // --- IMPROVED CATEGORY VALIDATION ---

            // 1. Check if anything is selected at all
            if (cbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Category.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbCategory.Focus();
                return false;
            }

            // 2. Check the VALUE of the selection
            // We use SelectedValue because DisplayMember/ValueMember are set.
            // If the user selects "-- Select Category --", the Value is 0.
            // If they select a real category, the Value is its ID (e.g., 1, 2, 5).

            if (cbCategory.SelectedValue is int selectedId)
            {
                if (selectedId <= 0)
                {
                    MessageBox.Show("Please select a valid Category.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbCategory.Focus();
                    return false;
                }
            }
            else if (cbCategory.SelectedValue is string selectedIdString)
            {
                if (int.TryParse(selectedIdString, out int id))
                {
                    if (id <= 0)
                    {
                        MessageBox.Show("Please select a valid Category.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbCategory.Focus();
                        return false;
                    }
                }
                else
                {
                    // Could not parse string value to int
                    MessageBox.Show("Invalid Category Selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                // SelectedValue is null or an unexpected type
                MessageBox.Show("Please select a valid Category.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbCategory.Focus();
                return false;
            }

            // Validate daily rate
            if (numRate.Value <= 0)
            {
                MessageBox.Show("Daily Rate must be greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numRate.Focus();
                return false;
            }

            // Validate year
            if (numYear.Value < 1900 || numYear.Value > DateTime.Now.Year + 1)
            {
                MessageBox.Show($"Year must be between 1900 and {DateTime.Now.Year + 1}.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numYear.Focus();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Validation
            if (!ValidateForm())
                return;

            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Start transaction to ensure both vehicle and image are saved
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            int newVehicleId = 0;
                            // Safe cast assuming ValueMember is correctly set to an integer column
                            int categoryId = Convert.ToInt32(cbCategory.SelectedValue);

                            // 1. Add Vehicle
                            using (MySqlCommand cmd = new MySqlCommand("sp_AddVehicle", conn, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@p_Make", txtMake.Text.Trim());
                                cmd.Parameters.AddWithValue("@p_Model", txtModel.Text.Trim());
                                cmd.Parameters.AddWithValue("@p_Year", numYear.Value);
                                cmd.Parameters.AddWithValue("@p_Color", txtColor.Text.Trim());
                                cmd.Parameters.AddWithValue("@p_LicensePlate", txtPlate.Text.Trim().ToUpper());
                                cmd.Parameters.AddWithValue("@p_VIN", txtVIN.Text.Trim());
                                cmd.Parameters.AddWithValue("@p_CategoryId", categoryId);
                                cmd.Parameters.AddWithValue("@p_Transmission", cbTransmission.SelectedItem?.ToString() ?? "Automatic");
                                cmd.Parameters.AddWithValue("@p_FuelType", cbFuel.SelectedItem?.ToString() ?? "Gasoline");
                                cmd.Parameters.AddWithValue("@p_SeatingCapacity", numSeats.Value);
                                cmd.Parameters.AddWithValue("@p_CurrentMileage", numMileage.Value);
                                cmd.Parameters.AddWithValue("@p_DailyRate", numRate.Value);
                                cmd.Parameters.AddWithValue("@p_Status", "Available");

                                // Get the new VehicleId
                                object result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    newVehicleId = Convert.ToInt32(result);
                                }
                                else
                                {
                                    throw new Exception("Failed to retrieve new vehicle ID.");
                                }
                            }

                            // 2. Handle Image if selected
                            if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                            {
                                string savedImagePath = SaveVehicleImage(newVehicleId, _selectedImagePath);
                                if (!string.IsNullOrEmpty(savedImagePath))
                                {
                                    AddVehicleImage(newVehicleId, savedImagePath, conn, transaction);
                                }
                            }

                            // Commit transaction
                            transaction.Commit();

                            MessageBox.Show($"Vehicle added successfully!\nVehicle ID: {newVehicleId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // This tells the parent form (VehiclesView) that we succeeded
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw; // Re-throw to outer catch block
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1062) // Duplicate entry error code for MySQL
                        MessageBox.Show("A vehicle with this License Plate or VIN already exists.", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string SaveVehicleImage(int vehicleId, string sourceImagePath)
        {
            try
            {
                // Create VehicleImages folder if it doesn't exist
                string folder = Path.Combine(Application.StartupPath, "VehicleImages");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // Generate unique filename
                string ext = Path.GetExtension(sourceImagePath);
                string newFileName = $"vehicle_{vehicleId}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                string destinationPath = Path.Combine(folder, newFileName);

                // Copy the image
                File.Copy(sourceImagePath, destinationPath, true);

                return destinationPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving image: {ex.Message}\nVehicle will be saved without image.", "Warning",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        private void AddVehicleImage(int vehicleId, string imagePath, MySqlConnection conn, MySqlTransaction transaction)
        {
            try
            {
                string insertQuery = @"
                    INSERT INTO vehicle_images 
                    (VehicleId, ImagePath, IsPrimary, UploadDate, ImageType) 
                    VALUES (@VehicleId, @ImagePath, 1, NOW(), 'vehicle')";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log error but don't stop the process
                Console.WriteLine($"Error adding image to database: {ex.Message}");
                // Optionally show a warning message
                MessageBox.Show($"Note: Vehicle was saved but image could not be added to database: {ex.Message}",
                              "Partial Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _selectedImagePath = ofd.FileName;

                        // Load and display the image
                        Image img = Image.FromFile(_selectedImagePath);
                        picVehicleImage.Image = img;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message, "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ShowDefaultImage();
                    }
                }
            }
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            _selectedImagePath = "";
            ShowDefaultImage();
        }

        private void PicVehicleImage_Click(object sender, EventArgs e)
        {
            // When user clicks on the picture box, open image selection
            btnSelectImage.PerformClick();
        }

        private void AddVehicleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up image resources
            if (picVehicleImage.Image != null)
            {
                picVehicleImage.Image.Dispose();
            }
        }

        // Debug method to check what's happening
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Uncomment to debug
            /*
            if (cbCategory.SelectedValue != null)
            {
                Console.WriteLine($"Selected Index: {cbCategory.SelectedIndex}");
                Console.WriteLine($"Selected Value: {cbCategory.SelectedValue}");
                Console.WriteLine($"Selected Value Type: {cbCategory.SelectedValue.GetType()}");
                Console.WriteLine($"Selected Text: {cbCategory.Text}");
            }
            */
        }
    }
}