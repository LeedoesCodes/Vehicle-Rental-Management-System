namespace Vehicle_Rental_Management_System.Controls
{
    partial class NavButton
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavButton));
            this.mainPanel = new System.Windows.Forms.Panel();
            this.btnTextLabel = new System.Windows.Forms.Label();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.btnTextLabel);
            this.mainPanel.Controls.Add(this.iconPictureBox);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(184, 36);
            this.mainPanel.TabIndex = 0;
            this.mainPanel.UseWaitCursor = true;
            // 
            // btnTextLabel
            // 
            this.btnTextLabel.AutoSize = true;
            this.btnTextLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTextLabel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTextLabel.Location = new System.Drawing.Point(45, 13);
            this.btnTextLabel.Name = "btnTextLabel";
            this.btnTextLabel.Size = new System.Drawing.Size(75, 17);
            this.btnTextLabel.TabIndex = 1;
            this.btnTextLabel.Text = "Dashboard";
            this.btnTextLabel.UseWaitCursor = true;
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.BackColor = System.Drawing.Color.White;
            this.iconPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("iconPictureBox.Image")));
            this.iconPictureBox.Location = new System.Drawing.Point(15, 12);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(20, 20);
            this.iconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconPictureBox.TabIndex = 0;
            this.iconPictureBox.TabStop = false;
            this.iconPictureBox.UseWaitCursor = true;
            // 
            // NavButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.Controls.Add(this.mainPanel);
            this.Name = "NavButton";
            this.Size = new System.Drawing.Size(184, 36);
            this.UseWaitCursor = true;
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Label btnTextLabel;
    }
}