using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class CustomersView : UserControl
    {
        public CustomersView()
        {
            InitializeComponent();
            SetupUI();
            LoadCustomers();
        }

        private void SetupUI()
        {
            this.Size = new Size(1000, 700);
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            // Title
            Label lblTitle = new Label { Text = "Customer Management", Font = new Font("Segoe UI", 20, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            this.Controls.Add(lblTitle);

            // Add Button
            Button btnAdd = new Button { Text = "+ Add Customer", BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(150, 40), Location = new Point(20, 70) };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            // Delete Button
            Button btnDelete = new Button { Text = "Delete Selected", BackColor = Color.IndianRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(150, 40), Location = new Point(180, 70) };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            // Grid
            DataGridView dgv = new DataGridView { Name = "dgvCustomers", Location = new Point(20, 130), Size = new Size(900, 500), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.WhiteSmoke;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;
            this.Controls.Add(dgv);
        }

        public void LoadCustomers()
        {
            string connString = GetConnectionString();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetAllCustomers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridView dgv = this.Controls["dgvCustomers"] as DataGridView;
                            if (dgv != null) dgv.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Placeholder: Open your AddCustomerForm here
            MessageBox.Show("TODO: Open AddCustomerForm here.\nUse sp_AddCustomer to save.");
            // After closing form: LoadCustomers(); 
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DataGridView dgv = this.Controls["dgvCustomers"] as DataGridView;
            if (dgv.SelectedRows.Count == 0) { MessageBox.Show("Please select a customer to delete."); return; }

            int customerId = Convert.ToInt32(dgv.SelectedRows[0].Cells["CustomerId"].Value);
            string name = dgv.SelectedRows[0].Cells["FirstName"].Value.ToString();

            if (MessageBox.Show($"Are you sure you want to delete {name}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DeleteCustomer(customerId);
                LoadCustomers(); // Refresh list
            }
        }

        private void DeleteCustomer(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_DeleteCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_CustomerId", id);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Customer deleted successfully.");
                }
                catch (Exception ex) { MessageBox.Show("Error deleting: " + ex.Message); }
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