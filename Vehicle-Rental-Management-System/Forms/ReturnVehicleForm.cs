using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class ReturnVehicleForm : Form
    {
        private int _rentalId;
        private int _vehicleId;
        private decimal _startOdometer = 0; // To store the starting value for validation

        public ReturnVehicleForm(int rentalId, int vehicleId, string vehicleName, string customerName)
        {
            InitializeComponent();

            _rentalId = rentalId;
            _vehicleId = vehicleId;

            // Set the labels with actual data
            lblVehicleInfo.Text = $"Returning: {vehicleName}";
            lblCustomerInfo.Text = $"Customer: {customerName}";

            dtReturns.Value = DateTime.Now;

       
            cbFuels.SelectedIndex = 4; 

  
            LoadRentalDetails();
        }

        private void LoadRentalDetails()
        {
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // FIX: We now select 'OdometerStart' (matches your database)
                    string query = "SELECT OdometerStart FROM Rentals WHERE RentalId = @pid";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@pid", _rentalId);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            _startOdometer = Convert.ToDecimal(result);

                            // Pre-fill the textbox
                            txtOdometers.Text = _startOdometer.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading rental details: " + ex.Message);
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOdometers.Text))
            {
                MessageBox.Show("Please enter the final odometer reading.");
                return;
            }

            decimal finalOdometer = 0;
            if (!decimal.TryParse(txtOdometers.Text, out finalOdometer))
            {
                MessageBox.Show("Please enter a valid number for the odometer.");
                return;
            }

        
            if (finalOdometer < _startOdometer)
            {
                MessageBox.Show($"Error: The final odometer ({finalOdometer}) cannot be lower than the starting odometer ({_startOdometer}).",
                                "Invalid Mileage", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ReturnVehicle", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@p_RentalId", _rentalId);
                        cmd.Parameters.AddWithValue("@p_VehicleId", _vehicleId);
                        cmd.Parameters.AddWithValue("@p_ReturnDate", dtReturns.Value);
                        cmd.Parameters.AddWithValue("@p_OdometerEnd", finalOdometer);
                        cmd.Parameters.AddWithValue("@p_FuelLevelEnd", cbFuels.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@p_FinalCondition", txtConditions.Text);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Vehicle returned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error processing return: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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