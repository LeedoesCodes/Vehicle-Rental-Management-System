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

        // Calculation Variables
        private decimal _startOdometer = 0;
        private decimal _dailyRate = 0;
        private DateTime _startDate;

        // Connection String
        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                    ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

        public ReturnVehicleForm(int rentalId, int vehicleId, string vehicleName, string customerName)
        {
            InitializeComponent();

            _rentalId = rentalId;
            _vehicleId = vehicleId;

            // 1. Setup UI Labels
            if (lblVehicleInfo != null) lblVehicleInfo.Text = $"Returning: {vehicleName}";
            if (lblCustomerInfo != null) lblCustomerInfo.Text = $"Customer: {customerName}";

            // 2. Setup Defaults
            dtReturns.Value = DateTime.Now;
            if (cbFuels.Items.Count > 0) cbFuels.SelectedIndex = 0;

            // 3. Link Events for Real-Time Calculation
            dtReturns.ValueChanged += (s, e) => CalculateTotal();
            if (numDamages != null) numDamages.ValueChanged += (s, e) => CalculateTotal();
            if (numLateFee != null) numLateFee.ValueChanged += (s, e) => CalculateTotal();

            // 4. Load Data
            LoadRentalDetails();
        }

        private void LoadRentalDetails()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Calls the FIXED Procedure (sp_GetRentalReturnDetails)
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetRentalReturnDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_RentalId", _rentalId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 1. Odometer
                                if (reader["OdometerStart"] != DBNull.Value)
                                {
                                    _startOdometer = Convert.ToDecimal(reader["OdometerStart"]);
                                    if (txtOdometers != null) txtOdometers.Text = _startOdometer.ToString();
                                }

                                // 2. Pickup Date (Correct Column Name)
                                if (reader["PickupDate"] != DBNull.Value)
                                    _startDate = Convert.ToDateTime(reader["PickupDate"]);

                                // 3. Daily Rate (Correct Column Name)
                                if (reader["DailyRate"] != DBNull.Value)
                                    _dailyRate = Convert.ToDecimal(reader["DailyRate"]);
                            }
                        }
                    }
                    CalculateTotal(); // Initial calc
                }
                catch (Exception ex) { MessageBox.Show("Error loading details: " + ex.Message); }
            }
        }

        private void CalculateTotal()
        {
            // Avoid crash if UI not ready
            if (lblTotal == null) return;

            // A. Calculate Days
            TimeSpan duration = dtReturns.Value.Date - _startDate.Date;
            int days = duration.Days;
            if (days < 1) days = 1;

            // B. Costs
            decimal rentalCost = days * _dailyRate;
            decimal damageCost = numDamages != null ? numDamages.Value : 0;
            decimal lateFees = numLateFee != null ? numLateFee.Value : 0;
            decimal grandTotal = rentalCost + damageCost + lateFees;

            // C. Display
            lblTotal.Text = $"Days: {days} | Total: {grandTotal:C2}";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // --- VALIDATION ---
            if (string.IsNullOrEmpty(txtOdometers.Text))
            {
                MessageBox.Show("Please enter the final odometer reading.");
                return;
            }

            decimal finalOdometer = 0;
            if (!decimal.TryParse(txtOdometers.Text, out finalOdometer))
            {
                MessageBox.Show("Invalid odometer number.");
                return;
            }

            if (finalOdometer < _startOdometer)
            {
                MessageBox.Show($"Error: Final odometer ({finalOdometer}) cannot be lower than start ({_startOdometer}).");
                return;
            }

            if (MessageBox.Show("Confirm return and process payment?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            SaveReturnTransaction(finalOdometer);
        }

        private void SaveReturnTransaction(decimal finalOdometer)
        {
            // 1. Calculate Totals
            TimeSpan duration = dtReturns.Value.Date - _startDate.Date;
            int days = (duration.Days < 1) ? 1 : duration.Days;

            // Safety check for null controls
            decimal damageFee = (numDamages != null) ? numDamages.Value : 0;
            decimal lateFee = (numLateFee != null) ? numLateFee.Value : 0;

            decimal totalAmount = (days * _dailyRate) + damageFee + lateFee;

            // 2. ➤ AUTO-FLAG DAMAGES 
            // This fixes your issue: If money is charged, force the word "Damage" into the text
            string conditionText = txtConditions.Text;
            if (damageFee > 0)
            {
                // Only add if it's not already typed
                if (!conditionText.ToLower().Contains("damage"))
                {
                    conditionText += " [Damage/Scratch Fee Charged]";
                }
            }

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_ProcessReturn", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@p_RentalId", _rentalId);
                        cmd.Parameters.AddWithValue("@p_VehicleId", _vehicleId);
                        cmd.Parameters.AddWithValue("@p_ActualReturnDate", dtReturns.Value);
                        cmd.Parameters.AddWithValue("@p_OdometerEnd", finalOdometer);
                        cmd.Parameters.AddWithValue("@p_FuelLevelEnd", cbFuels.SelectedItem?.ToString() ?? "Full");

                        // ➤ SEND THE MODIFIED TEXT
                        cmd.Parameters.AddWithValue("@p_FinalCondition", conditionText);

                        cmd.Parameters.AddWithValue("@p_TotalAmount", totalAmount);
                        cmd.Parameters.AddWithValue("@p_Notes", $"Return Processed. Damage: {damageFee:C2}");

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Vehicle Returned & Damage Recorded!", "Success");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Transaction Failed: " + ex.Message);
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