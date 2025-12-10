using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vehicle_Rental_Management_System.Controls // Or .Forms depending on where you put it
{
    public partial class VehiclesView : UserControl
    {
        public VehiclesView()
        {
            InitializeComponent();
            SetupUI();
            LoadVehicles();
        }

        private void SetupUI()
        {
            // 1. Create Layout
            this.Size = new Size(1000, 700);
            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill; // Fill the content panel

            // 2. Title
            Label lblTitle = new Label();
            lblTitle.Text = "Vehicle Inventory";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            // 3. Add Button
            Button btnAdd = new Button();
            btnAdd.Text = "+ Add Vehicle";
            btnAdd.BackColor = Color.FromArgb(0, 120, 215);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Size = new Size(150, 40);
            btnAdd.Location = new Point(20, 70);
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            // 4. Data Grid View (The List)
            DataGridView dgv = new DataGridView();
            dgv.Name = "dgvVehicles";
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

        // Fetch data from MySQL
        public void LoadVehicles()
        {
            string connString = "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
            if (ConfigurationManager.ConnectionStrings["MySqlConnection"] != null)
                connString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // UPDATED QUERY:
                    // We use 'LEFT JOIN' to connect Vehicles (v) with VehicleCategories (c)
                    // We select 'c.CategoryName' but nickname it 'Category' so the grid looks nice.
                    string query = @"
                SELECT 
                    v.VehicleId, 
                    v.Make, 
                    v.Model, 
                    v.Year, 
                    v.LicensePlate, 
                    c.CategoryName AS Category, 
                    v.DailyRate, 
                    v.Status 
                FROM Vehicles v
                LEFT JOIN VehicleCategories c ON v.CategoryId = c.CategoryId
                ORDER BY v.VehicleId DESC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    DataGridView dgv = this.Controls["dgvVehicles"] as DataGridView;
                    if (dgv != null)
                    {
                        dgv.DataSource = dt;

                        // Optional: Format the DailyRate column as Currency
                        if (dgv.Columns["DailyRate"] != null)
                        {
                            dgv.Columns["DailyRate"].DefaultCellStyle.Format = "C2"; // C2 = Currency with 2 decimals
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading vehicles: " + ex.Message);
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Opens the Add Form (We will build this next step)
            // Forms.AddVehicleForm addForm = new Forms.AddVehicleForm();
            // if (addForm.ShowDialog() == DialogResult.OK) 
            // {
            //     LoadVehicles(); // Refresh list after adding
            // }
            MessageBox.Show("Open Add Vehicle Form Here");
        }
    }
}