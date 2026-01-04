using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Vehicle_Rental_Management_System.Forms
{
    public partial class CameraForm : Form
    {
        public Image CapturedImage { get; private set; }
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        // Manual Controls
        private PictureBox pbVideo;
        private Button btnCapture;
        private ComboBox cbCameras;

        // Safety flag to prevent freezing
        private bool isStopping = false;

        public CameraForm()
        {
            InitializeComponent_Manual();
            LoadCameras();
        }

        private void LoadCameras()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                {
                    MessageBox.Show("No camera found.");
                    return;
                }

                foreach (FilterInfo device in videoDevices)
                    cbCameras.Items.Add(device.Name);

                cbCameras.SelectedIndex = 0;
                StartCamera();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading camera: " + ex.Message);
            }
        }

        private void StartCamera()
        {
            StopCamera(); // Ensure old camera is closed first

            if (videoDevices.Count > 0)
            {
                isStopping = false; // Reset flag
                videoSource = new VideoCaptureDevice(videoDevices[cbCameras.SelectedIndex].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // If we are shutting down, DO NOT try to update the UI.
            // This prevents the deadlock freeze.
            if (isStopping) return;

            try
            {
                Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();

                // Check if form is disposing to avoid errors
                if (this.Disposing || this.IsDisposed) return;

                // Update UI safely
                pbVideo.BeginInvoke(new MethodInvoker(() =>
                {
                    if (isStopping || this.IsDisposed) return;

                    Image old = pbVideo.Image;
                    pbVideo.Image = bmp;
                    if (old != null) old.Dispose();
                }));
            }
            catch { /* Ignore errors during shutdown */ }
        }

        private void StopCamera()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                // 1. Set flag so the background thread stops talking to UI
                isStopping = true;

                // 2. Unsubscribe immediately to cut the connection
                videoSource.NewFrame -= VideoSource_NewFrame;

                // 3. Ask camera to stop
                videoSource.SignalToStop();

                // 4. Wait for it to finish (this won't freeze now because connection is cut)
                videoSource.WaitForStop();

                videoSource = null;
            }
        }

        private void BtnCapture_Click(object sender, EventArgs e)
        {
            if (pbVideo.Image != null)
            {
                // 1. Grab the image
                CapturedImage = (Image)pbVideo.Image.Clone();

                // 2. Stop camera explicitly
                StopCamera();

                // 3. Set Result and Close
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        // Handles the user clicking "X"
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopCamera();
            base.OnFormClosing(e);
        }

        private void InitializeComponent_Manual()
        {
            this.Size = new Size(500, 480);
            this.Text = "Take Photo";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            cbCameras = new ComboBox { Parent = this, Top = 10, Left = 10, Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            cbCameras.SelectedIndexChanged += (s, e) => StartCamera();

            pbVideo = new PictureBox { Parent = this, Top = 50, Left = 10, Width = 460, Height = 300, BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Black };

            btnCapture = new Button { Parent = this, Text = "📷 Capture Photo", Top = 370, Left = 150, Width = 180, Height = 40, BackColor = Color.Teal, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnCapture.FlatAppearance.BorderSize = 0;
            btnCapture.Click += BtnCapture_Click;
        }
    }
}