using System;
using System.Drawing;
using System.Windows.Forms;

using Vehicle_Rental_Management_System.Controls;

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
          
            if (lbluserInfo != null)
            {
                lbluserInfo.Text = $"Welcome,\n{Program.CurrentUsername}\n({Program.CurrentUserRole})";
            }
            this.Text = $"Vehicle Rental System - {Program.CurrentUsername}";

            
            if (Program.CurrentUserRole != "Admin" && btnAdmin != null)
            {
                btnAdmin.Visible = false;
            }

           
            SetupButtonEvents();

       
            ActivateButton(btnDashboard);
            ShowDashboard();
        }

        private void SetupButtonEvents()
        {
            
            Button[] navButtons = {
                btnDashboard,
                btnVehicles,
                btnCustomers,
                btnRentals,
                btnReservation,
                btnReports,
                btnAdmin
            };

            foreach (Button button in navButtons)
            {
                if (button != null)
                {
                    if (button == btnAdmin && !btnAdmin.Visible) continue;

                    
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.BackColor = normalColor;
                    button.ForeColor = Color.White;
                    button.Font = new Font("Segoe UI", 10);
                    button.TextAlign = ContentAlignment.MiddleLeft;
                    button.Padding = new Padding(15, 0, 0, 0);

                    
                    button.Click += NavButton_Click;
                    button.MouseEnter += Button_MouseEnter;
                    button.MouseLeave += Button_MouseLeave;
                }
            }

            if (btnLogout != null)
            {
                btnLogout.FlatStyle = FlatStyle.Flat;
                btnLogout.FlatAppearance.BorderSize = 0;
                btnLogout.BackColor = Color.FromArgb(220, 53, 69);
                btnLogout.ForeColor = Color.White;
                btnLogout.Font = new Font("Segoe UI", 10);
                btnLogout.TextAlign = ContentAlignment.MiddleLeft;
                btnLogout.Padding = new Padding(15, 0, 0, 0);
                btnLogout.Click += (s, e) => Logout();
                btnLogout.MouseEnter += (s, e) => btnLogout.BackColor = Color.FromArgb(200, 35, 51);
                btnLogout.MouseLeave += (s, e) => btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            }
        }

        
        private void NavButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == activeButton) return;

            if (activeButton != null)
            {
                activeButton.BackColor = normalColor;
                activeButton.Font = new Font(activeButton.Font, FontStyle.Regular);
            }

            ActivateButton(clickedButton);
            HandleNavigation(clickedButton);
        }

        private void ActivateButton(Button button)
        {
            if (button != null)
            {
                button.BackColor = activeColor;
                button.Font = new Font(button.Font, FontStyle.Bold);
                activeButton = button;
            }
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

                case "btnReservation":
                    ShowReservations();
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
            contentPanel.Controls.Clear();
            var view = new DashboardView();
            view.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(view);
        }

        private void ShowVehicleManagement()
        {
            contentPanel.Controls.Clear();
            var view = new VehiclesView();
            view.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(view);
        }

        private void ShowRentalManagement()
        {
            contentPanel.Controls.Clear();
            var view = new RentalsView();
            view.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(view);
        }

        private void ShowReservations()
        {
            contentPanel.Controls.Clear();

            
            var view = new ReservationsView();
            view.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(view);
        }

        private void ShowCustomerManagement()
        {
            contentPanel.Controls.Clear();

            
            var view = new CustomersView();

         
            view.Dock = DockStyle.Fill;

     
            contentPanel.Controls.Add(view);
        }

        private void ShowReports()
        {
            contentPanel.Controls.Clear();
            Label lbl = new Label { Text = "Reports Panel", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(50, 30), AutoSize = true };
            contentPanel.Controls.Add(lbl);
        }

        private void ShowAdminPanel()
        {
            contentPanel.Controls.Clear();
            Label lbl = new Label { Text = "Admin Panel", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(50, 30), AutoSize = true };
            contentPanel.Controls.Add(lbl);
        }

        private Button CreateActionButton(string text, int x, int y, Color color)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(180, 45),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
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

        private void Logout()
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void OpenAddCustomerForm()
        {
            try { new CustomerAddForm { StartPosition = FormStartPosition.CenterParent }.ShowDialog(this); }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void OpenCustomerListForm()
        {
            try { new CustomerListForm { StartPosition = FormStartPosition.CenterParent }.Show(); }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void SearchCustomers()
        {
            using (Form f = new Form { Text = "Search", Size = new Size(300, 150), StartPosition = FormStartPosition.CenterParent })
            {
                TextBox txt = new TextBox { Left = 50, Top = 20, Width = 200 };
                Button btn = new Button { Text = "Search", Left = 50, Top = 60, DialogResult = DialogResult.OK };
                f.Controls.Add(txt); f.Controls.Add(btn);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Searching for: " + txt.Text);
                }
            }
        }

       
        private void navButtonsPanel_Paint(object sender, PaintEventArgs e) { }
        private void btnDashboard_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void lbluserInfo_Click(object sender, EventArgs e) { }
        private void btnVehicles_Click(object sender, EventArgs e) { }
        private void btnCustomers_Click(object sender, EventArgs e) { }
        private void btnRentals_Click(object sender, EventArgs e) { }
        private void btnReports_Click(object sender, EventArgs e) { }
        private void btnAdmin_Click(object sender, EventArgs e) { }
        private void contentPanel_Paint(object sender, PaintEventArgs e) { }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}