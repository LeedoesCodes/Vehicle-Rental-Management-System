using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vehicle_Rental_Management_System.Forms;

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class VehiclesView : UserControl
    {
        // Connection String
        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                    ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

        // State variables
        private string currentImagePath = "";
        private int currentVehicleId = -1;

        public VehiclesView()
        {
            InitializeComponent();

            // This is the ONLY layout code we keep, because making a label 
            // transparent over an image is hard to do in the Designer.
            SetupImageLabel();

            LoadVehicles();
        }

        private void SetupImageLabel()
        {
            // Make the label child of picture box so it sits on top transparently
            lblVehicleDetails.Parent = picVehiclePreview;
            lblVehicleDetails.BackColor = Color.FromArgb(180, 255, 255, 255); // Transparent White
            lblVehicleDetails.Dock = DockStyle.Bottom; // Stick to bottom of image
            lblVehicleDetails.TextAlign = ContentAlignment.MiddleCenter;
            lblVehicleDetails.BringToFront();
        }

        public void LoadVehicles()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetAllVehicles", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvVehicles.DataSource = dt;
                            FormatGrid();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading vehicles: " + ex.Message, "Error");
                }
            }
        }

        private void FormatGrid()
        {
            // 1. Hide unwanted columns
            string[] hiddenColumns = {
                "VehicleId", "VIN", "Transmission", "FuelType",
                "Features", "CreatedDate", "ImagePath", "CategoryId", "BaseDailyRate"
            };

            foreach (string col in hiddenColumns)
            {
                if (dgvVehicles.Columns.Contains(col)) dgvVehicles.Columns[col].Visible = false;
            }

            // 2. Format Money and Numbers
            if (dgvVehicles.Columns.Contains("DailyRate"))
                dgvVehicles.Columns["DailyRate"].DefaultCellStyle.Format = "C2";

            if (dgvVehicles.Columns.Contains("CurrentMileage"))
                dgvVehicles.Columns["CurrentMileage"].DefaultCellStyle.Format = "N0";

            // 3. General Settings
            dgvVehicles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Auto-fit width
            dgvVehicles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehicles.MultiSelect = false;

            // Re-attach event if needed (Designer usually handles this, but good to be safe)
            dgvVehicles.SelectionChanged -= DgvVehicles_SelectionChanged;
            dgvVehicles.SelectionChanged += DgvVehicles_SelectionChanged;
        }

        private void DgvVehicles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvVehicles.SelectedRows.Count > 0)
            {
                ShowVehiclePreview(dgvVehicles.SelectedRows[0]);
                ToggleButtons(true);
            }
            else
            {
                ClearPreview();
                ToggleButtons(false);
            }
        }

        private void ShowVehiclePreview(DataGridViewRow row)
        {
            try
            {
                // Extract Data
                currentVehicleId = Convert.ToInt32(row.Cells["VehicleId"].Value);
                string make = row.Cells["Make"].Value?.ToString();
                string model = row.Cells["Model"].Value?.ToString();
                string year = row.Cells["Year"].Value?.ToString();
                string category = row.Cells["CategoryName"].Value?.ToString();

                // Update UI
                lblVehicleDetails.Text = $"{year} {make} {model}\n{category}";
                picVehiclePreview.Visible = true;

                // Handle Image
                string path = row.Cells["ImagePath"]?.Value?.ToString();
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    currentImagePath = path;
                    // Dispose old image to free memory
                    if (picVehiclePreview.Image != null) picVehiclePreview.Image.Dispose();
                    picVehiclePreview.Image = Image.FromFile(path);
                }
                else
                {
                    ShowDefaultImage(category);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ShowDefaultImage("Unknown");
            }
        }

        private void ShowDefaultImage(string category)
        {
            // Create a simple blank image with text
            Bitmap bmp = new Bitmap(Math.Max(1, picVehiclePreview.Width), Math.Max(1, picVehiclePreview.Height));
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.WhiteSmoke);
                using (Font f = new Font("Arial", 10, FontStyle.Bold))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString(category ?? "Vehicle", f, Brushes.Gray,
                        new RectangleF(0, 0, bmp.Width, bmp.Height), sf);
                }
            }

            if (picVehiclePreview.Image != null) picVehiclePreview.Image.Dispose();
            picVehiclePreview.Image = bmp;
            currentImagePath = "";
        }

        private void ClearPreview()
        {
            if (picVehiclePreview.Image != null) picVehiclePreview.Image.Dispose();
            picVehiclePreview.Image = null;
            picVehiclePreview.Visible = false;
            lblVehicleDetails.Text = "Select a vehicle";
            currentImagePath = "";
            currentVehicleId = -1;
        }

        private void ToggleButtons(bool enabled)
        {
            if (btnEdit != null) btnEdit.Enabled = enabled;
            if (btnDelete != null) btnDelete.Enabled = enabled;
        }

        // ================= BUTTON EVENTS =================

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (AddVehicleForm addForm = new AddVehicleForm())
                {
                    if (addForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadVehicles();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadVehicles();
            ClearPreview();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvVehicles.SelectedRows.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to delete this vehicle?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DeleteVehicle(currentVehicleId);
            }
        }

        private void DeleteVehicle(int vehicleId)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("sp_RetireVehicle", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_VehicleId", vehicleId);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Vehicle deleted successfully.");
                            LoadVehicles();
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        // ---------------------------------------------------------
        // UPDATED EDIT BUTTON LOGIC
        // ---------------------------------------------------------
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // 1. Check if a row is selected
            if (dgvVehicles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a vehicle to edit.");
                return;
            }

            try
            {
                // 2. Get the VehicleId from the selected row (It's a hidden column)
                int vehicleId = Convert.ToInt32(dgvVehicles.SelectedRows[0].Cells["VehicleId"].Value);

                // 3. Open the Edit Form passing the ID
                using (EditVehicleForm editForm = new EditVehicleForm(vehicleId))
                {
                    // 4. If the user clicked "Save" (OK), refresh the grid
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadVehicles();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening edit form: " + ex.Message);
            }
        }

        private void picVehiclePreview_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentImagePath) && File.Exists(currentImagePath))
            {
                // Simple full screen viewer
                using (Form f = new Form())
                {
                    f.Text = lblVehicleDetails.Text.Replace("\n", " ");
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.Size = new Size(800, 600);
                    PictureBox pb = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        Image = Image.FromFile(currentImagePath),
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    f.Controls.Add(pb);
                    f.ShowDialog();
                }
            }
        }

        // Cleanup resources
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            if (disposing && picVehiclePreview.Image != null) picVehiclePreview.Image.Dispose();
            base.Dispose(disposing);
        }
    }
}