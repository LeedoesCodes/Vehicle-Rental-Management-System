namespace Vehicle_Rental_Management_System.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mySqlCommand1 = new MySql.Data.MySqlClient.MySqlCommand();
            this.sidePanel = new System.Windows.Forms.Panel();
            this.navButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnVehicles = new System.Windows.Forms.Button();
            this.btnCustomers = new System.Windows.Forms.Button();
            this.btnRentals = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lbluserInfo = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.sidePanel.SuspendLayout();
            this.navButtonsPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mySqlCommand1
            // 
            this.mySqlCommand1.CacheAge = 0;
            this.mySqlCommand1.Connection = null;
            this.mySqlCommand1.EnableCaching = false;
            this.mySqlCommand1.Transaction = null;
            // 
            // sidePanel
            // 
            this.sidePanel.BackColor = System.Drawing.Color.DarkSlateGray;
            this.sidePanel.Controls.Add(this.navButtonsPanel);
            this.sidePanel.Controls.Add(this.headerPanel);
            this.sidePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidePanel.Location = new System.Drawing.Point(0, 0);
            this.sidePanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(293, 628);
            this.sidePanel.TabIndex = 0;
            // 
            // navButtonsPanel
            // 
            this.navButtonsPanel.BackColor = System.Drawing.Color.Transparent;
            this.navButtonsPanel.Controls.Add(this.btnDashboard);
            this.navButtonsPanel.Controls.Add(this.btnVehicles);
            this.navButtonsPanel.Controls.Add(this.btnCustomers);
            this.navButtonsPanel.Controls.Add(this.btnRentals);
            this.navButtonsPanel.Controls.Add(this.btnReports);
            this.navButtonsPanel.Controls.Add(this.btnAdmin);
            this.navButtonsPanel.Controls.Add(this.btnLogout);
            this.navButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navButtonsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.navButtonsPanel.Location = new System.Drawing.Point(0, 123);
            this.navButtonsPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.navButtonsPanel.Name = "navButtonsPanel";
            this.navButtonsPanel.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.navButtonsPanel.Size = new System.Drawing.Size(293, 505);
            this.navButtonsPanel.TabIndex = 1;
            this.navButtonsPanel.WrapContents = false;
            this.navButtonsPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.navButtonsPanel_Paint);
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.Transparent;
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnDashboard.Location = new System.Drawing.Point(13, 24);
            this.btnDashboard.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(237, 49);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "🏠 Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // btnVehicles
            // 
            this.btnVehicles.BackColor = System.Drawing.Color.Transparent;
            this.btnVehicles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVehicles.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVehicles.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnVehicles.Location = new System.Drawing.Point(13, 85);
            this.btnVehicles.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.btnVehicles.Name = "btnVehicles";
            this.btnVehicles.Size = new System.Drawing.Size(237, 49);
            this.btnVehicles.TabIndex = 1;
            this.btnVehicles.Text = "🚗 Vehicles";
            this.btnVehicles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVehicles.UseVisualStyleBackColor = false;
            this.btnVehicles.Click += new System.EventHandler(this.btnVehicles_Click);
            // 
            // btnCustomers
            // 
            this.btnCustomers.BackColor = System.Drawing.Color.Transparent;
            this.btnCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomers.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomers.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCustomers.Location = new System.Drawing.Point(13, 146);
            this.btnCustomers.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Size = new System.Drawing.Size(237, 49);
            this.btnCustomers.TabIndex = 2;
            this.btnCustomers.Text = "👥 Customers";
            this.btnCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCustomers.UseVisualStyleBackColor = false;
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            // 
            // btnRentals
            // 
            this.btnRentals.BackColor = System.Drawing.Color.Transparent;
            this.btnRentals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRentals.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRentals.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnRentals.Location = new System.Drawing.Point(13, 207);
            this.btnRentals.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.btnRentals.Name = "btnRentals";
            this.btnRentals.Size = new System.Drawing.Size(237, 49);
            this.btnRentals.TabIndex = 3;
            this.btnRentals.Text = "📋 Rentals";
            this.btnRentals.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRentals.UseVisualStyleBackColor = false;
            this.btnRentals.Click += new System.EventHandler(this.btnRentals_Click);
            // 
            // btnReports
            // 
            this.btnReports.BackColor = System.Drawing.Color.Transparent;
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReports.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnReports.Location = new System.Drawing.Point(13, 268);
            this.btnReports.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(237, 49);
            this.btnReports.TabIndex = 4;
            this.btnReports.Text = "📊 Reports";
            this.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.UseVisualStyleBackColor = false;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnAdmin
            // 
            this.btnAdmin.BackColor = System.Drawing.Color.Transparent;
            this.btnAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdmin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdmin.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAdmin.Location = new System.Drawing.Point(13, 329);
            this.btnAdmin.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(237, 49);
            this.btnAdmin.TabIndex = 5;
            this.btnAdmin.Text = "👑 Admin";
            this.btnAdmin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdmin.UseVisualStyleBackColor = false;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.IndianRed;
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnLogout.Location = new System.Drawing.Point(13, 390);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(237, 49);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = " Logout";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.button1_Click);
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.headerPanel.Controls.Add(this.lbluserInfo);
            this.headerPanel.Controls.Add(this.logoPictureBox);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(293, 123);
            this.headerPanel.TabIndex = 0;
            // 
            // lbluserInfo
            // 
            this.lbluserInfo.AutoSize = true;
            this.lbluserInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbluserInfo.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbluserInfo.Location = new System.Drawing.Point(107, 31);
            this.lbluserInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbluserInfo.Name = "lbluserInfo";
            this.lbluserInfo.Size = new System.Drawing.Size(278, 23);
            this.lbluserInfo.TabIndex = 0;
            this.lbluserInfo.Text = "\"Welcome, \\nUser Name\\n(Role)\"";
            this.lbluserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbluserInfo.Click += new System.EventHandler(this.lbluserInfo_Click);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(20, 18);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(67, 62);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            // 
            // contentPanel
            // 
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(293, 0);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(774, 628);
            this.contentPanel.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 628);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.sidePanel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vehicle Rental System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.sidePanel.ResumeLayout(false);
            this.navButtonsPanel.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MySql.Data.MySqlClient.MySqlCommand mySqlCommand1;
        private System.Windows.Forms.Panel sidePanel;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Label lbluserInfo;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.FlowLayoutPanel navButtonsPanel;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnVehicles;
        private System.Windows.Forms.Button btnCustomers;
        private System.Windows.Forms.Button btnRentals;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.Button btnLogout;
    }
}