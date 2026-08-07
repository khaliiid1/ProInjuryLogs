using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ProInjuryLogs.Model;

namespace InjuryLogs.controller
{
    public class StorageManager
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public StorageManager(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        // Helper method that automatically creates the table if missing
        private void EnsureUsersTableExists(SqlConnection conn)
        {
            string createTableSql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                BEGIN
                    CREATE TABLE dbo.Users (
                        UserID INT IDENTITY(1,1) PRIMARY KEY,
                        Username NVARCHAR(50) NOT NULL UNIQUE,
                        Password NVARCHAR(50) NOT NULL
                    );
                END";

            using (SqlCommand cmd = new SqlCommand(createTableSql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public bool UserExists(string username)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Creates table automatically if missing
                EnsureUsersTableExists(conn);

                string query = "SELECT COUNT(1) FROM dbo.Users WHERE dbo.Users.Username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public bool ValidateUserCredentials(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                EnsureUsersTableExists(conn);

                string query = "SELECT COUNT(1) FROM dbo.Users WHERE dbo.Users.Username = @Username AND dbo.Users.Password = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public int CreateUser(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Creates table automatically if missing
                EnsureUsersTableExists(conn);

                string query = "INSERT INTO dbo.Users (Username, Password) VALUES (@Username, @Password)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Injuries> GetAllInjuries()
        {
            List<Injuries> injuriesList = new List<Injuries>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT dbo.Injury.Injury_ID, dbo.Injury.InjuryType FROM dbo.Injury";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            injuriesList.Add(new Injuries
                            {
                                InjuryID = Convert.ToInt32(reader["Injury_ID"]),
                                InjuryName = reader["InjuryType"].ToString()
                            });
                        }
                    }
                }
            }
            return injuriesList;
        }

        public int UpdateInjuryName(int injuryId, string newName)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE dbo.Injury SET InjuryType = @NewName WHERE dbo.Injury.Injury_ID = @InjuryID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NewName", newName);
                    cmd.Parameters.AddWithValue("@InjuryID", injuryId);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int InsertInjury(string injuryName)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO dbo.Injury (InjuryType) VALUES (@InjuryName)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InjuryName", injuryName);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int DeleteInjuriesByName(string injuryName)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM dbo.Injury WHERE dbo.Injury.InjuryType = @InjuryName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InjuryName", injuryName);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable RunReportsQueries(string query)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
    }
}