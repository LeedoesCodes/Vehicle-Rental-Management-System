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

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class RentalsView : UserControl
    {
        public RentalsView()
        {
            InitializeComponent();
            SetupUI();
            LoadRentals();
        }

        private void SetupUI()
        {
            this.Size = new Size(1000, 700);
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            // 1. Title
            Label lblTitle = new Label();
            lblTitle.Text = "Rental Transactions";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            // 2. New Rental Button
            Button btnNewRental = new Button();
            btnNewRental.Text = "➕ New Rental";
            btnNewRental.BackColor = Color.FromArgb(40, 167, 69); // Green
            btnNewRental.ForeColor = Color.White;
            btnNewRental.FlatStyle = FlatStyle.Flat;
            btnNewRental.FlatAppearance.BorderSize = 0;
            btnNewRental.Size = new Size(150, 40);
            btnNewRental.Location = new Point(20, 70);
            btnNewRental.Click += BtnNewRental_Click;
            this.Controls.Add(btnNewRental);

            // 3. Return Vehicle Button
            Button btnReturn = new Button();
            btnReturn.Text = "↩ Return Vehicle";
            btnReturn.BackColor = Color.FromArgb(255, 193, 7); // Yellow/Orange
            btnReturn.ForeColor = Color.Black;
            btnReturn.FlatStyle = FlatStyle.Flat;
            btnReturn.FlatAppearance.BorderSize = 0;
            btnReturn.Size = new Size(150, 40);
            btnReturn.Location = new Point(180, 70);
            btnReturn.Click += BtnReturn_Click;
            this.Controls.Add(btnReturn);

            // 4. Grid
            DataGridView dgv = new DataGridView();
            dgv.Name = "dgvRentals";
            dgv.Location = new Point(20, 130);
            dgv.Size = new Size(900, 500);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.WhiteSmoke;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;
            this.Controls.Add(dgv);
        }

        public void LoadRentals()
        {
            string connString = "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
            if (ConfigurationManager.ConnectionStrings["MySqlConnection"] != null)
                connString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Use the stored procedure we just created
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetAllRentals", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridView dgv = this.Controls["dgvRentals"] as DataGridView;
                            if (dgv != null)
                            {
                                dgv.DataSource = dt;
                                // Hide the ID columns
                                if (dgv.Columns["VehicleId"] != null) dgv.Columns["VehicleId"].Visible = false;
                                // Format currency column if it exists
                                if (dgv.Columns["TotalAmount"] != null)
                                    dgv.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading rentals: " + ex.Message);
                }
            }
        }

        private void BtnNewRental_Click(object sender, EventArgs e)
        {
            // Create and show the form
            Forms.NewRentalForm newRental = new Forms.NewRentalForm();

            // If they clicked "Confirm Rental", refresh the list
            if (newRental.ShowDialog() == DialogResult.OK)
            {
                LoadRentals(); // Reload grid to see the new rental
            }
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            DataGridView dgv = this.Controls["dgvRentals"] as DataGridView;

            // 1. Check if a row is selected
            if (dgv != null && dgv.SelectedRows.Count > 0)
            {
                // 2. Check if already returned
                string status = dgv.SelectedRows[0].Cells["Status"].Value.ToString();
                if (status == "Returned")
                {
                    MessageBox.Show("This vehicle has already been returned.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 3. Get Data from Grid
                int rentalId = Convert.ToInt32(dgv.SelectedRows[0].Cells["RentalId"].Value);
                // Note: Ensure your grid actually has VehicleId hidden or visible. 
                // If sp_GetAllRentals includes it, it will be in the datasource even if not visible.
                int vehicleId = Convert.ToInt32(dgv.SelectedRows[0].Cells["VehicleId"].Value);
                string vehicleName = dgv.SelectedRows[0].Cells["VehicleName"].Value.ToString();
                string customerName = dgv.SelectedRows[0].Cells["CustomerName"].Value.ToString();

                // 4. Open Return Form
                Forms.ReturnVehicleForm returnForm = new Forms.ReturnVehicleForm(rentalId, vehicleId, vehicleName, customerName);

                if (returnForm.ShowDialog() == DialogResult.OK)
                {
                    // 5. Refresh Grid to show updated status
                    LoadRentals();
                }
            }
            else
            {
                MessageBox.Show("Please select a rental transaction to return.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}