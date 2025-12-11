using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class MainForm : Form
    {
        private Button activeButton = null;
        private Color normalColor = Color.FromArgb(33, 33, 33);
        private Color hoverColor = Color.FromArgb(50, 50, 50);
        private Color activeColor = Color.FromArgb(0, 120, 215);

        public MainForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Update user info label
            if (lbluserInfo != null)
            {
                lbluserInfo.Text = $"Welcome,\n{Program.CurrentUsername}\n({Program.CurrentUserRole})";
            }

            // Set form title
            this.Text = $"Vehicle Rental System - {Program.CurrentUsername}";

            // Hide admin button for non-admin users
            if (Program.CurrentUserRole != "Admin" && btnAdmin != null)
            {
                btnAdmin.Visible = false;
            }

            // Setup button styling and events
            SetupButtonEvents();

            // Activate dashboard by default
            ActivateButton(btnDashboard);
            ShowDashboard();
        }

        private void SetupButtonEvents()
        {
            // Array of all navigation buttons
            Button[] navButtons = { btnDashboard, btnVehicles, btnCustomers,
                                   btnRentals, btnReports, btnAdmin };

            foreach (Button button in navButtons)
            {
                if (button != null)
                {
                    // Skip admin button if not visible
                    if (button == btnAdmin && !btnAdmin.Visible) continue;

                    // Set initial styling
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.BackColor = normalColor;
                    button.ForeColor = Color.White;
                    button.Font = new Font("Segoe UI", 10);
                    button.TextAlign = ContentAlignment.MiddleLeft;
                    button.Padding = new Padding(15, 0, 0, 0);

                    // Add events
                    button.Click += NavButton_Click;
                    button.MouseEnter += Button_MouseEnter;
                    button.MouseLeave += Button_MouseLeave;
                }
            }

            // Style logout button
            if (btnLogout != null)
            {
                btnLogout.FlatStyle = FlatStyle.Flat;
                btnLogout.FlatAppearance.BorderSize = 0;
                btnLogout.BackColor = Color.FromArgb(220, 53, 69); // Red
                btnLogout.ForeColor = Color.White;
                btnLogout.Font = new Font("Segoe UI", 10);
                btnLogout.TextAlign = ContentAlignment.MiddleLeft;
                btnLogout.Padding = new Padding(15, 0, 0, 0);
                btnLogout.Click += (s, e) => Logout();
                btnLogout.MouseEnter += (s, e) => btnLogout.BackColor = Color.FromArgb(200, 35, 51);
                btnLogout.MouseLeave += (s, e) => btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            }
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != activeButton && button != btnLogout)
            {
                button.BackColor = hoverColor;
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != activeButton && button != btnLogout)
            {
                button.BackColor = normalColor;
            }
        }

        private void NavButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            // Skip if already active
            if (clickedButton == activeButton) return;

            // Deactivate previous button
            if (activeButton != null)
            {
                activeButton.BackColor = normalColor;
                activeButton.Font = new Font(activeButton.Font, FontStyle.Regular);
            }

            // Activate clicked button
            ActivateButton(clickedButton);

            // Handle navigation
            HandleNavigation(clickedButton);
        }

        private void ActivateButton(Button button)
        {
            button.BackColor = activeColor;
            button.Font = new Font(button.Font, FontStyle.Bold);
            activeButton = button;
        }

        private void HandleNavigation(Button button)
        {
            switch (button.Name)
            {
                case "btnDashboard":
                    ShowDashboard();
                    break;

                case "btnVehicles":
                    ShowVehicleManagement();
                    break;

                case "btnCustomers":
                    ShowCustomerManagement();
                    break;

                case "btnRentals":
                    ShowRentalManagement();
                    break;

                case "btnReports":
                    ShowReports();
                    break;

                case "btnAdmin":
                    ShowAdminPanel();
                    break;
            }
        }

        private void ShowDashboard()
        {
            // 1. Clear current content
            contentPanel.Controls.Clear();

            // 2. Load the Dashboard UserControl
            // Ensure you have: using Vehicle_Rental_Management_System.Controls;
            var dashboardView = new Controls.DashboardView();
            dashboardView.Dock = DockStyle.Fill;

            // 3. Add to panel
            contentPanel.Controls.Add(dashboardView);
        }

        private void AddQuickStats()
        {
            Panel statsPanel = new Panel();
            statsPanel.Location = new Point(50, 150);
            statsPanel.Size = new Size(300, 200);
            statsPanel.BackColor = Color.FromArgb(240, 240, 240);
            statsPanel.BorderStyle = BorderStyle.FixedSingle;

            Label statsTitle = new Label();
            statsTitle.Text = "System Overview";
            statsTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            statsTitle.AutoSize = true;
            statsTitle.Location = new Point(20, 20);

            Label statsContent = new Label();
            statsContent.Text = "• Total Vehicles: 25\n" +
                               "• Available: 15\n" +
                               "• Rented: 10\n" +
                               "• Active Customers: 50\n" +
                               "• Pending Rentals: 5\n" +
                               "• Revenue Today: ₱5,250";
            statsContent.Font = new Font("Segoe UI", 10);
            statsContent.AutoSize = true;
            statsContent.Location = new Point(20, 50);

            statsPanel.Controls.Add(statsTitle);
            statsPanel.Controls.Add(statsContent);
            contentPanel.Controls.Add(statsPanel);
        }

        private void ShowVehicleManagement()
        {
            // Clear the content panel
            contentPanel.Controls.Clear();

            // Create an instance of VehiclesView
            var vehiclesView = new Controls.VehiclesView();
            vehiclesView.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(vehiclesView);
        }

        private void ShowCustomerManagement()
        {
            contentPanel.Controls.Clear();

            // 1. Title
            Label lblTitle = new Label();
            lblTitle.Text = "Customer Management";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 120, 215);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(50, 30);
            contentPanel.Controls.Add(lblTitle);

            // 2. Buttons
            Button btnAddCustomer = new Button();
            btnAddCustomer.Text = "➕ Add New Customer";
            btnAddCustomer.Size = new Size(180, 45);
            btnAddCustomer.Location = new Point(50, 90);
            btnAddCustomer.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAddCustomer.BackColor = Color.FromArgb(40, 167, 69);
            btnAddCustomer.ForeColor = Color.White;
            btnAddCustomer.FlatStyle = FlatStyle.Flat;
            btnAddCustomer.FlatAppearance.BorderSize = 0;
            btnAddCustomer.Click += (s, e) => OpenAddCustomerForm();
            contentPanel.Controls.Add(btnAddCustomer);

            Button btnViewCustomers = new Button();
            btnViewCustomers.Text = "👥 View All Customers";
            btnViewCustomers.Size = new Size(180, 45);
            btnViewCustomers.Location = new Point(250, 90);
            btnViewCustomers.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnViewCustomers.BackColor = Color.FromArgb(0, 123, 255);
            btnViewCustomers.ForeColor = Color.White;
            btnViewCustomers.FlatStyle = FlatStyle.Flat;
            btnViewCustomers.FlatAppearance.BorderSize = 0;
            btnViewCustomers.Click += (s, e) => OpenCustomerListForm();
            contentPanel.Controls.Add(btnViewCustomers);

            Button btnSearchCustomers = new Button();
            btnSearchCustomers.Text = "🔍 Search Customers";
            btnSearchCustomers.Size = new Size(180, 45);
            btnSearchCustomers.Location = new Point(450, 90);
            btnSearchCustomers.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSearchCustomers.BackColor = Color.FromArgb(108, 117, 125);
            btnSearchCustomers.ForeColor = Color.White;
            btnSearchCustomers.FlatStyle = FlatStyle.Flat;
            btnSearchCustomers.FlatAppearance.BorderSize = 0;
            btnSearchCustomers.Click += (s, e) => SearchCustomers();
            contentPanel.Controls.Add(btnSearchCustomers);

            // 3. Recent Customers Panel (NOW DYNAMIC)
            Panel recentPanel = new Panel();
            recentPanel.Location = new Point(50, 160);
            recentPanel.Size = new Size(700, 300);
            recentPanel.BackColor = Color.White;
            recentPanel.BorderStyle = BorderStyle.FixedSingle;

            Label recentTitle = new Label();
            recentTitle.Text = "Recently Added Customers";
            recentTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            recentTitle.Location = new Point(10, 10);
            recentTitle.AutoSize = true;
            recentPanel.Controls.Add(recentTitle);

            // Call the helper method to load real data
            LoadRecentCustomers(recentPanel);

            contentPanel.Controls.Add(recentPanel);
        }

        // NEW HELPER METHOD TO FETCH DATA
        private void LoadRecentCustomers(Panel container)
        {
            // Connection logic (matches your standard connection)
            string connString = "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
            if (System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"] != null)
                connString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("sp_GetRecentCustomers", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (MySql.Data.MySqlClient.MySqlDataAdapter adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                // Show Data in a Grid
                                DataGridView dgv = new DataGridView();
                                dgv.DataSource = dt;
                                dgv.Location = new Point(10, 40);
                                dgv.Size = new Size(680, 250);
                                dgv.ReadOnly = true;
                                dgv.AllowUserToAddRows = false;
                                dgv.RowHeadersVisible = false;
                                dgv.BorderStyle = BorderStyle.None;
                                dgv.BackgroundColor = Color.White;
                                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                                container.Controls.Add(dgv);
                            }
                            else
                            {
                                // Only show this IF database is actually empty
                                Label lblEmpty = new Label();
                                lblEmpty.Text = "No customers found in database.";
                                lblEmpty.ForeColor = Color.Gray;
                                lblEmpty.Location = new Point(10, 40);
                                lblEmpty.AutoSize = true;
                                container.Controls.Add(lblEmpty);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Label lblError = new Label();
                    lblError.Text = "Error loading data: " + ex.Message;
                    lblError.ForeColor = Color.Red;
                    lblError.Location = new Point(10, 40);
                    lblError.AutoSize = true;
                    container.Controls.Add(lblError);
                }
            }
        }

        // In MainForm.cs

        private void ShowRentalManagement()
        {
            // 1. Clear the content panel
            contentPanel.Controls.Clear();

            // 2. Create the View
            // Ensure you have: using Vehicle_Rental_Management_System.Controls;
            var rentalsView = new Controls.RentalsView();

            // 3. Dock it to fill the space
            rentalsView.Dock = DockStyle.Fill;

            // 4. Add to panel
            contentPanel.Controls.Add(rentalsView);
        }

        private void ShowReports()
        {
            contentPanel.Controls.Clear();

            Label lblTitle = new Label();
            lblTitle.Text = "Reports";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(50, 30);

            contentPanel.Controls.Add(lblTitle);
        }

        private void ShowAdminPanel()
        {
            contentPanel.Controls.Clear();

            Label lblTitle = new Label();
            lblTitle.Text = "Admin Panel";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(50, 30);

            contentPanel.Controls.Add(lblTitle);
        }

        // ============ CUSTOMER MANAGEMENT METHODS ============

        private void OpenAddCustomerForm()
        {
            try
            {
                CustomerAddForm customerForm = new CustomerAddForm();
                customerForm.StartPosition = FormStartPosition.CenterParent;

                DialogResult result = customerForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    MessageBox.Show("Customer added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh customer data if needed
                    // RefreshCustomerData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening customer form: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenCustomerListForm()
        {
            try
            {
                CustomerListForm listForm = new CustomerListForm();
                listForm.StartPosition = FormStartPosition.CenterParent;
                listForm.Show();
                listForm.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening customer list: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchCustomers()
        {
            // Create a simple search dialog
            using (Form searchForm = new Form())
            {
                searchForm.Text = "Search Customers";
                searchForm.Size = new Size(400, 200);
                searchForm.StartPosition = FormStartPosition.CenterParent;
                searchForm.FormBorderStyle = FormBorderStyle.FixedDialog;

                Label lblSearch = new Label();
                lblSearch.Text = "Enter search term:";
                lblSearch.Location = new Point(20, 30);
                lblSearch.Size = new Size(150, 25);

                TextBox txtSearch = new TextBox();
                txtSearch.Location = new Point(180, 30);
                txtSearch.Size = new Size(180, 25);
                txtSearch.Focus();

                Button btnSearch = new Button();
                btnSearch.Text = "Search";
                btnSearch.Location = new Point(120, 80);
                btnSearch.Size = new Size(100, 30);
                btnSearch.DialogResult = DialogResult.OK;

                Button btnCancel = new Button();
                btnCancel.Text = "Cancel";
                btnCancel.Location = new Point(230, 80);
                btnCancel.Size = new Size(100, 30);
                btnCancel.DialogResult = DialogResult.Cancel;

                searchForm.Controls.AddRange(new Control[] { lblSearch, txtSearch, btnSearch, btnCancel });

                if (searchForm.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    // Open customer list with search filter
                    OpenCustomerListFormWithSearch(txtSearch.Text);
                }
            }
        }

        private void OpenCustomerListFormWithSearch(string searchTerm)
        {
            try
            {
                CustomerListForm listForm = new CustomerListForm();
                listForm.StartPosition = FormStartPosition.CenterParent;
                listForm.Show();
                listForm.BringToFront();

                // Note: You'll need to modify CustomerListForm to accept search terms
                // For now, show a message
                MessageBox.Show($"Searching for customers with: {searchTerm}", "Search",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============ VEHICLE MANAGEMENT METHODS ============

        private void OpenVehicleManagement()
        {
            VehicleManagementForm vehicleForm = new VehicleManagementForm();
            vehicleForm.ShowDialog();
        }

        private void OpenAddVehicleForm()
        {
            MessageBox.Show("Open Add Vehicle Form");
        }

        private void OpenVehicleSearch()
        {
            MessageBox.Show("Open Vehicle Search");
        }

        // ============ UTILITY METHODS ============

        private void Logout()
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // ============ EVENT HANDLERS ============

        private void navButtonsPanel_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Add custom painting if needed
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            // Handled by NavButton_Click
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Rename this button to something meaningful
        }

        private void lbluserInfo_Click(object sender, EventArgs e)
        {
            // Optional: Add click behavior for user info
        }

        private void btnVehicles_Click(object sender, EventArgs e)
        {
            // Handled by NavButton_Click
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            // Handled by NavButton_Click
        }

        private void btnRentals_Click(object sender, EventArgs e)
        {
            // Handled by NavButton_Click
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            // Handled by NavButton_Click
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            // Handled by NavButton_Click
        }
    }
}