using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace Vehicle_Rental_Management_System.Services
{
    public static class DatabaseHelper
    {
        private static string GetConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString;

            // Fallback for testing
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Server=localhost;Database=vehicle_rental_db;Uid=root;Pwd=;";
            }

            return connectionString;
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(GetConnectionString());
        }

        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection test failed: {ex.Message}");
                return false;
            }
        }

      
        public static int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Execute scalar (return single value)
        public static object ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        // Get DataTable
        public static DataTable GetDataTable(string query, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        // Get DataReader (for large datasets)
        public static MySqlDataReader ExecuteReader(string query, params MySqlParameter[] parameters)
        {
            var conn = GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }



        // Execute stored procedure that returns no data
        public static int ExecuteStoredProcedure(string procedureName, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Execute stored procedure that returns a scalar value
        public static object ExecuteStoredProcedureScalar(string procedureName, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        // Execute stored procedure that returns DataReader
        public static MySqlDataReader ExecuteStoredProcedureReader(string procedureName, params MySqlParameter[] parameters)
        {
            var conn = GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(procedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        // Execute stored procedure that returns DataTable
        public static DataTable ExecuteStoredProcedureTable(string procedureName, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);

                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        // Execute stored procedure with OUTPUT parameters
        public static MySqlParameter[] ExecuteStoredProcedureWithOutput(
            string procedureName,
            MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);

                    cmd.ExecuteNonQuery();

                   
                    return parameters;
                }
            }
        }

       
        public static MySqlDataReader ExecuteAuthProcedure(string username, string password)
        {
            return ExecuteStoredProcedureReader("AuthenticateUser",
                new MySqlParameter("@p_username", username),
                new MySqlParameter("@p_password", password));
        }

        // Execute password change stored procedure
        public static bool ExecuteChangePassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@p_userId", userId),
                    new MySqlParameter("@p_oldPassword", oldPassword),
                    new MySqlParameter("@p_newPassword", newPassword),
                    new MySqlParameter("@p_success", MySqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    }
                };

                ExecuteStoredProcedureWithOutput("ChangeUserPassword", parameters);

                // Get the output parameter value
                var successParam = parameters[3]; // @p_success parameter
                return Convert.ToBoolean(successParam.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Change password error: {ex.Message}");
                return false;
            }
        }

        // ============ HELPER METHODS ============

        // Create parameter for stored procedures
        public static MySqlParameter CreateParameter(string name, object value, MySqlDbType type)
        {
            return new MySqlParameter(name, type)
            {
                Value = value ?? DBNull.Value
            };
        }

        // Create output parameter for stored procedures
        public static MySqlParameter CreateOutputParameter(string name, MySqlDbType type)
        {
            return new MySqlParameter(name, type)
            {
                Direction = ParameterDirection.Output
            };
        }

        
    }
}