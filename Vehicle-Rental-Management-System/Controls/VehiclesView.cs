using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Vehicle_Rental_Management_System.Controls
{
    public partial class VehiclesView : UserControl
    {
        public VehiclesView()
        {
            InitializeComponent();
            LoadVehicles();
        }

        // Fetch data from MySQL
        public void LoadVehicles()
        {
            string connString = "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
            if (ConfigurationManager.ConnectionStrings["MySqlConnection"] != null)
                connString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

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

                                // Format columns if they exist
                                if (dgvVehicles.Columns.Contains("DailyRate"))
                                {
                                    dgvVehicles.Columns["DailyRate"].DefaultCellStyle.Format = "C2";
                                }

                                // Hide VehicleId column if it exists
                                if (dgvVehicles.Columns.Contains("VehicleId"))
                                {
                                    dgvVehicles.Columns["VehicleId"].Visible = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading vehicles: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the new form
                Forms.AddVehicleForm addForm = new Forms.AddVehicleForm();

                // If they saved successfully, reload the grid to show the new car
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadVehicles();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Add Vehicle form: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvVehicles.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a vehicle to edit.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the selected vehicle ID
                DataGridViewRow selectedRow = dgvVehicles.SelectedRows[0];

                if (!selectedRow.Cells["VehicleId"].Value.ToString().All(char.IsDigit))
                {
                    MessageBox.Show("Invalid vehicle selection.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int vehicleId = Convert.ToInt32(selectedRow.Cells["VehicleId"].Value);

                // Open edit form (you need to create this form)
                // Note: You'll need to create EditVehicleForm similar to AddVehicleForm
                // Forms.EditVehicleForm editForm = new Forms.EditVehicleForm(vehicleId);

                // For now, show a message that edit is not implemented
                MessageBox.Show("Edit functionality will be implemented soon!\nSelected Vehicle ID: " + vehicleId,
                    "Edit Vehicle", MessageBoxButtons.OK, MessageBoxIcon.Information);

                /*
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadVehicles();
                }
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error editing vehicle: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvVehicles.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a vehicle to delete.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the selected vehicle details
                DataGridViewRow selectedRow = dgvVehicles.SelectedRows[0];

                if (!selectedRow.Cells["VehicleId"].Value.ToString().All(char.IsDigit))
                {
                    MessageBox.Show("Invalid vehicle selection.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int vehicleId = Convert.ToInt32(selectedRow.Cells["VehicleId"].Value);
                string vehicleName = selectedRow.Cells["Model"].Value?.ToString() ?? "Unknown";
                string vehicleMake = selectedRow.Cells["Make"].Value?.ToString() ?? "";

                // Confirm deletion
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete '{vehicleMake} {vehicleName}'?\n\n" +
                    "This action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    DeleteVehicle(vehicleId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error preparing delete: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteVehicle(int vehicleId)
        {
            string connString = "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
            if (ConfigurationManager.ConnectionStrings["MySqlConnection"] != null)
                connString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Check if vehicle has active rentals first
                    string checkQuery = "SELECT COUNT(*) FROM rentals WHERE VehicleId = @VehicleId AND ReturnDate IS NULL";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                        int activeRentals = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (activeRentals > 0)
                        {
                            MessageBox.Show("Cannot delete vehicle with active rentals.\n" +
                                "Please process all returns before deleting.",
                                "Cannot Delete",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Soft delete (set IsActive to 0)
                    string deleteQuery = "UPDATE vehicles SET IsActive = 0 WHERE VehicleId = @VehicleId";

                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@VehicleId", vehicleId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Vehicle deleted successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadVehicles();
                        }
                        else
                        {
                            MessageBox.Show("Vehicle not found or already deleted.", "Info",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (MySqlException mysqlEx)
                {
                    if (mysqlEx.Number == 1451) // Foreign key constraint
                    {
                        MessageBox.Show("Cannot delete vehicle because it has rental history.\n" +
                            "Consider archiving instead.",
                            "Constraint Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Database error: " + mysqlEx.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting vehicle: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvVehicles_SelectionChanged(object sender, EventArgs e)
        {
            // Enable/disable Edit and Delete buttons based on selection
            bool hasSelection = dgvVehicles.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private void VehiclesView_Load(object sender, EventArgs e)
        {
            // Initialize button states
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void dgvVehicles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Double-click to edit
            if (e.RowIndex >= 0)
            {
                btnEdit.PerformClick();
            }
        }
    }
}