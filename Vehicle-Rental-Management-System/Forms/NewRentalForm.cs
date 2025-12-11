using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class NewRentalForm : Form
    {
        // To store the selected vehicle's daily rate for calculation
        private decimal _currentDailyRate = 0;

        public NewRentalForm()
        {
            InitializeComponent();
            SetupForm();
            LoadData();
        }

        private void SetupForm()
        {
            this.Text = "Process New Rental";
            this.Size = new Size(500, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // --- UI CREATION (If you don't use the Designer) ---

            // Labels
            CreateLabel("Select Customer:", 20, 20);
            CreateLabel("Select Vehicle:", 20, 80);
            CreateLabel("Pickup Date:", 20, 140);
            CreateLabel("Return Date:", 240, 140);
            CreateLabel("Current Odometer:", 20, 200);
            CreateLabel("Fuel Level:", 240, 200);
            CreateLabel("Notes / Condition:", 20, 260);

            // Controls
            cbCustomer = new ComboBox { Location = new Point(20, 45), Size = new Size(440, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            cbVehicle = new ComboBox { Location = new Point(20, 105), Size = new Size(440, 30), DropDownStyle = ComboBoxStyle.DropDownList };

            dtPickup = new DateTimePicker { Location = new Point(20, 165), Size = new Size(200, 30), Format = DateTimePickerFormat.Short, Value = DateTime.Now };
            dtReturn = new DateTimePicker { Location = new Point(240, 165), Size = new Size(220, 30), Format = DateTimePickerFormat.Short, Value = DateTime.Now.AddDays(1) };

            txtOdometer = new TextBox { Location = new Point(20, 225), Size = new Size(200, 30) };
            cbFuel = new ComboBox { Location = new Point(240, 225), Size = new Size(220, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            cbFuel.Items.AddRange(new string[] { "Empty", "1/4", "1/2", "3/4", "Full" });
            cbFuel.SelectedIndex = 4; // Default to Full

            txtNotes = new TextBox { Location = new Point(20, 285), Size = new Size(440, 80), Multiline = true };

            // Total Price Label (Big and Bold)
            lblTotal = new Label { Location = new Point(20, 380), Size = new Size(440, 40), Text = "Total: ₱0.00", Font = new Font("Segoe UI", 16, FontStyle.Bold), TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.FromArgb(0, 120, 215) };

            // Buttons
            btnSave = new Button { Text = "Confirm Rental", Location = new Point(280, 440), Size = new Size(180, 45), BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.None };
            btnCancel = new Button { Text = "Cancel", Location = new Point(140, 440), Size = new Size(120, 45), BackColor = Color.IndianRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel };

            // Add Events
            cbVehicle.SelectedIndexChanged += CbVehicle_SelectedIndexChanged;
            dtPickup.ValueChanged += UpdateTotalCalculation;
            dtReturn.ValueChanged += UpdateTotalCalculation;
            btnSave.Click += BtnSave_Click;

            // Add to form
            this.Controls.AddRange(new Control[] { cbCustomer, cbVehicle, dtPickup, dtReturn, txtOdometer, cbFuel, txtNotes, lblTotal, btnSave, btnCancel });
        }

        // --- DATA LOADING ---

        private void LoadData()
        {
            string connString = GetConnectionString();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // 1. Load Customers
                    MySqlDataAdapter daCust = new MySqlDataAdapter("SELECT CustomerId, CONCAT(FirstName, ' ', LastName) as FullName FROM Customers ORDER BY LastName", conn);
                    DataTable dtCust = new DataTable();
                    daCust.Fill(dtCust);

                    cbCustomer.DataSource = dtCust;
                    cbCustomer.DisplayMember = "FullName";
                    cbCustomer.ValueMember = "CustomerId";
                    cbCustomer.SelectedIndex = -1;

                    // 2. Load AVAILABLE Vehicles
                    // Only show vehicles with Status = 'Available'
                    MySqlDataAdapter daVeh = new MySqlDataAdapter("SELECT VehicleId, CONCAT(Make, ' ', Model, ' - ', LicensePlate) as DisplayName, DailyRate, CurrentMileage FROM Vehicles WHERE Status = 'Available'", conn);
                    DataTable dtVeh = new DataTable();
                    daVeh.Fill(dtVeh);

                    cbVehicle.DataSource = dtVeh;
                    cbVehicle.DisplayMember = "DisplayName";
                    cbVehicle.ValueMember = "VehicleId";
                    cbVehicle.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        // --- LOGIC ---

        private void CbVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVehicle.SelectedIndex != -1 && cbVehicle.SelectedItem is DataRowView row)
            {
                // 1. Get Daily Rate
                _currentDailyRate = Convert.ToDecimal(row["DailyRate"]);

                // 2. Auto-fill Odometer from Vehicle record
                txtOdometer.Text = row["CurrentMileage"].ToString();

                UpdateTotalCalculation(sender, e);
            }
        }

        private void UpdateTotalCalculation(object sender, EventArgs e)
        {
            if (_currentDailyRate > 0 && dtReturn.Value > dtPickup.Value)
            {
                TimeSpan duration = dtReturn.Value.Date - dtPickup.Value.Date;
                int days = (int)duration.TotalDays;
                if (days < 1) days = 1; // Minimum 1 day rental

                decimal total = days * _currentDailyRate;
                lblTotal.Text = $"Total: ₱{total:N2} ({days} Days)";
                lblTotal.Tag = total; // Store the actual value for saving
            }
            else
            {
                lblTotal.Text = "Total: ₱0.00";
                lblTotal.Tag = 0m;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (cbCustomer.SelectedValue == null) { MessageBox.Show("Please select a customer."); return; }
            if (cbVehicle.SelectedValue == null) { MessageBox.Show("Please select a vehicle."); return; }
            if (string.IsNullOrEmpty(txtOdometer.Text)) { MessageBox.Show("Please enter odometer reading."); return; }

            decimal totalAmount = Convert.ToDecimal(lblTotal.Tag);

            // Save to DB
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
                        cmd.Parameters.AddWithValue("@p_FuelLevelStart", cbFuel.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@p_Notes", txtNotes.Text);
                        cmd.Parameters.AddWithValue("@p_UserId", Program.CurrentUserId); // Logged in user

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

        // Helpers
        private void CreateLabel(string text, int x, int y)
        {
            Label lbl = new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            this.Controls.Add(lbl);
        }

        private string GetConnectionString()
        {
            if (ConfigurationManager.ConnectionStrings["MySqlConnection"] != null)
                return ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            return "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
        }

        // Variables
        private ComboBox cbCustomer;
        private ComboBox cbVehicle;
        private DateTimePicker dtPickup;
        private DateTimePicker dtReturn;
        private TextBox txtOdometer;
        private ComboBox cbFuel;
        private TextBox txtNotes;
        private Label lblTotal;
        private Button btnSave;
        private Button btnCancel;
    }
}