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
          

            ShowLoginForm();
        }

        private void ShowLoginForm()
        {
            using (Forms.Login loginForm = new Forms.Login())
            {
                loginForm.StartPosition = FormStartPosition.CenterParent;

             

                DialogResult result = loginForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    MessageBox.Show($"Login Successful!\nWelcome, {Program.CurrentUsername}.",
                                  "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide(); // We ONLY hide Welcome after successful login to show Main

                    Forms.MainForm mainForm = new Forms.MainForm();
                    mainForm.Show();
                    mainForm.FormClosed += (s, args) => this.Close();
                }
            }
        }
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