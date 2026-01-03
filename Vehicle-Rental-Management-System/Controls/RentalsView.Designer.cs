namespace Vehicle_Rental_Management_System.Controls
{
    partial class RentalsView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnNewRental = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.dgvRentals = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblDetailAmount = new System.Windows.Forms.Label();
            this.lblDetailDates = new System.Windows.Forms.Label();
            this.lblDetailCustomer = new System.Windows.Forms.Label();
            this.lblDetailVehicle = new System.Windows.Forms.Label();
            this.pbVehicle = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRentals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVehicle)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(16, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(254, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Rental Transaction";
            // 
            // btnNewRental
            // 
            this.btnNewRental.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnNewRental.FlatAppearance.BorderSize = 0;
            this.btnNewRental.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewRental.ForeColor = System.Drawing.Color.White;
            this.btnNewRental.Location = new System.Drawing.Point(20, 56);
            this.btnNewRental.Name = "btnNewRental";
            this.btnNewRental.Size = new System.Drawing.Size(150, 40);
            this.btnNewRental.TabIndex = 1;
            this.btnNewRental.Text = "➕ New Rental";
            this.btnNewRental.UseVisualStyleBackColor = false;
            this.btnNewRental.Click += new System.EventHandler(this.BtnNewRental_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnReturn.FlatAppearance.BorderSize = 0;
            this.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturn.ForeColor = System.Drawing.Color.White;
            this.btnReturn.Location = new System.Drawing.Point(176, 56);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(150, 40);
            this.btnReturn.TabIndex = 2;
            this.btnReturn.Text = "↩ Return Vehicle";
            this.btnReturn.UseVisualStyleBackColor = false;
            this.btnReturn.Click += new System.EventHandler(this.BtnReturn_Click);
            // 
            // dgvRentals
            // 
            this.dgvRentals.AllowUserToAddRows = false;
            this.dgvRentals.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRentals.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvRentals.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRentals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRentals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRentals.Location = new System.Drawing.Point(0, 0);
            this.dgvRentals.Name = "dgvRentals";
            this.dgvRentals.ReadOnly = true;
            this.dgvRentals.RowHeadersVisible = false;
            this.dgvRentals.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRentals.Size = new System.Drawing.Size(864, 457);
            this.dgvRentals.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(3, 112);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvRentals);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblDetailAmount);
            this.splitContainer1.Panel2.Controls.Add(this.lblDetailDates);
            this.splitContainer1.Panel2.Controls.Add(this.lblDetailCustomer);
            this.splitContainer1.Panel2.Controls.Add(this.lblDetailVehicle);
            this.splitContainer1.Panel2.Controls.Add(this.pbVehicle);
            this.splitContainer1.Size = new System.Drawing.Size(1220, 457);
            this.splitContainer1.SplitterDistance = 864;
            this.splitContainer1.TabIndex = 4;
            // 
            // lblDetailAmount
            // 
            this.lblDetailAmount.AutoSize = true;
            this.lblDetailAmount.Location = new System.Drawing.Point(27, 270);
            this.lblDetailAmount.Name = "lblDetailAmount";
            this.lblDetailAmount.Size = new System.Drawing.Size(35, 13);
            this.lblDetailAmount.TabIndex = 4;
            this.lblDetailAmount.Text = "label1";
            // 
            // lblDetailDates
            // 
            this.lblDetailDates.AutoSize = true;
            this.lblDetailDates.Location = new System.Drawing.Point(27, 250);
            this.lblDetailDates.Name = "lblDetailDates";
            this.lblDetailDates.Size = new System.Drawing.Size(35, 13);
            this.lblDetailDates.TabIndex = 3;
            this.lblDetailDates.Text = "label1";
            // 
            // lblDetailCustomer
            // 
            this.lblDetailCustomer.AutoSize = true;
            this.lblDetailCustomer.Location = new System.Drawing.Point(27, 228);
            this.lblDetailCustomer.Name = "lblDetailCustomer";
            this.lblDetailCustomer.Size = new System.Drawing.Size(35, 13);
            this.lblDetailCustomer.TabIndex = 2;
            this.lblDetailCustomer.Text = "label1";
            // 
            // lblDetailVehicle
            // 
            this.lblDetailVehicle.AutoSize = true;
            this.lblDetailVehicle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetailVehicle.Location = new System.Drawing.Point(26, 198);
            this.lblDetailVehicle.Name = "lblDetailVehicle";
            this.lblDetailVehicle.Size = new System.Drawing.Size(57, 21);
            this.lblDetailVehicle.TabIndex = 1;
            this.lblDetailVehicle.Text = "label1";
            // 
            // pbVehicle
            // 
            this.pbVehicle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbVehicle.Location = new System.Drawing.Point(53, 3);
            this.pbVehicle.Name = "pbVehicle";
            this.pbVehicle.Size = new System.Drawing.Size(250, 180);
            this.pbVehicle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbVehicle.TabIndex = 0;
            this.pbVehicle.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.btnNewRental);
            this.panel1.Controls.Add(this.btnReturn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1223, 106);
            this.panel1.TabIndex = 5;
            // 
            // RentalsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "RentalsView";
            this.Size = new System.Drawing.Size(1223, 600);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRentals)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbVehicle)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnNewRental;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.DataGridView dgvRentals;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDetailAmount;
        private System.Windows.Forms.Label lblDetailDates;
        private System.Windows.Forms.Label lblDetailCustomer;
        private System.Windows.Forms.Label lblDetailVehicle;
        private System.Windows.Forms.PictureBox pbVehicle;
    }
}
