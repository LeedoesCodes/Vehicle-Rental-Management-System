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
    public partial class ReturnVehicleForm : Form
    {
        private int _rentalId;
        private int _vehicleId;

        // Constructor requires ID and Name to initialize
        public ReturnVehicleForm(int rentalId, int vehicleId, string vehicleName, string customerName)
        {
            InitializeComponent();

            _rentalId = rentalId;
            _vehicleId = vehicleId;

            SetupForm(vehicleName, customerName);
        }

        private void SetupForm(string vehicleName, string customerName)
        {
            this.Text = "Process Return";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Info Labels
            CreateLabel($"Returning: {vehicleName}", 20, 20, true);
            CreateLabel($"Customer: {customerName}", 20, 45, false);

            // Input Fields
            CreateLabel("Return Date:", 20, 90, false);
            dtReturn = new DateTimePicker { Location = new Point(20, 115), Size = new Size(340, 30), Format = DateTimePickerFormat.Short, Value = DateTime.Now };

            CreateLabel("Final Odometer Reading:", 20, 160, false);
            txtOdometer = new TextBox { Location = new Point(20, 185), Size = new Size(340, 30) };

            CreateLabel("Fuel Level:", 20, 230, false);
            cbFuel = new ComboBox { Location = new Point(20, 255), Size = new Size(340, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            cbFuel.Items.AddRange(new string[] { "Empty", "1/4", "1/2", "3/4", "Full" });
            cbFuel.SelectedIndex = 4;

            CreateLabel("Final Condition / Notes:", 20, 300, false);
            txtCondition = new TextBox { Location = new Point(20, 325), Size = new Size(340, 60), Multiline = true };

            // Buttons
            btnConfirm = new Button { Text = "Complete Return", Location = new Point(190, 400), Size = new Size(170, 45), BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.None };
            btnCancel = new Button { Text = "Cancel", Location = new Point(20, 400), Size = new Size(150, 45), BackColor = Color.IndianRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel };

            btnConfirm.Click += BtnConfirm_Click;

            this.Controls.AddRange(new Control[] { dtReturn, txtOdometer, cbFuel, txtCondition, btnConfirm, btnCancel });
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOdometer.Text)) { MessageBox.Show("Please enter the final odometer reading."); return; }

            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

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
                        cmd.Parameters.AddWithValue("@p_ReturnDate", dtReturn.Value);
                        cmd.Parameters.AddWithValue("@p_OdometerEnd", Convert.ToDecimal(txtOdometer.Text));
                        cmd.Parameters.AddWithValue("@p_FuelLevelEnd", cbFuel.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@p_FinalCondition", txtCondition.Text);

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

        private void CreateLabel(string text, int x, int y, bool isBold)
        {
            Label lbl = new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", isBold ? 11 : 9, isBold ? FontStyle.Bold : FontStyle.Regular) };
            this.Controls.Add(lbl);
        }

        private DateTimePicker dtReturn;
        private TextBox txtOdometer;
        private ComboBox cbFuel;
        private TextBox txtCondition;
        private Button btnConfirm;
        private Button btnCancel;
    }
}