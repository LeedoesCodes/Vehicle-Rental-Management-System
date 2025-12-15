using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class AddVehicleForm : Form
    {
        public AddVehicleForm()
        {
            InitializeComponent();
        }

        private void AddVehicleForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            numYear.Value = DateTime.Now.Year;
            numSeats.Value = 5;

            // Pre-select first item if available to avoid null errors
            if (cbTransmission.Items.Count > 0) cbTransmission.SelectedIndex = 0;
            if (cbFuel.Items.Count > 0) cbFuel.SelectedIndex = 0;
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
                    MySqlDataAdapter da = new MySqlDataAdapter("sp_GetAllCategories", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbCategory.DataSource = dt;
                    cbCategory.DisplayMember = "CategoryName";
                    cbCategory.ValueMember = "CategoryId";
                    cbCategory.SelectedIndex = -1; // Default to empty
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Validation
            if (string.IsNullOrWhiteSpace(txtMake.Text) ||
                string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtPlate.Text))
            {
                MessageBox.Show("Please fill in Make, Model, and License Plate.", "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbCategory.SelectedValue == null)
            {
                MessageBox.Show("Please select a Category.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Save Logic
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_AddVehicle", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Use .Trim() to remove accidental spaces
                        cmd.Parameters.AddWithValue("@p_Make", txtMake.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_Model", txtModel.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_Year", numYear.Value);
                        cmd.Parameters.AddWithValue("@p_Color", txtColor.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_LicensePlate", txtPlate.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@p_VIN", txtVIN.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_CategoryId", cbCategory.SelectedValue);

                        // Handle potential nulls in dropdowns safely
                        cmd.Parameters.AddWithValue("@p_Transmission", cbTransmission.SelectedItem?.ToString() ?? "Automatic");
                        cmd.Parameters.AddWithValue("@p_FuelType", cbFuel.SelectedItem?.ToString() ?? "Gasoline");

                        cmd.Parameters.AddWithValue("@p_SeatingCapacity", numSeats.Value);
                        cmd.Parameters.AddWithValue("@p_CurrentMileage", numMileage.Value);
                        cmd.Parameters.AddWithValue("@p_DailyRate", numRate.Value);
                        cmd.Parameters.AddWithValue("@p_Status", "Available");

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Vehicle added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // This tells the parent form (VehiclesView) that we succeeded
                    this.DialogResult = DialogResult.OK;
                    this.Close();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}