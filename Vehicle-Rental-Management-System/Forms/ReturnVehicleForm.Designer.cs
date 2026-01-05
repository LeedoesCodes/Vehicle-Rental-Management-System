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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numLateFee = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numDamages = new System.Windows.Forms.NumericUpDown();
            this.dgvDamages = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLateFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDamages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDamages)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vehicle Return";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCustomerInfo
            // 
            this.lblCustomerInfo.AutoSize = true;
            this.lblCustomerInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerInfo.Location = new System.Drawing.Point(27, 123);
            this.lblCustomerInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCustomerInfo.Name = "lblCustomerInfo";
            this.lblCustomerInfo.Size = new System.Drawing.Size(98, 23);
            this.lblCustomerInfo.TabIndex = 1;
            this.lblCustomerInfo.Text = "Customer: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 172);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Return Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(27, 234);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(186, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Final Odometer Reading: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(27, 295);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Fuel Level:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(27, 357);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(175, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Final Condition / Notes:";
            // 
            // dtReturns
            // 
            this.dtReturns.Location = new System.Drawing.Point(27, 197);
            this.dtReturns.Margin = new System.Windows.Forms.Padding(4);
            this.dtReturns.Name = "dtReturns";
            this.dtReturns.Size = new System.Drawing.Size(265, 22);
            this.dtReturns.TabIndex = 6;
            // 
            // txtOdometers
            // 
            this.txtOdometers.Location = new System.Drawing.Point(27, 258);
            this.txtOdometers.Margin = new System.Windows.Forms.Padding(4);
            this.txtOdometers.Name = "txtOdometers";
            this.txtOdometers.Size = new System.Drawing.Size(452, 22);
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
            this.cbFuels.Location = new System.Drawing.Point(27, 320);
            this.cbFuels.Margin = new System.Windows.Forms.Padding(4);
            this.cbFuels.Name = "cbFuels";
            this.cbFuels.Size = new System.Drawing.Size(452, 24);
            this.cbFuels.TabIndex = 8;
            // 
            // txtConditions
            // 
            this.txtConditions.Location = new System.Drawing.Point(27, 382);
            this.txtConditions.Margin = new System.Windows.Forms.Padding(4);
            this.txtConditions.Multiline = true;
            this.txtConditions.Name = "txtConditions";
            this.txtConditions.Size = new System.Drawing.Size(452, 73);
            this.txtConditions.TabIndex = 9;
            // 
            // lblVehicleInfo
            // 
            this.lblVehicleInfo.AutoSize = true;
            this.lblVehicleInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVehicleInfo.Location = new System.Drawing.Point(27, 86);
            this.lblVehicleInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVehicleInfo.Name = "lblVehicleInfo";
            this.lblVehicleInfo.Size = new System.Drawing.Size(71, 23);
            this.lblVehicleInfo.TabIndex = 10;
            this.lblVehicleInfo.Text = "Vehicle:";
            // 
            // btnConfirms
            // 
            this.btnConfirms.BackColor = System.Drawing.Color.Blue;
            this.btnConfirms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirms.ForeColor = System.Drawing.Color.White;
            this.btnConfirms.Location = new System.Drawing.Point(252, 628);
            this.btnConfirms.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfirms.Name = "btnConfirms";
            this.btnConfirms.Size = new System.Drawing.Size(227, 55);
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
            this.btnCancels.Location = new System.Drawing.Point(27, 627);
            this.btnCancels.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancels.Name = "btnCancels";
            this.btnCancels.Size = new System.Drawing.Size(200, 55);
            this.btnCancels.TabIndex = 12;
            this.btnCancels.Text = "Cancel";
            this.btnCancels.UseVisualStyleBackColor = false;
            this.btnCancels.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTotal);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.numLateFee);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(27, 462);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 137);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fees and Billing";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(11, 99);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(51, 20);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Total:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(11, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 20);
            this.label7.TabIndex = 3;
            this.label7.Text = "Late Fees ( ₱):";
            // 
            // numLateFee
            // 
            this.numLateFee.Location = new System.Drawing.Point(10, 59);
            this.numLateFee.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numLateFee.Name = "numLateFee";
            this.numLateFee.Size = new System.Drawing.Size(202, 27);
            this.numLateFee.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(516, 555);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Damage Fees ( ₱):";
            // 
            // numDamages
            // 
            this.numDamages.Location = new System.Drawing.Point(516, 577);
            this.numDamages.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numDamages.Name = "numDamages";
            this.numDamages.Size = new System.Drawing.Size(202, 22);
            this.numDamages.TabIndex = 0;
            // 
            // dgvDamages
            // 
            this.dgvDamages.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvDamages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDamages.Location = new System.Drawing.Point(516, 96);
            this.dgvDamages.Name = "dgvDamages";
            this.dgvDamages.RowHeadersWidth = 51;
            this.dgvDamages.RowTemplate.Height = 24;
            this.dgvDamages.Size = new System.Drawing.Size(475, 430);
            this.dgvDamages.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Blue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(516, 25);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(227, 55);
            this.button1.TabIndex = 15;
            this.button1.Text = "Complete Return";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // ReturnVehicleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 703);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgvDamages);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancels);
            this.Controls.Add(this.btnConfirms);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numDamages);
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
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "ReturnVehicleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Process Return";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLateFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDamages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDamages)).EndInit();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numDamages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numLateFee;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvDamages;
        private System.Windows.Forms.Button button1;
    }
}