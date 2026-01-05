using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class AddDamageForm : Form
    {
        // This object holds the report data to send back
        public DamageReport ResultReport { get; private set; }
        private string _tempImagePath = "";

        public AddDamageForm()
        {
            InitializeComponent();
            cbType.Items.AddRange(new object[] { "Scratch", "Dent", "Broken Part", "Stain", "Missing Item", "Other" });
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Images|*.jpg;*.png;*.jpeg" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _tempImagePath = ofd.FileName;
                pbEvidence.Image = Image.FromFile(_tempImagePath);
                pbEvidence.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbType.Text)) { MessageBox.Show("Select a damage type."); return; }

            // Create the report object
            ResultReport = new DamageReport
            {
                Type = cbType.Text,
                Description = txtDescription.Text,
                Fee = numFee.Value,
                TempImagePath = _tempImagePath // We save the file later
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    // Simple helper class to store the data
    public class DamageReport
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Fee { get; set; }
        public string TempImagePath { get; set; } // Location of file on disk
    }
}