using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ProInjuryLogs.Model;

namespace InjuryLogs.controller
{
    public class StorageManager
    {
        private string _connectionString;

        public StorageManager(string connectionString)
        {
            _connectionString = connectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    Console.WriteLine("Connection Successful");
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Invalid connection string or connection already open.");
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to database: {ex.Message}");
            }
        }

        public void CloseConnection()
        {
        }

        public List<Injuries> GetAllInjuries()
        {
            List<Injuries> injuryList = new List<Injuries>();
            string query = "SELECT dbo.Injury.Athlete_ID, dbo.Injury.InjuryType FROM dbo.Injury";
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Injuries injury = new Injuries
                            {
                                InjuryID = reader.GetInt32(0),
                                InjuryName = reader.GetString(1)
                            };
                            injuryList.Add(injury);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving injuries: {ex.Message}");
            }
            return injuryList;
        }

        public int UpdateInjuryName(int injuryId, string injuryName)
        {
            string query = "UPDATE dbo.Injury SET dbo.Injury.InjuryType = @InjuryName WHERE dbo.Injury.Athlete_ID = @InjuryId";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InjuryName", injuryName);
                    cmd.Parameters.AddWithValue("@InjuryId", injuryId);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int InsertInjury(string injuryName)
        {
            string query = "INSERT INTO dbo.Injury (InjuryType) VALUES (@InjuryName);";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InjuryName", injuryName);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int DeleteInjuriesByName(string injuryName)
        {
            string query = "DELETE FROM dbo.Injury WHERE dbo.Injury.InjuryType = @InjuryName";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InjuryName", injuryName);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int ViewInjuriesByDuration(string duration)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Injury WHERE DATEDIFF(day, dbo.Injury.StartDate, dbo.Injury.RecoveryDate) = @Duration", conn))
                {
                    cmd.Parameters.AddWithValue("@Duration", duration);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public DataTable RunReportsQueries(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running report: {ex.Message}");
            }
            return dt;
        }
    }
}