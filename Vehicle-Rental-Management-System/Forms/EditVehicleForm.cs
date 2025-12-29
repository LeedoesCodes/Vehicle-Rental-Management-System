using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class EditVehicleForm : Form
    {
        private int _vehicleId;

        // Connection String
        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                    ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

        public EditVehicleForm(int vehicleId)
        {
            InitializeComponent();

            _vehicleId = vehicleId;
            this.Text = "Edit Vehicle";

            // Load data immediately when form opens
            LoadCategories();
            LoadVehicleDetails();
        }

        private void EditVehicleForm_Load(object sender, EventArgs e) { }

        private void LoadCategories()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Using direct SQL here to ensure we hit the correct table 'vehiclecategories'
                    string query = "SELECT CategoryId, CategoryName FROM vehiclecategories";

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
                    // Using Stored Procedure to get vehicle data
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMake.Text) || string.IsNullOrWhiteSpace(txtModel.Text))
            {
                MessageBox.Show("Make and Model are required.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Using Stored Procedure to Update
                    using (MySqlCommand cmd = new MySqlCommand("sp_UpdateVehicle", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parameters matching your NEW Stored Procedure (No Status)
                        cmd.Parameters.AddWithValue("@p_VehicleId", _vehicleId);
                        cmd.Parameters.AddWithValue("@p_Make", txtMake.Text);
                        cmd.Parameters.AddWithValue("@p_Model", txtModel.Text);
                        cmd.Parameters.AddWithValue("@p_Year", numYear.Value);
                        cmd.Parameters.AddWithValue("@p_Color", txtColor.Text);
                        cmd.Parameters.AddWithValue("@p_LicensePlate", txtPlate.Text);
                        cmd.Parameters.AddWithValue("@p_VIN", txtVIN.Text);
                        cmd.Parameters.AddWithValue("@p_CategoryId", cbCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@p_DailyRate", numRate.Value);
                        cmd.Parameters.AddWithValue("@p_Transmission", cbTransmission.Text);
                        cmd.Parameters.AddWithValue("@p_FuelType", cbFuel.Text);
                        cmd.Parameters.AddWithValue("@p_SeatingCapacity", numSeats.Value);
                        cmd.Parameters.AddWithValue("@p_CurrentMileage", numMileage.Value);

                        // NOTE: p_Status is NOT added here, preventing the error.

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
    }
}