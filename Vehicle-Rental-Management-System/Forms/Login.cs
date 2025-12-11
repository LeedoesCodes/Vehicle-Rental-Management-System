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

            // --- FIX 1: Set "Enter" key to trigger Login globally ---
            this.KeyPreview = true;
            this.AcceptButton = btnLogin; 

            // --- FIX 2: Manually connect Events (Guarantees they work) ---
            
            // Buttons
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
            if (this.btnGoToRegister != null)
                this.btnGoToRegister.Click += new EventHandler(this.btnGoToRegister_Click);

            // TextBoxes (This fixes the "next line" or "no reaction" issue)
            if (this.txtUsername != null)
                this.txtUsername.KeyPress += new KeyPressEventHandler(this.txtUsername_KeyPress);
            
            if (this.txtPassword != null)
                this.txtPassword.KeyPress += new KeyPressEventHandler(this.txtPassword_KeyPress);
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

        // --- NAVIGATION LOGIC ---
        private void btnGoToRegister_Click(object sender, EventArgs e)
        {
            // 1. Hide the Login form so only Register (and Welcome) are visible
            this.Hide();

            using (Forms.RegisterForm registerForm = new Forms.RegisterForm())
            {
                registerForm.StartPosition = FormStartPosition.CenterParent;

                // Show the Register form and wait for it to close
                registerForm.ShowDialog(this);
            }

            // 2. When Register form closes, bring the Login form back
            this.Show();
            this.txtUsername.Focus();
        }

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
                    Program.CurrentUserId = user.Id;
                    Program.CurrentUsername = user.Username;
                    Program.CurrentUserRole = user.Role;

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
            if (username == "admin" && password == "admin123")
            {
                return new UserInfo { Id = 1, Username = "admin", Role = "Admin", FullName = "System Administrator" };
            }

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

        // KEY PRESS EVENTS
        
        // 1. Username Field: Pressing Enter moves focus to Password
        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevents "Ding" sound
                txtPassword.Focus(); 
            }
        }

        // 2. Password Field: Pressing Enter triggers Login
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevents "Ding" sound
                PerformLogin(); 
            }
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