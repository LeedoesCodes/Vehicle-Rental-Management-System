using System;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class VehicleManagementForm : Form
    {
        public VehicleManagementForm()
        {
            InitializeComponent();
            this.Text = "Vehicle Management";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(800, 600);

            // Add a simple label
            Label lbl = new Label();
            lbl.Text = "Vehicle Management - Under Construction";
            lbl.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            lbl.AutoSize = true;
            lbl.Location = new System.Drawing.Point(50, 50);
            this.Controls.Add(lbl);
        }
    }
}