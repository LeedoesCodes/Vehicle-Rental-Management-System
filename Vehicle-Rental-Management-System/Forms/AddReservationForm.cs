using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class AddReservationForm : Form
    {
        // UI Controls
        private ComboBox cbCustomer;
        private ComboBox cbVehicle;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private Label lblTotal;
        private TextBox txtNotes;
        private Button btnSave;
        private Button btnCancel;

        // Data Variables
        private decimal currentDailyRate = 0;

        public AddReservationForm()
        {
            // 1. Setup the Window and Controls manually
            SetupUI();

            // 2. Load Data from Database
            LoadData();
        }

        private void SetupUI()
        {
            this.Text = "New Reservation";
            this.Size = new Size(450, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Helper to create labels
            Label lblCust = new Label { Text = "Select Customer:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            this.Controls.Add(lblCust);

            cbCustomer = new ComboBox { Location = new Point(20, 45), Size = new Size(390, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(cbCustomer);

            Label lblVeh = new Label { Text = "Select Vehicle:", Location = new Point(20, 80), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            this.Controls.Add(lblVeh);

            cbVehicle = new ComboBox { Location = new Point(20, 105), Size = new Size(390, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            // Event to update price when car changes
            cbVehicle.SelectedIndexChanged += CbVehicle_SelectedIndexChanged;
            this.Controls.Add(cbVehicle);

            Label lblStart = new Label { Text = "Start Date:", Location = new Point(20, 150), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            this.Controls.Add(lblStart);

            dtpStart = new DateTimePicker { Location = new Point(20, 175), Size = new Size(180, 30), Format = DateTimePickerFormat.Short, Value = DateTime.Now };
            dtpStart.ValueChanged += RecalculateTotal;
            this.Controls.Add(dtpStart);

            Label lblEnd = new Label { Text = "End Date:", Location = new Point(230, 150), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            this.Controls.Add(lblEnd);

            dtpEnd = new DateTimePicker { Location = new Point(230, 175), Size = new Size(180, 30), Format = DateTimePickerFormat.Short, Value = DateTime.Now.AddDays(1) };
            dtpEnd.ValueChanged += RecalculateTotal;
            this.Controls.Add(dtpEnd);

            Label lblNote = new Label { Text = "Notes:", Location = new Point(20, 220), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            this.Controls.Add(lblNote);

            txtNotes = new TextBox { Location = new Point(20, 245), Size = new Size(390, 80), Multiline = true };
            this.Controls.Add(txtNotes);

            // Total Price Label
            lblTotal = new Label
            {
                Text = "Total: ₱0.00",
                Location = new Point(20, 340),
                AutoSize = false,
                Size = new Size(390, 40),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 120, 215)
            };
            this.Controls.Add(lblTotal);

            // Buttons
            btnSave = new Button
            {
                Text = "Save Reservation",
                Location = new Point(230, 400),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(40, 400),
                Size = new Size(120, 45),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            this.Controls.Add(btnCancel);
        }

        private void LoadData()
        {
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // 1. Load Customers
                    string custQuery = "SELECT CustomerId, CONCAT(FirstName, ' ', LastName) as FullName FROM Customers ORDER BY LastName";
                    MySqlDataAdapter daCust = new MySqlDataAdapter(custQuery, conn);
                    DataTable dtCust = new DataTable();
                    daCust.Fill(dtCust);

                    cbCustomer.DisplayMember = "FullName";
                    cbCustomer.ValueMember = "CustomerId";
                    cbCustomer.DataSource = dtCust;
                    cbCustomer.SelectedIndex = -1;

                    // 2. Load Available Vehicles (With Rate)
                    string vehQuery = "SELECT VehicleId, CONCAT(Make, ' ', Model, ' - ', LicensePlate) as DisplayName, DailyRate FROM Vehicles WHERE Status = 'Available'";
                    MySqlDataAdapter daVeh = new MySqlDataAdapter(vehQuery, conn);
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
                currentDailyRate = Convert.ToDecimal(row["DailyRate"]);
                RecalculateTotal(null, null);
            }
        }

        private void RecalculateTotal(object sender, EventArgs e)
        {
            if (currentDailyRate > 0 && dtpEnd.Value > dtpStart.Value)
            {
                TimeSpan duration = dtpEnd.Value.Date - dtpStart.Value.Date;
                int days = (int)duration.TotalDays;
                if (days < 1) days = 1;

                decimal total = days * currentDailyRate;
                lblTotal.Text = $"Total: ₱{total:N2} ({days} Days)";
                lblTotal.Tag = total; // Store raw value
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
            if (cbCustomer.SelectedValue == null) { MessageBox.Show("Select a customer."); return; }
            if (cbVehicle.SelectedValue == null) { MessageBox.Show("Select a vehicle."); return; }
            if (dtpEnd.Value <= dtpStart.Value) { MessageBox.Show("End date must be after start date."); return; }

            decimal finalAmount = Convert.ToDecimal(lblTotal.Tag);

            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_AddReservation", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_CustomerId", cbCustomer.SelectedValue);
                        cmd.Parameters.AddWithValue("@p_VehicleId", cbVehicle.SelectedValue);
                        cmd.Parameters.AddWithValue("@p_StartDate", dtpStart.Value);
                        cmd.Parameters.AddWithValue("@p_EndDate", dtpEnd.Value);
                        cmd.Parameters.AddWithValue("@p_TotalAmount", finalAmount);
                        cmd.Parameters.AddWithValue("@p_Notes", txtNotes.Text);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Reservation saved successfully!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving: " + ex.Message);
                }
            }
        }
    }
}