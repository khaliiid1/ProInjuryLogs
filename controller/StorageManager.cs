using ProInjuryLogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("Connection Successful"); // if successfull this is the massage that will be displayed.
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Invalid connection string or connection already open.");
            }
            catch (SqlException e)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
                Console.WriteLine($"SQL Error: {e.Message}");
                if (e.Message.Contains("attach an auto-named database")) // these are the possible fixes that could help the databse run correctly.
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
                Console.WriteLine($"Error connecting to database: {ex.Message}");  // if there is an error this is the message that will be displayed.
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
        public List<InjuryLogs> GetAllInjury()
        {
            List<Injury> InjuryList = new List<Injury>();
            string query = "SELECT * FROM Injury";
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Injuries Injury = new Injury
                            {
                                InjuryID = reader.GetInt32(0),
                                InjuryName = reader.GetString(1)
                            };
                            InjuryList.Add(Injury);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving injuries: {ex.Message}");
            }
            return InjuryList;
    }
        public int UpdateInjuryName(int InjuryId, string InjuryName)
        {
            using (SqlCommand cmd = new SqlCommand($"UPDATE Injury SET Injury_NAME = @InjuryName WHERE Injury_ID = @BInjuryID", conn))
            {
                cmd.Parameters.AddWithValue("@InjuryName", InjuryName);
                cmd.Parameters.AddWithValue("@InjuryId", InjuryId);
                return cmd.ExecuteNonQuery();

            }
        }
        public int InsertInjury(string InjuryName)
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO production.Brands (BRAND_NAME) VALUES (@BrandName); SELECT SCOPE_IDENTITY();", conn))
            {
                cmd.Parameters.AddWithValue("@InjuryName", InjuryName);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

    }
    }
}