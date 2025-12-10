using System;
using System.Windows.Forms;
using System.Drawing;

namespace Vehicle_Rental_Management_System
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();

            // --- MANUALLY FIX THE BUTTON CONNECTION ---
            // This line guarantees the button works, ignoring Designer errors.
            this.btnProceed.Click += new EventHandler(this.btnProceed_Click);
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            btnProceed.Focus();
        }

        // The logic to open the Login Form
        private void btnProceed_Click(object sender, EventArgs e)
        {
            // Debug message (You can remove this later once it works)
            MessageBox.Show("Button connection active. Attempting to open Login...", "Debug");

            ShowLoginForm();
        }

        private void ShowLoginForm()
        {
            using (Forms.Login loginForm = new Forms.Login())
            {
                // 1. Show the Login form and wait for it to close
                loginForm.StartPosition = FormStartPosition.CenterParent;
                DialogResult result = loginForm.ShowDialog(this);

                // 2. Check the signal we set in Step 1
                if (result == DialogResult.OK)
                {
                    // A. Show the Confirmation Message
                    MessageBox.Show($"Login Successful!\nWelcome, {Program.CurrentUsername}.",
                                  "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // B. Hide the Welcome Screen
                    this.Hide();

                    // C. Create and Show the Main Form
                    Forms.MainForm mainForm = new Forms.MainForm();
                    mainForm.Show();

                    // D. Ensure the app closes when the Main Form is closed
                    mainForm.FormClosed += (s, args) => this.Close();
                }
                else
                {
                    // If they cancelled or failed, just stay on Welcome screen
                    // No code needed here
                }
            }
        }

        // Allow pressing ENTER key to trigger the button
        private void Welcome_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnProceed_Click(sender, e);
            }
        }

        // Hover Effects
        private void btnProceed_MouseEnter(object sender, EventArgs e)
        {
            btnProceed.BackColor = Color.LightBlue;
        }

        private void btnProceed_MouseLeave(object sender, EventArgs e)
        {
            btnProceed.BackColor = SystemColors.Control;
        }
    }
}