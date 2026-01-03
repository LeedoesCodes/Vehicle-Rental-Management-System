using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class NewRentalForm : Form
    {
        private decimal _currentDailyRate = 0;

        public NewRentalForm()
        {
            InitializeComponent();

            // =========================================================
            // CRITICAL: EVENT LINKS
            // These lines ensure the math runs automatically
            // =========================================================
            cbVehicle.SelectedIndexChanged += CbVehicle_SelectedIndexChanged;
            dtPickup.ValueChanged += UpdateTotalCalculation;
            dtReturn.ValueChanged += UpdateTotalCalculation;

            // Defaults
            dtPickup.Value = DateTime.Now;
            dtReturn.Value = DateTime.Now.AddDays(1);

            if (cbFuel.Items.Count > 0 && cbFuel.SelectedIndex == -1)
                cbFuel.SelectedIndex = 4;

            LoadData();
        }

        private void LoadData()
        {
            string connString = GetConnectionString();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Load Customers
                    string custQuery = "SELECT CustomerId, CONCAT(FirstName, ' ', LastName) as FullName FROM Customers ORDER BY LastName";
                    MySqlDataAdapter daCust = new MySqlDataAdapter(custQuery, conn);
                    DataTable dtCust = new DataTable();
                    daCust.Fill(dtCust);

                    cbCustomer.DisplayMember = "FullName";
                    cbCustomer.ValueMember = "CustomerId";
                    cbCustomer.DataSource = dtCust;
                    cbCustomer.SelectedIndex = -1;

                    // Load Vehicles (Fetching Mileage & Fuel Level)
                    string vehicleQuery = @"SELECT 
                                            VehicleId, 
                                            CONCAT(Make, ' ', Model, ' - ', LicensePlate) as DisplayName, 
                                            DailyRate, 
                                            CurrentMileage, 
                                            CurrentFuelLevel 
                                          FROM Vehicles 
                                          WHERE Status = 'Available'";

                    MySqlDataAdapter daVeh = new MySqlDataAdapter(vehicleQuery, conn);
                    DataTable dtVeh = new DataTable();
                    daVeh.Fill(dtVeh);

                    cbVehicle.DisplayMember = "DisplayName";
                    cbVehicle.ValueMember = "VehicleId";
                    cbVehicle.DataSource = dtVeh;
                    cbVehicle.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        private void CbVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVehicle.SelectedIndex != -1 && cbVehicle.SelectedItem is DataRowView row)
            {
                _currentDailyRate = Convert.ToDecimal(row["DailyRate"]);

                // Auto-fill Odometer
                if (row["CurrentMileage"] != DBNull.Value)
                    txtOdometer.Text = row["CurrentMileage"].ToString();
                else
                    txtOdometer.Text = "0";

                // Auto-select Fuel Level
                string dbFuelLevel = row["CurrentFuelLevel"] != DBNull.Value ? row["CurrentFuelLevel"].ToString() : "Full";
                if (cbFuel.Items.Contains(dbFuelLevel))
                    cbFuel.SelectedItem = dbFuelLevel;
                else
                    cbFuel.Text = "Full";

                // Force recalculation immediately
                UpdateTotalCalculation(sender, e);
            }
        }

        private void UpdateTotalCalculation(object sender, EventArgs e)
        {
            // Only calculate if a rate is set and dates are valid
            if (_currentDailyRate > 0 && dtReturn.Value > dtPickup.Value)
            {
                TimeSpan duration = dtReturn.Value.Date - dtPickup.Value.Date;
                int days = (int)duration.TotalDays;

                if (days < 1) days = 1;

                decimal total = days * _currentDailyRate;

                lblTotal.Text = $"Total: ₱{total:N2} ({days} Days)";
                lblTotal.Tag = total;
            }
            else
            {
                lblTotal.Text = "Total: ₱0.00";
                lblTotal.Tag = 0m;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbCustomer.SelectedValue == null) { MessageBox.Show("Please select a customer."); return; }
            if (cbVehicle.SelectedValue == null) { MessageBox.Show("Please select a vehicle."); return; }
            if (string.IsNullOrEmpty(txtOdometer.Text)) { MessageBox.Show("Please enter odometer reading."); return; }

            decimal totalAmount = Convert.ToDecimal(lblTotal.Tag);
            string connString = GetConnectionString();

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_CreateRental", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@p_VehicleId", cbVehicle.SelectedValue);
                        cmd.Parameters.AddWithValue("@p_CustomerId", cbCustomer.SelectedValue);
                        cmd.Parameters.AddWithValue("@p_PickupDate", dtPickup.Value);
                        cmd.Parameters.AddWithValue("@p_ReturnDate", dtReturn.Value);
                        cmd.Parameters.AddWithValue("@p_TotalAmount", totalAmount);
                        cmd.Parameters.AddWithValue("@p_OdometerStart", Convert.ToDecimal(txtOdometer.Text));
                        cmd.Parameters.AddWithValue("@p_FuelLevelStart", cbFuel.Text);
                        cmd.Parameters.AddWithValue("@p_Notes", txtNotes.Text);

                       
                        cmd.Parameters.AddWithValue("@p_UserId", 1);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Rental processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error processing rental: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetConnectionString()
        {
            if (ConfigurationManager.ConnectionStrings["MySqlConnection"] != null)
                return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            return "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
        }
    }
}