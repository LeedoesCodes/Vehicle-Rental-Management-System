using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class CustomerListForm : Form
    {
        private DataGridView dgvCustomers;

        public CustomerListForm()
        {
            InitializeComponent();
            SetupForm();
            LoadCustomers();
        }

        private void SetupForm()
        {
            this.Text = "Customer List";
            this.Size = new Size(900, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create DataGridView
            dgvCustomers = new DataGridView();
            dgvCustomers.Dock = DockStyle.Fill;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.ReadOnly = true;

            // Add buttons
            Button btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Size = new Size(100, 30);
            btnClose.Location = new Point(780, 420);
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(dgvCustomers);
            this.Controls.Add(btnClose);
        }

        private void LoadCustomers()
        {
            try
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
                        SELECT 
                            CustomerId,
                            CONCAT(FirstName, ' ', LastName) as 'Full Name',
                            Email,
                            Phone,
                            DateOfBirth,
                            LicenseNumber,
                            LicenseExpiry,
                            IsBlacklisted
                        FROM Customers
                        ORDER BY CreatedDate DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvCustomers.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}