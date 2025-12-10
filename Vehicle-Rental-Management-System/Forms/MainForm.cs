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
            // Update user info label (if you have one named lblUserInfo)
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

            // Style logout button differently (if you have one)
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
            // Clear content panel
            contentPanel.Controls.Clear();

            // Create welcome label
            Label lblWelcome = new Label();
            lblWelcome.Text = $"Welcome to Vehicle Rental Management System!\n\n" +
                             $"Logged in as: {Program.CurrentUsername}\n" +
                             $"Role: {Program.CurrentUserRole}";
            lblWelcome.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(50, 50);

            // Add to content panel
            contentPanel.Controls.Add(lblWelcome);

            // Add quick stats (optional)
            AddQuickStats();
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
            // 1. Clear the content panel (remove whatever was there before)
            contentPanel.Controls.Clear();

            // 2. Create an instance of your new UserControl
            // Make sure to add 'using Vehicle_Rental_Management_System.Controls;' at the top
            var vehiclesView = new Controls.VehiclesView();

            // 3. Make it fill the space
            vehiclesView.Dock = DockStyle.Fill;

            // 4. Add it to the panel
            contentPanel.Controls.Add(vehiclesView);
        }

        private void ShowCustomerManagement()
        {
            // 1. Clear old content
            contentPanel.Controls.Clear();

            // 2. Load the Customer View
            var customerView = new Controls.CustomersView();
            customerView.Dock = DockStyle.Fill;

            // 3. Add to panel
            contentPanel.Controls.Add(customerView);
        }

        private void ShowRentalManagement()
        {
            contentPanel.Controls.Clear();

            Label lblTitle = new Label();
            lblTitle.Text = "Rental Management";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(50, 30);

            contentPanel.Controls.Add(lblTitle);
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

        // Navigation methods that open other forms
        private void OpenVehicleManagement()
        {
            VehicleManagementForm vehicleForm = new VehicleManagementForm();
            vehicleForm.ShowDialog();
        }

        private void OpenAddVehicleForm()
        {
            // Create or open Add Vehicle form
            MessageBox.Show("Open Add Vehicle Form");
        }

        private void OpenVehicleSearch()
        {
            MessageBox.Show("Open Vehicle Search");
        }

        private void Logout()
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // Keep your existing event handlers (they can be empty or updated)
        private void navButtonsPanel_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Add custom painting if needed
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            // This will be handled by NavButton_Click, but you can keep it
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
            // This will be handled by NavButton_Click
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            // This will be handled by NavButton_Click
        }

        private void btnRentals_Click(object sender, EventArgs e)
        {
            // This will be handled by NavButton_Click
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            // This will be handled by NavButton_Click
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            // This will be handled by NavButton_Click
        }
    }
}