using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Vehicle_Rental_Management_System.Forms; // Required to see AddVehicleForm

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class VehiclesView : UserControl
    {
        // Define the connection string centrally so it's easy to change
        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString
                                    ?? "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";

        public VehiclesView()
        {
            InitializeComponent();

            // This forces the data to load immediately when the screen opens
            LoadVehicles();
        }

        // --- DATA LOADING ---
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

                            if (dgvVehicles != null)
                            {
                                dgvVehicles.DataSource = dt;
                                FormatGrid(); // Make it look nice
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading vehicles: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormatGrid()
        {
            // Only format if the columns actually exist
            if (dgvVehicles.Columns.Contains("DailyRate"))
            {
                dgvVehicles.Columns["DailyRate"].DefaultCellStyle.Format = "C2"; // Currency format (e.g., $50.00 or ₱50.00)
                dgvVehicles.Columns["DailyRate"].HeaderText = "Daily Rate";
            }
            if (dgvVehicles.Columns.Contains("VehicleId"))
            {
                dgvVehicles.Columns["VehicleId"].Visible = false; // Hide the internal ID
            }
            if (dgvVehicles.Columns.Contains("IsActive"))
            {
                dgvVehicles.Columns["IsActive"].Visible = false; // Hide status flag if it exists
            }

            // Make columns fill the width of the screen
            dgvVehicles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Selection mode: Select whole row, not just one cell
            dgvVehicles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehicles.MultiSelect = false;
        }

        // --- ADD BUTTON ---
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Create the popup form
                using (AddVehicleForm addForm = new AddVehicleForm())
                {
                    // Show it as a modal Dialog (blocks the background window)
                    DialogResult result = addForm.ShowDialog();

                    // If user clicked Save (OK), refresh the list
                    if (result == DialogResult.OK)
                    {
                        LoadVehicles();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Add Vehicle form:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- REFRESH BUTTON ---
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadVehicles();
        }

        // --- DELETE BUTTON ---
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Safety check: Is a row selected?
            if (dgvVehicles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a vehicle to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get ID and Name safely
            int vehicleId = Convert.ToInt32(dgvVehicles.SelectedRows[0].Cells["VehicleId"].Value);
            string make = dgvVehicles.SelectedRows[0].Cells["Make"].Value?.ToString() ?? "Unknown";
            string model = dgvVehicles.SelectedRows[0].Cells["Model"].Value?.ToString() ?? "Vehicle";

            // Confirm with user
            DialogResult result = MessageBox.Show($"Are you sure you want to delete {make} {model}?",
                                                  "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                DeleteVehicle(vehicleId);
            }
        }

        private void DeleteVehicle(int vehicleId)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Basic soft delete (assuming you have an IsActive column) or hard delete
                    // Ideally use a stored procedure like "sp_DeleteVehicle"
                    string query = "UPDATE vehicles SET IsActive = 0 WHERE VehicleId = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", vehicleId);
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Vehicle deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadVehicles(); // Refresh grid
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- EDIT BUTTON (Placeholder) ---
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvVehicles.SelectedRows.Count == 0) return;

            // You can implement EditVehicleForm logic here later
            MessageBox.Show("Edit functionality coming soon!", "Info");
        }

        // --- UI EVENTS ---

        // Enable buttons only when a row is selected
        private void dgvVehicles_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dgvVehicles.SelectedRows.Count > 0;
            if (btnEdit != null) btnEdit.Enabled = hasSelection;
            if (btnDelete != null) btnDelete.Enabled = hasSelection;
        }

        // Double click to trigger Edit
        private void dgvVehicles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (btnEdit != null) btnEdit.PerformClick();
            }
        }
    }
}