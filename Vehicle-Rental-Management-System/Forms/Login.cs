using System;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            SetupForm();

            // FIX 1: Manually connect the button to ensure it works
            // This bypasses the Designer error you likely have
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
        }

        private void SetupForm()
        {
            this.Text = "Vehicle Rental System - Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            if (txtPassword != null) txtPassword.PasswordChar = '*';
            if (txtUsername != null) txtUsername.Focus();
        }

        // This is the method that MUST run when you click Login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            PerformLogin();
        }

        private void PerformLogin()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                var user = ValidateUserCredentials(username, password);

                if (user != null)
                {
                    // FIX 2: Save to GLOBAL Program class
                    // This ensures MainForm knows who is logged in
                    Program.CurrentUserId = user.Id;
                    Program.CurrentUsername = user.Username;
                    Program.CurrentUserRole = user.Role;

                    // Tell the Welcome form that login was a success
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Database Connection Error:\n{ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"System Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private UserInfo ValidateUserCredentials(string username, string password)
        {
            // 1. HARDCODED ADMIN (For testing)
            if (username == "admin" && password == "admin123")
            {
                return new UserInfo { Id = 1, Username = "admin", Role = "Admin", FullName = "System Administrator" };
            }

            // 2. DATABASE CHECK
            string connectionString = "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            if (ConfigurationManager.ConnectionStrings["MySqlConnection"] != null)
                connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT UserId, Username, Role, FullName FROM Users WHERE Username = @User AND (PasswordHash = @Pass OR PasswordHash IS NULL)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@User", username);
                    cmd.Parameters.AddWithValue("@Pass", password);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserInfo
                            {
                                Id = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                Role = reader["Role"].ToString(),
                                FullName = reader["FullName"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) { txtPassword.Focus(); e.Handled = true; }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) { PerformLogin(); e.Handled = true; }
        }

        private class UserInfo
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string FullName { get; set; }
            public string Role { get; set; }
        }
    }
}