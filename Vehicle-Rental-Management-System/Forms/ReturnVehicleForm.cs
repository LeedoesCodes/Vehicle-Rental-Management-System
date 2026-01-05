using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class ReturnVehicleForm : Form
    {
        private int _rentalId;
        private int _vehicleId;
        private decimal _dailyRate = 0;
        private DateTime _startDate;

        // ➤ THE LIST: Holds all damage reports in memory before saving
        private List<DamageReport> _damageReports = new List<DamageReport>();

        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                    ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

        public ReturnVehicleForm(int rentalId, int vehicleId, string vehicleName, string customerName)
        {
            InitializeComponent();
            _rentalId = rentalId;
            _vehicleId = vehicleId;

            if (lblVehicleInfo != null) lblVehicleInfo.Text = $"Returning: {vehicleName}";
            if (lblCustomerInfo != null) lblCustomerInfo.Text = $"Customer: {customerName}";

            dtReturns.Value = DateTime.Now;
            if (cbFuels.Items.Count > 0) cbFuels.SelectedIndex = 0;

            // Events to recalucate totals automatically
            dtReturns.ValueChanged += (s, e) => CalculateTotal();
            if (numLateFee != null) numLateFee.ValueChanged += (s, e) => CalculateTotal();
            if (btnAddDamage != null) btnAddDamage.Click += BtnAddDamage_Click; // ➤ Link the new button

            LoadRentalDetails();
        }

        // =============================================================
        // 1. ADDING DAMAGES (The New Logic)
        // =============================================================
        private void BtnAddDamage_Click(object sender, EventArgs e)
        {
            using (var form = new AddDamageForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _damageReports.Add(form.ResultReport); // Add to list
                    RefreshDamageGrid(); // Update UI
                    CalculateTotal();    // Update Price
                }
            }
        }

        private void RefreshDamageGrid()
        {
            if (dgvDamages == null) return;
            dgvDamages.DataSource = null;
            // Create a clean view for the grid
            dgvDamages.DataSource = _damageReports.Select(d => new {
                d.Type,
                d.Fee,
                Notes = d.Description,
                Photo = string.IsNullOrEmpty(d.TempImagePath) ? "No" : "Yes"
            }).ToList();
        }

        // =============================================================
        // 2. CALCULATIONS
        // =============================================================
        private void LoadRentalDetails()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetRentalReturnDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_RentalId", _rentalId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (txtOdometers != null) txtOdometers.Text = reader["OdometerStart"].ToString();
                                _startDate = Convert.ToDateTime(reader["PickupDate"]);
                                _dailyRate = Convert.ToDecimal(reader["DailyRate"]);
                            }
                        }
                    }
                    CalculateTotal();
                }
                catch (Exception ex) { MessageBox.Show("Error loading details: " + ex.Message); }
            }
        }

        private void CalculateTotal()
        {
            if (lblTotal == null) return;

            TimeSpan duration = dtReturns.Value.Date - _startDate.Date;
            int days = (duration.Days < 1) ? 1 : duration.Days;

            decimal rentalCost = days * _dailyRate;
            decimal lateFees = numLateFee != null ? numLateFee.Value : 0;

            // ➤ SUM UP THE LIST
            decimal damageCost = _damageReports.Sum(x => x.Fee);
            if (numDamages != null) numDamages.Value = damageCost;

            decimal grandTotal = rentalCost + damageCost + lateFees;
            lblTotal.Text = $"Days: {days} | Total: {grandTotal:C2}";
        }

        // =============================================================
        // 3. SAVING (The Loop)
        // =============================================================
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm return?", "Process", MessageBoxButtons.YesNo) == DialogResult.No) return;
            decimal.TryParse(txtOdometers?.Text, out decimal finalOdometer);
            SaveReturnTransaction(finalOdometer);
        }

        private void SaveReturnTransaction(decimal finalOdometer)
        {
            // Calculate final amounts
            TimeSpan duration = dtReturns.Value.Date - _startDate.Date;
            int days = (duration.Days < 1) ? 1 : duration.Days;
            decimal damageFee = _damageReports.Sum(x => x.Fee);
            decimal totalAmount = (days * _dailyRate) + damageFee + (numLateFee?.Value ?? 0);

            // Generate summary text
            string conditionText = txtConditions.Text;
            if (_damageReports.Count > 0)
                conditionText += " [Damage Incidents Recorded: " + _damageReports.Count + "]";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // A. Save the Main Return (Updates Rental & Vehicle Status)
                    using (MySqlCommand cmd = new MySqlCommand("sp_ProcessReturn", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_RentalId", _rentalId);
                        cmd.Parameters.AddWithValue("@p_VehicleId", _vehicleId);
                        cmd.Parameters.AddWithValue("@p_ActualReturnDate", dtReturns.Value);
                        cmd.Parameters.AddWithValue("@p_OdometerEnd", finalOdometer);
                        cmd.Parameters.AddWithValue("@p_FuelLevelEnd", cbFuels.SelectedItem?.ToString() ?? "Full");
                        cmd.Parameters.AddWithValue("@p_FinalCondition", conditionText);
                        cmd.Parameters.AddWithValue("@p_TotalAmount", totalAmount);
                        cmd.Parameters.AddWithValue("@p_Notes", "App Processed");
                        cmd.ExecuteNonQuery();
                    }

                    // B. LOOP: Save the individual Damage Reports
                    string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../Assets/Images/Damages");
                    if (!Directory.Exists(assetsPath)) Directory.CreateDirectory(assetsPath);

                    foreach (var report in _damageReports)
                    {
                        string finalImageName = "";
                        if (!string.IsNullOrEmpty(report.TempImagePath))
                        {
                            finalImageName = $"Dmg_{_rentalId}_{DateTime.Now.Ticks}.jpg";
                            File.Copy(report.TempImagePath, Path.Combine(assetsPath, finalImageName));
                        }

                        // Insert directly into the new table
                        string sql = "INSERT INTO RentalDamages (RentalId, DamageType, Description, DamageFee, EvidencePhotoPath, CreatedDate) " +
                                     "VALUES (@rId, @type, @desc, @fee, @img, NOW())";

                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@rId", _rentalId);
                            cmd.Parameters.AddWithValue("@type", report.Type);
                            cmd.Parameters.AddWithValue("@desc", report.Description);
                            cmd.Parameters.AddWithValue("@fee", report.Fee);
                            cmd.Parameters.AddWithValue("@img", finalImageName);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Return Successful!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex) { MessageBox.Show("Save Error: " + ex.Message); }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
    }
}