using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class ReservationsView : UserControl
    {
        private DataGridView dgvReservations;

        public ReservationsView()
        {
            InitializeComponent();
            SetupUI();
            LoadReservations(); // Fetches from Reservations Table
        }

        private void SetupUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // 1. Header
            Label lblTitle = new Label();
            lblTitle.Text = "Future Reservations"; // Distinct Title
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            // 2. Buttons
            Button btnAdd = CreateButton("➕ New Reservation", 20, 70, Color.FromArgb(40, 167, 69));
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            Button btnCancel = CreateButton("❌ Cancel Selected", 210, 70, Color.FromArgb(220, 53, 69));
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);

            Button btnRefresh = CreateButton("🔄 Refresh", 400, 70, Color.Gray);
            btnRefresh.Width = 100;
            btnRefresh.Click += (s, e) => LoadReservations();
            this.Controls.Add(btnRefresh);

            // 3. Grid
            dgvReservations = new DataGridView();
            dgvReservations.Location = new Point(20, 130);
            dgvReservations.Size = new Size(900, 500);
            dgvReservations.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dgvReservations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReservations.BackgroundColor = Color.WhiteSmoke;
            dgvReservations.BorderStyle = BorderStyle.None;
            dgvReservations.RowHeadersVisible = false;
            dgvReservations.AllowUserToAddRows = false;
            dgvReservations.ReadOnly = true;
            dgvReservations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReservations.CellFormatting += DgvReservations_CellFormatting;
            this.Controls.Add(dgvReservations);
        }

        private Button CreateButton(string text, int x, int y, Color bg)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(180, 40),
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatAppearance = { BorderSize = 0 }
            };
        }

        public void LoadReservations()
        {
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // --- DISTINCT LOGIC: Calls sp_GetAllReservations ---
                    using (MySqlCommand cmd = new MySqlCommand("sp_GetAllReservations", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvReservations.DataSource = dt;
                            ConfigureGridColumns();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading reservations: " + ex.Message);
                }
            }
        }

        private void ConfigureGridColumns()
        {
            if (dgvReservations.DataSource == null) return;

            // Hide Database IDs
            string[] hiddenCols = { "ReservationId", "CustomerId", "VehicleId" };
            foreach (var col in hiddenCols)
                if (dgvReservations.Columns.Contains(col)) dgvReservations.Columns[col].Visible = false;

            // Format Money & Dates
            if (dgvReservations.Columns.Contains("TotalAmount"))
                dgvReservations.Columns["TotalAmount"].DefaultCellStyle.Format = "C2"; // Currency

            if (dgvReservations.Columns.Contains("StartDate"))
                dgvReservations.Columns["StartDate"].DefaultCellStyle.Format = "d"; // Short Date

            if (dgvReservations.Columns.Contains("EndDate"))
                dgvReservations.Columns["EndDate"].DefaultCellStyle.Format = "d";
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Opens the AddReservationForm we created previously
            Forms.AddReservationForm form = new Forms.AddReservationForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadReservations();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0) { MessageBox.Show("Select a reservation."); return; }

            int resId = Convert.ToInt32(dgvReservations.SelectedRows[0].Cells["ReservationId"].Value);
            string status = dgvReservations.SelectedRows[0].Cells["Status"].Value.ToString();

            if (status == "Cancelled") { MessageBox.Show("Already cancelled."); return; }

            if (MessageBox.Show("Cancel this reservation?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                CancelReservationInDb(resId);
            }
        }

        private void CancelReservationInDb(int id)
        {
            string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("sp_CancelReservation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_ReservationId", id);
                    cmd.ExecuteNonQuery();
                }
                LoadReservations();
            }
        }

        // Color coding for status
        private void DgvReservations_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvReservations.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                if (status == "Cancelled") e.CellStyle.ForeColor = Color.Red;
                if (status == "Confirmed") e.CellStyle.ForeColor = Color.Green;
            }
        }
    }
}