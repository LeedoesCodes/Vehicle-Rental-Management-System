namespace Vehicle_Rental_Management_System.Forms
{
    partial class ReturnVehicleForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblCustomerInfo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtReturns = new System.Windows.Forms.DateTimePicker();
            this.txtOdometers = new System.Windows.Forms.TextBox();
            this.cbFuels = new System.Windows.Forms.ComboBox();
            this.txtConditions = new System.Windows.Forms.TextBox();
            this.lblVehicleInfo = new System.Windows.Forms.Label();
            this.btnConfirms = new System.Windows.Forms.Button();
            this.btnCancels = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vehicle Return";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCustomerInfo
            // 
            this.lblCustomerInfo.AutoSize = true;
            this.lblCustomerInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerInfo.Location = new System.Drawing.Point(20, 100);
            this.lblCustomerInfo.Name = "lblCustomerInfo";
            this.lblCustomerInfo.Size = new System.Drawing.Size(76, 17);
            this.lblCustomerInfo.TabIndex = 1;
            this.lblCustomerInfo.Text = "Customer: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Return Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(20, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Final Odometer Reading: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(20, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Fuel Level:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 290);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "Final Condition / Notes:";
            // 
            // dtReturns
            // 
            this.dtReturns.Location = new System.Drawing.Point(20, 160);
            this.dtReturns.Name = "dtReturns";
            this.dtReturns.Size = new System.Drawing.Size(200, 20);
            this.dtReturns.TabIndex = 6;
            // 
            // txtOdometers
            // 
            this.txtOdometers.Location = new System.Drawing.Point(20, 210);
            this.txtOdometers.Name = "txtOdometers";
            this.txtOdometers.Size = new System.Drawing.Size(340, 20);
            this.txtOdometers.TabIndex = 7;
            // 
            // cbFuels
            // 
            this.cbFuels.FormattingEnabled = true;
            this.cbFuels.Items.AddRange(new object[] {
            "Empty",
            "1/4",
            "1/2",
            "3/4",
            "Full"});
            this.cbFuels.Location = new System.Drawing.Point(20, 260);
            this.cbFuels.Name = "cbFuels";
            this.cbFuels.Size = new System.Drawing.Size(340, 21);
            this.cbFuels.TabIndex = 8;
            // 
            // txtConditions
            // 
            this.txtConditions.Location = new System.Drawing.Point(20, 310);
            this.txtConditions.Multiline = true;
            this.txtConditions.Name = "txtConditions";
            this.txtConditions.Size = new System.Drawing.Size(340, 60);
            this.txtConditions.TabIndex = 9;
            // 
            // lblVehicleInfo
            // 
            this.lblVehicleInfo.AutoSize = true;
            this.lblVehicleInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVehicleInfo.Location = new System.Drawing.Point(20, 70);
            this.lblVehicleInfo.Name = "lblVehicleInfo";
            this.lblVehicleInfo.Size = new System.Drawing.Size(56, 17);
            this.lblVehicleInfo.TabIndex = 10;
            this.lblVehicleInfo.Text = "Vehicle:";
            // 
            // btnConfirms
            // 
            this.btnConfirms.BackColor = System.Drawing.Color.Blue;
            this.btnConfirms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirms.ForeColor = System.Drawing.Color.White;
            this.btnConfirms.Location = new System.Drawing.Point(190, 400);
            this.btnConfirms.Name = "btnConfirms";
            this.btnConfirms.Size = new System.Drawing.Size(170, 45);
            this.btnConfirms.TabIndex = 11;
            this.btnConfirms.Text = "Complete Return";
            this.btnConfirms.UseVisualStyleBackColor = false;
            this.btnConfirms.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancels
            // 
            this.btnCancels.BackColor = System.Drawing.Color.IndianRed;
            this.btnCancels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancels.ForeColor = System.Drawing.Color.White;
            this.btnCancels.Location = new System.Drawing.Point(20, 400);
            this.btnCancels.Name = "btnCancels";
            this.btnCancels.Size = new System.Drawing.Size(150, 45);
            this.btnCancels.TabIndex = 12;
            this.btnCancels.Text = "Cancel";
            this.btnCancels.UseVisualStyleBackColor = false;
            this.btnCancels.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ReturnVehicleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 461);
            this.Controls.Add(this.btnCancels);
            this.Controls.Add(this.btnConfirms);
            this.Controls.Add(this.lblVehicleInfo);
            this.Controls.Add(this.txtConditions);
            this.Controls.Add(this.cbFuels);
            this.Controls.Add(this.txtOdometers);
            this.Controls.Add(this.dtReturns);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCustomerInfo);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ReturnVehicleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Process Return";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCustomerInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtReturns;
        private System.Windows.Forms.TextBox txtOdometers;
        private System.Windows.Forms.ComboBox cbFuels;
        private System.Windows.Forms.TextBox txtConditions;
        private System.Windows.Forms.Label lblVehicleInfo;
        private System.Windows.Forms.Button btnConfirms;
        private System.Windows.Forms.Button btnCancels;
    }
}