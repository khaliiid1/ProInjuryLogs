using Microsoft.Data.SqlClient;
using ProInjuryLogs.Model;
using System;
using System.Collections.Generic;

namespace InjuryLogs.controller
{
    public class StorageManager
    {
        private SqlConnection conn;

        public StorageManager(string connectionString)
        {
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                Console.WriteLine("Connection Successful");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Invalid connection string or connection already open.");
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
                if (e.Message.Contains("attach an auto-named database"))
                {
                    Console.WriteLine("Fix: Database is already attached OR file is in use.");
                    Console.WriteLine("Try this:");
                    Console.WriteLine("1. Remove AttachDbFilename from connection string");
                    Console.WriteLine("2. Use Initial Catalog instead");
                    Console.WriteLine("3. Or delete/rename duplicate DB in SQL Server");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to database: {ex.Message}");
            }
        }

        public void CloseConnection()
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
                Console.WriteLine("Connection Closed");
            }
        }

        public List<Injuries> GetAllInjuries()
        {
            List<Injuries> injuryList = new List<Injuries>();
            string query = "SELECT Athlete_ID, InjuryType FROM dbo.Injury";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
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
            string query = "UPDATE dbo.Injury SET InjuryType = @InjuryName WHERE Athlete_ID = @InjuryId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@InjuryName", injuryName);
                cmd.Parameters.AddWithValue("@InjuryId", injuryId);
                return cmd.ExecuteNonQuery();
            }
        }

        public int InsertInjury(string injuryName)
        {
            string query = "INSERT INTO dbo.Injury (InjuryType) VALUES (@InjuryName);";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@InjuryName", injuryName);
                return cmd.ExecuteNonQuery();
            }
        }

        public int DeleteInjuriesByName(string injuryName)
        {
            string query = "DELETE FROM dbo.Injury WHERE InjuryType = @InjuryName";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@InjuryName", injuryName);
                return cmd.ExecuteNonQuery();
            }
        }

        public int ViewInjuriesByDuration(string duration)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Injury WHERE DATEDIFF(day, StartDate, RecoveryDate) = @Duration", conn))
            {
                cmd.Parameters.AddWithValue("@Duration", duration);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public SqlDataReader RunReportsQueries(string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            return cmd.ExecuteReader();
        }
    }
}