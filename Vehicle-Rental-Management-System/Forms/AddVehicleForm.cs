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
    public partial class AddVehicleForm : Form
    {
        public AddVehicleForm()
        {
            InitializeComponent();
            SetupUI();
            LoadCategories();
        }

        private void SetupUI()
        {
            this.Text = "Add New Vehicle";
            this.Size = new Size(600, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // --- LEFT COLUMN ---
            CreateLabel("Make:", 20, 20);
            txtMake = CreateTextBox(20, 45);

            CreateLabel("Model:", 20, 80);
            txtModel = CreateTextBox(20, 105);

            CreateLabel("Year:", 20, 140);
            numYear = CreateNumeric(20, 165, 1990, 2100);
            numYear.Value = DateTime.Now.Year;

            CreateLabel("Color:", 20, 200);
            txtColor = CreateTextBox(20, 225);

            CreateLabel("License Plate:", 20, 260);
            txtPlate = CreateTextBox(20, 285);

            CreateLabel("Category:", 20, 320);
            cbCategory = CreateComboBox(20, 345);

            // --- RIGHT COLUMN ---
            CreateLabel("Daily Rate (PHP):", 300, 20);
            numRate = CreateNumeric(300, 45, 0, 100000);
            numRate.DecimalPlaces = 2;

            CreateLabel("Transmission:", 300, 80);
            cbTransmission = CreateComboBox(300, 105);
            cbTransmission.Items.AddRange(new string[] { "Automatic", "Manual" });
            cbTransmission.SelectedIndex = 0;

            CreateLabel("Fuel Type:", 300, 140);
            cbFuel = CreateComboBox(300, 165);
            cbFuel.Items.AddRange(new string[] { "Gasoline", "Diesel", "Electric", "Hybrid" });
            cbFuel.SelectedIndex = 0;

            CreateLabel("Seats:", 300, 200);
            numSeats = CreateNumeric(300, 225, 2, 50);
            numSeats.Value = 5;

            CreateLabel("Current Mileage:", 300, 260);
            numMileage = CreateNumeric(300, 285, 0, 1000000);
            numMileage.DecimalPlaces = 0;

            CreateLabel("VIN (Chassis #):", 300, 320);
            txtVIN = CreateTextBox(300, 345);

            // --- BUTTONS ---
            btnSave = new Button { Text = "Save Vehicle", Location = new Point(320, 420), Size = new Size(200, 50), BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.None };
            btnCancel = new Button { Text = "Cancel", Location = new Point(100, 420), Size = new Size(150, 50), BackColor = Color.IndianRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel };

            btnSave.Click += BtnSave_Click;

            this.Controls.AddRange(new Control[] { btnSave, btnCancel });
        }

        private void LoadCategories()
        {
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
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
                    cbCategory.SelectedIndex = -1; // Start empty
                }
                catch (Exception ex) { MessageBox.Show("Error loading categories: " + ex.Message); }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtMake.Text) || string.IsNullOrWhiteSpace(txtModel.Text) || string.IsNullOrWhiteSpace(txtPlate.Text))
            {
                MessageBox.Show("Please fill in Make, Model, and License Plate.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbCategory.SelectedValue == null)
            {
                MessageBox.Show("Please select a Category.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_AddVehicle", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@p_Make", txtMake.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_Model", txtModel.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_Year", numYear.Value);
                        cmd.Parameters.AddWithValue("@p_Color", txtColor.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_LicensePlate", txtPlate.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@p_VIN", txtVIN.Text.Trim());
                        cmd.Parameters.AddWithValue("@p_CategoryId", cbCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@p_Transmission", cbTransmission.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@p_FuelType", cbFuel.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@p_SeatingCapacity", numSeats.Value);
                        cmd.Parameters.AddWithValue("@p_CurrentMileage", numMileage.Value);
                        cmd.Parameters.AddWithValue("@p_DailyRate", numRate.Value);
                        cmd.Parameters.AddWithValue("@p_Status", "Available"); // Default status

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Vehicle added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- UI HELPERS ---
        private TextBox txtMake, txtModel, txtColor, txtPlate, txtVIN;
        private NumericUpDown numYear, numRate, numSeats, numMileage;
        private ComboBox cbCategory, cbTransmission, cbFuel;
        private Button btnSave, btnCancel;

        private void CreateLabel(string text, int x, int y)
        {
            this.Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
        }
        private TextBox CreateTextBox(int x, int y)
        {
            TextBox t = new TextBox { Location = new Point(x, y), Size = new Size(240, 30) };
            this.Controls.Add(t);
            return t;
        }
        private NumericUpDown CreateNumeric(int x, int y, int min, int max)
        {
            NumericUpDown n = new NumericUpDown { Location = new Point(x, y), Size = new Size(240, 30), Minimum = min, Maximum = max };
            this.Controls.Add(n);
            return n;
        }
        private ComboBox CreateComboBox(int x, int y)
        {
            ComboBox c = new ComboBox { Location = new Point(x, y), Size = new Size(240, 30), DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(c);
            return c;
        }
    }
}