using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Vehicle_Rental_Management_System.Controls
{
    public partial class NavButton : UserControl
    {
        private bool _isActive = false;

        // Property to set/get button text
        public string ButtonText
        {
            get => btnTextLabel.Text;
            set => btnTextLabel.Text = value;
        }

        // Property to set/get icon image
        public Image IconImage
        {
            get => iconPictureBox.Image;
            set => iconPictureBox.Image = value;
        }

        // Property to control active state
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                UpdateAppearance();
            }
        }

        // Event for when button is clicked
        public event EventHandler NavButtonClick;

        public NavButton()
        {
            InitializeComponent();
            SetupButton();
        }

        private void SetupButton()
        {
            this.Cursor = Cursors.Hand; // Shows hand cursor on hover

            // Wire up click events to all controls
            foreach (Control control in this.Controls)
            {
                control.Click += (s, e) => NavButtonClick?.Invoke(this, e);
                control.MouseEnter += (s, e) => OnMouseEnter(e);
                control.MouseLeave += (s, e) => OnMouseLeave(e);
            }
        }

        private void UpdateAppearance()
        {
            if (_isActive)
            {
                this.BackColor = Color.FromArgb(0, 120, 215); // Blue when active
                mainPanel.BackColor = Color.FromArgb(0, 120, 215);
            }
            else
            {
                this.BackColor = Color.FromArgb(33, 33, 33); // Dark gray when inactive
                mainPanel.BackColor = Color.FromArgb(33, 33, 33);
            }
        }

        // Mouse hover effects
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!_isActive)
            {
                this.BackColor = Color.FromArgb(50, 50, 50); // Lighter gray on hover
                mainPanel.BackColor = Color.FromArgb(50, 50, 50);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!_isActive)
            {
                this.BackColor = Color.FromArgb(33, 33, 33); // Back to dark gray
                mainPanel.BackColor = Color.FromArgb(33, 33, 33);
            }
        }

        // Click handler
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            NavButtonClick?.Invoke(this, e);
        }

        // Public methods to control state
        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
