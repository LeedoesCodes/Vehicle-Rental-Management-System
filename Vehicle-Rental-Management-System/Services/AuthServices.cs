using System;
using System.Data;
using MySql.Data.MySqlClient;
using Vehicle_Rental_Management_System.Models;

namespace Vehicle_Rental_Management_System.Services
{
    public class AuthService
    {
        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            try
            {
                // Using stored procedure instead of inline SQL
                using (var reader = ExecuteStoredProcedure(
                    "AuthenticateUser",
                    new MySqlParameter("@p_username", username),
                    new MySqlParameter("@p_password", password) // Plain text comparison
                ))
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            Username = reader["Username"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            Role = reader["Role"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication error: {ex.Message}");
                // Log the error properly in production
            }

            return null;
        }

        public bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                // Using stored procedure with output parameter
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    using (var cmd = new MySqlCommand("ChangeUserPassword", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        cmd.Parameters.AddWithValue("@p_userId", userId);
                        cmd.Parameters.AddWithValue("@p_oldPassword", oldPassword);
                        cmd.Parameters.AddWithValue("@p_newPassword", newPassword);

                        // Output parameter
                        var successParam = new MySqlParameter("@p_success", MySqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(successParam);

                        cmd.ExecuteNonQuery();

                        // Get the output parameter value
                        return Convert.ToBoolean(successParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Change password error: {ex.Message}");
                return false;
            }
        }

        // Alternative method using different approach
        public User AuthenticateUsingSP(string username, string password)
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    using (var cmd = new MySqlCommand("AuthenticateUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_username", username);
                        cmd.Parameters.AddWithValue("@p_password", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Username = reader["Username"].ToString(),
                                    PasswordHash = reader["PasswordHash"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Phone = reader["Phone"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication error: {ex.Message}");
            }

            return null;
        }

        // Helper method to execute stored procedures that return data
        private MySqlDataReader ExecuteStoredProcedure(string procedureName, params MySqlParameter[] parameters)
        {
            var conn = DatabaseHelper.GetConnection();
            var cmd = new MySqlCommand(procedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}