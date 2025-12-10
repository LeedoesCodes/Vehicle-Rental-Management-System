using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class CustomerAddForm : Form
    {
        public CustomerAddForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Form properties
            this.Text = "Add New Customer";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);

            CreateControls();
        }

        private void CreateControls()
        {
            int yPos = 20;
            int labelWidth = 120;
            int controlWidth = 300;
            int controlHeight = 25;
            int spacing = 35;

            // Title Label
            Label lblTitle = new Label();
            lblTitle.Text = "Add New Customer";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 120, 215);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, yPos);
            this.Controls.Add(lblTitle);

            yPos += 50;

            // First Name
            AddLabel("First Name:", 20, yPos, labelWidth);
            txtFirstName = new TextBox();
            SetupTextBox(txtFirstName, 140, yPos, controlWidth, controlHeight);
            this.Controls.Add(txtFirstName);

            yPos += spacing;

            // Last Name
            AddLabel("Last Name:", 20, yPos, labelWidth);
            txtLastName = new TextBox();
            SetupTextBox(txtLastName, 140, yPos, controlWidth, controlHeight);
            this.Controls.Add(txtLastName);

            yPos += spacing;

            // Email
            AddLabel("Email:", 20, yPos, labelWidth);
            txtEmail = new TextBox();
            SetupTextBox(txtEmail, 140, yPos, controlWidth, controlHeight);
            this.Controls.Add(txtEmail);

            yPos += spacing;

            // Phone
            AddLabel("Phone:", 20, yPos, labelWidth);
            txtPhone = new TextBox();
            SetupTextBox(txtPhone, 140, yPos, controlWidth, controlHeight);
            this.Controls.Add(txtPhone);

            yPos += spacing;

            // Address
            AddLabel("Address:", 20, yPos, labelWidth);
            txtAddress = new TextBox();
            SetupTextBox(txtAddress, 140, yPos, controlWidth, controlHeight);
            this.Controls.Add(txtAddress);

            yPos += spacing;

            // Date of Birth
            AddLabel("Date of Birth:", 20, yPos, labelWidth);
            dtpDateOfBirth = new DateTimePicker();
            dtpDateOfBirth.Location = new Point(140, yPos);
            dtpDateOfBirth.Size = new Size(controlWidth, controlHeight);
            dtpDateOfBirth.Format = DateTimePickerFormat.Short;
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = DateTime.Now.AddYears(-25);
            this.Controls.Add(dtpDateOfBirth);

            yPos += spacing;

            // License Number
            AddLabel("License Number:", 20, yPos, labelWidth);
            txtLicenseNumber = new TextBox();
            SetupTextBox(txtLicenseNumber, 140, yPos, controlWidth, controlHeight);
            this.Controls.Add(txtLicenseNumber);

            yPos += spacing;

            // License Expiry
            AddLabel("License Expiry:", 20, yPos, labelWidth);
            dtpLicenseExpiry = new DateTimePicker();
            dtpLicenseExpiry.Location = new Point(140, yPos);
            dtpLicenseExpiry.Size = new Size(controlWidth, controlHeight);
            dtpLicenseExpiry.Format = DateTimePickerFormat.Short;
            dtpLicenseExpiry.MinDate = DateTime.Now;
            dtpLicenseExpiry.Value = DateTime.Now.AddYears(3);
            this.Controls.Add(dtpLicenseExpiry);

            yPos += spacing;

            // License State
            AddLabel("License State:", 20, yPos, labelWidth);
            txtLicenseState = new TextBox();
            SetupTextBox(txtLicenseState, 140, yPos, controlWidth, controlHeight);
            this.Controls.Add(txtLicenseState);

            yPos += 50;

            // Buttons Panel
            Panel buttonPanel = new Panel();
            buttonPanel.Location = new Point(20, yPos);
            buttonPanel.Size = new Size(440, 50);
            buttonPanel.BackColor = Color.Transparent;

            // Save Button
            btnSave = new Button();
            btnSave.Text = "Save Customer";
            btnSave.Size = new Size(150, 40);
            btnSave.Location = new Point(50, 5);
            btnSave.BackColor = Color.FromArgb(40, 167, 69); // Green
            btnSave.ForeColor = Color.White;
            btnSave.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            buttonPanel.Controls.Add(btnSave);

            // Cancel Button
            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Size = new Size(150, 40);
            btnCancel.Location = new Point(240, 5);
            btnCancel.BackColor = Color.FromArgb(108, 117, 125); // Gray
            btnCancel.ForeColor = Color.White;
            btnCancel.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            buttonPanel.Controls.Add(btnCancel);

            this.Controls.Add(buttonPanel);
        }

        private void AddLabel(string text, int x, int y, int width)
        {
            Label label = new Label();
            label.Text = text;
            label.Font = new Font("Segoe UI", 10);
            label.Location = new Point(x, y);
            label.Size = new Size(width, 25);
            label.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(label);
        }

        private void SetupTextBox(TextBox textBox, int x, int y, int width, int height)
        {
            textBox.Location = new Point(x, y);
            textBox.Size = new Size(width, height);
            textBox.Font = new Font("Segoe UI", 10);
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        // Control declarations
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private DateTimePicker dtpDateOfBirth;
        private TextBox txtLicenseNumber;
        private DateTimePicker dtpLicenseExpiry;
        private TextBox txtLicenseState;
        private Button btnSave;
        private Button btnCancel;

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    // Create customer object
                    Models.Customer customer = new Models.Customer
                    {
                        FirstName = txtFirstName.Text.Trim(),
                        LastName = txtLastName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Address = txtAddress.Text.Trim(),
                        DateOfBirth = dtpDateOfBirth.Value,
                        LicenseNumber = txtLicenseNumber.Text.Trim(),
                        LicenseExpiry = dtpLicenseExpiry.Value,
                        LicenseState = txtLicenseState.Text.Trim(),
                        IsBlacklisted = false,
                        CreatedDate = DateTime.Now
                    };

                    // Save to database
                    if (SaveCustomerToDatabase(customer))
                    {
                        MessageBox.Show("Customer added successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving customer: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            // Check required fields
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Please enter First Name", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Please enter Last Name", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter Email", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter Phone Number", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // Validate email format
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                txtEmail.SelectAll();
                return false;
            }

            // Check age (must be at least 18)
            int age = DateTime.Now.Year - dtpDateOfBirth.Value.Year;
            if (dtpDateOfBirth.Value.Date > DateTime.Now.AddYears(-age)) age--;

            if (age < 18)
            {
                MessageBox.Show("Customer must be at least 18 years old", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDateOfBirth.Focus();
                return false;
            }

            // Check license expiry
            if (dtpLicenseExpiry.Value <= DateTime.Now)
            {
                MessageBox.Show("License must not be expired", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpLicenseExpiry.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool SaveCustomerToDatabase(Models.Customer customer)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    INSERT INTO Customers 
                    (FirstName, LastName, Email, Phone, Address, DateOfBirth, 
                     LicenseNumber, LicenseExpiry, LicenseState, IsBlacklisted, CreatedDate)
                    VALUES 
                    (@FirstName, @LastName, @Email, @Phone, @Address, @DateOfBirth,
                     @LicenseNumber, @LicenseExpiry, @LicenseState, @IsBlacklisted, @CreatedDate)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                    cmd.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                    cmd.Parameters.AddWithValue("@LicenseNumber", customer.LicenseNumber);
                    cmd.Parameters.AddWithValue("@LicenseExpiry", customer.LicenseExpiry);
                    cmd.Parameters.AddWithValue("@LicenseState", customer.LicenseState);
                    cmd.Parameters.AddWithValue("@IsBlacklisted", customer.IsBlacklisted);
                    cmd.Parameters.AddWithValue("@CreatedDate", customer.CreatedDate);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}