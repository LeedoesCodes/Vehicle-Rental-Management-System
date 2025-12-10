using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using Vehicle_Rental_Management_System.Models;

namespace Vehicle_Rental_Management_System.Services
{
    public class VehicleService
    {
        // Get all vehicles with category names using stored procedure
        public List<Vehicle> GetAllVehicles()
        {
            var vehicles = new List<Vehicle>();

            try
            {
                using (var reader = DatabaseHelper.ExecuteStoredProcedureReader("GetAllVehiclesWithCategories"))
                {
                    while (reader.Read())
                    {
                        vehicles.Add(MapToVehicle(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllVehicles error: {ex.Message}");
            }

            return vehicles;
        }

        // Get vehicle by ID using stored procedure
        public Vehicle GetVehicleById(int vehicleId)
        {
            try
            {
                using (var reader = DatabaseHelper.ExecuteStoredProcedureReader(
                    "GetVehicleByIdWithCategory",
                    DatabaseHelper.CreateParameter("@p_vehicleId", vehicleId, MySqlDbType.Int32)))
                {
                    if (reader.Read())
                    {
                        return MapToVehicle(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetVehicleById error: {ex.Message}");
            }

            return null;
        }

        // Add new vehicle using stored procedure
        public bool AddVehicle(Vehicle vehicle)
        {
            try
            {
                int result = DatabaseHelper.ExecuteStoredProcedure("AddVehicle",
                    DatabaseHelper.CreateParameter("@p_make", vehicle.Make, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_model", vehicle.Model, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_year", vehicle.Year, MySqlDbType.Int32),
                    DatabaseHelper.CreateParameter("@p_color", vehicle.Color, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_licensePlate", vehicle.LicensePlate, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_vin", vehicle.VIN, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_categoryName", vehicle.Category, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_transmission", vehicle.Transmission, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_fuelType", vehicle.FuelType, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_seatingCapacity", vehicle.SeatingCapacity, MySqlDbType.Int32),
                    DatabaseHelper.CreateParameter("@p_currentMileage", vehicle.CurrentMileage, MySqlDbType.Decimal),
                    DatabaseHelper.CreateParameter("@p_status", vehicle.Status, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_dailyRate", vehicle.DailyRate, MySqlDbType.Decimal),
                    DatabaseHelper.CreateParameter("@p_imagePath", vehicle.ImagePath, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_features", vehicle.Features, MySqlDbType.Text));

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddVehicle error: {ex.Message}");
                return false;
            }
        }

        // Update vehicle using stored procedure
        public bool UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                int result = DatabaseHelper.ExecuteStoredProcedure("UpdateVehicle",
                    DatabaseHelper.CreateParameter("@p_vehicleId", vehicle.VehicleId, MySqlDbType.Int32),
                    DatabaseHelper.CreateParameter("@p_make", vehicle.Make, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_model", vehicle.Model, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_year", vehicle.Year, MySqlDbType.Int32),
                    DatabaseHelper.CreateParameter("@p_color", vehicle.Color, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_licensePlate", vehicle.LicensePlate, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_vin", vehicle.VIN, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_categoryName", vehicle.Category, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_transmission", vehicle.Transmission, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_fuelType", vehicle.FuelType, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_seatingCapacity", vehicle.SeatingCapacity, MySqlDbType.Int32),
                    DatabaseHelper.CreateParameter("@p_currentMileage", vehicle.CurrentMileage, MySqlDbType.Decimal),
                    DatabaseHelper.CreateParameter("@p_status", vehicle.Status, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_dailyRate", vehicle.DailyRate, MySqlDbType.Decimal),
                    DatabaseHelper.CreateParameter("@p_imagePath", vehicle.ImagePath, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_features", vehicle.Features, MySqlDbType.Text));

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateVehicle error: {ex.Message}");
                return false;
            }
        }

        // Delete vehicle using stored procedure
        public bool DeleteVehicle(int vehicleId)
        {
            try
            {
                int result = DatabaseHelper.ExecuteStoredProcedure("DeleteVehicle",
                    DatabaseHelper.CreateParameter("@p_vehicleId", vehicleId, MySqlDbType.Int32));

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteVehicle error: {ex.Message}");
                return false;
            }
        }

        // Search vehicles using stored procedure
        public List<Vehicle> SearchVehicles(string searchTerm, string status = null)
        {
            var vehicles = new List<Vehicle>();

            try
            {
                using (var reader = DatabaseHelper.ExecuteStoredProcedureReader("SearchVehicles",
                    DatabaseHelper.CreateParameter("@p_searchTerm", searchTerm, MySqlDbType.VarChar),
                    DatabaseHelper.CreateParameter("@p_status", status, MySqlDbType.VarChar)))
                {
                    while (reader.Read())
                    {
                        vehicles.Add(MapToVehicle(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SearchVehicles error: {ex.Message}");
            }

            return vehicles;
        }

        // Get available vehicles for dates using stored procedure
        public List<Vehicle> GetAvailableVehicles(DateTime startDate, DateTime endDate)
        {
            var vehicles = new List<Vehicle>();

            try
            {
                using (var reader = DatabaseHelper.ExecuteStoredProcedureReader("GetAvailableVehicles",
                    DatabaseHelper.CreateParameter("@p_startDate", startDate, MySqlDbType.Date),
                    DatabaseHelper.CreateParameter("@p_endDate", endDate, MySqlDbType.Date)))
                {
                    while (reader.Read())
                    {
                        vehicles.Add(MapToVehicle(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAvailableVehicles error: {ex.Message}");
            }

            return vehicles;
        }

        // Get all categories using stored procedure
        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            try
            {
                using (var reader = DatabaseHelper.ExecuteStoredProcedureReader("GetAllCategories"))
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            BaseDailyRate = reader["BaseDailyRate"] != DBNull.Value ?
                                           Convert.ToDecimal(reader["BaseDailyRate"]) : 0,
                            Description = reader["Description"] != DBNull.Value ?
                                         reader["Description"].ToString() : ""
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllCategories error: {ex.Message}");
            }

            return categories;
        }

        // Get vehicle statistics using stored procedure
        public Dictionary<string, int> GetVehicleStatistics()
        {
            var stats = new Dictionary<string, int>();

            try
            {
                using (var reader = DatabaseHelper.ExecuteStoredProcedureReader("GetVehicleStatistics"))
                {
                    while (reader.Read())
                    {
                        string status = reader["Status"].ToString();
                        int count = Convert.ToInt32(reader["Count"]);
                        stats[status] = count;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetVehicleStatistics error: {ex.Message}");
            }

            return stats;
        }

        // Update vehicle status using stored procedure
        public bool UpdateVehicleStatus(int vehicleId, string status)
        {
            try
            {
                int result = DatabaseHelper.ExecuteStoredProcedure("UpdateVehicleStatus",
                    DatabaseHelper.CreateParameter("@p_vehicleId", vehicleId, MySqlDbType.Int32),
                    DatabaseHelper.CreateParameter("@p_status", status, MySqlDbType.VarChar));

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateVehicleStatus error: {ex.Message}");
                return false;
            }
        }

        // Get vehicles by category using stored procedure
        public List<Vehicle> GetVehiclesByCategory(string categoryName)
        {
            var vehicles = new List<Vehicle>();

            try
            {
                using (var reader = DatabaseHelper.ExecuteStoredProcedureReader("GetVehiclesByCategory",
                    DatabaseHelper.CreateParameter("@p_categoryName", categoryName, MySqlDbType.VarChar)))
                {
                    while (reader.Read())
                    {
                        vehicles.Add(MapToVehicle(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetVehiclesByCategory error: {ex.Message}");
            }

            return vehicles;
        }

        // Helper method to map DataReader to Vehicle object
        private Vehicle MapToVehicle(MySqlDataReader reader)
        {
            return new Vehicle
            {
                VehicleId = Convert.ToInt32(reader["VehicleId"]),
                Make = reader["Make"].ToString(),
                Model = reader["Model"].ToString(),
                Year = Convert.ToInt32(reader["Year"]),
                Color = reader["Color"].ToString(),
                LicensePlate = reader["LicensePlate"].ToString(),
                VIN = reader["VIN"] != DBNull.Value ? reader["VIN"].ToString() : null,
                Category = reader["Category"] != DBNull.Value ? reader["Category"].ToString() : null,
                Transmission = reader["Transmission"].ToString(),
                FuelType = reader["FuelType"].ToString(),
                SeatingCapacity = Convert.ToInt32(reader["SeatingCapacity"]),
                CurrentMileage = Convert.ToDecimal(reader["CurrentMileage"]),
                Status = reader["Status"].ToString(),
                DailyRate = Convert.ToDecimal(reader["DailyRate"]),
                ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : null,
                Features = reader["Features"] != DBNull.Value ? reader["Features"].ToString() : null,
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
            };
        }
    }

    // Category class definition (kept inside Services namespace)
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal BaseDailyRate { get; set; }
        public string Description { get; set; }
    }
}