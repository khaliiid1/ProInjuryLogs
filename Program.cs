using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using InjuryLogs.controller;
using ProInjuryLogs.View;

namespace ProInjuryLogs
{
    internal class Program
    {
        private static StorageManager storageManager;
        private static ConsoleView myView;

        static void Main(string[] args)
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=BikeStores;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

            storageManager = new StorageManager(connectionString);
            myView = new ConsoleView();

            bool exit = false;
            while (!exit)
            {
                myView.DisplayMessage("=== Welcome to Pro Injury Logs ===");
                Console.WriteLine("Please enter your name (or 'exit' to quit):");
                string name = myView.GetInput();

                if (name.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    exit = true;
                    break;
                }

                myView.DisplayMessage("Please enter your password:");
                string password = myView.GetInput();

                if (string.IsNullOrWhiteSpace(password))
                {
                    myView.DisplayMessage("Password is required. Please try again.\n");
                    continue; 
                }

                Console.WriteLine($"\nHello, {name}! Please select your role:");
                myView.DisplayMessage("1. Admin");
                myView.DisplayMessage("2. User");

                string input = myView.GetInput();

                switch (input)
                {
                    case "1":
                        myView.DisplayMessage("Role selected: Admin\n");
                        DisplayAdminMenu();
                        break;

                    case "2":
                        myView.DisplayMessage("Role selected: EndUser\n");
                        DisplayUserMenu();
                        break;

                    default:
                        myView.DisplayMessage("Invalid option. Please enter 1 or 2.\n");
                        break;
                }
            }

            storageManager.CloseConnection();
            myView.DisplayMessage("Goodbye!");
        }

        private static void DisplayAdminMenu()
        {
            bool exitAdmin = false;
            while (!exitAdmin)
            {
                myView.AdminMenu();
                string choice = myView.GetInput();

                switch (choice)
                {
                    case "1":
                        ViewAllInjuries();
                        break;
                    case "2":
                        UpdateInjuryName();
                        break;
                    case "3":
                        InsertNewInjury();
                        break;
                    case "4":
                        DeleteInjuryByName();
                        break;
                    case "5":
                        exitAdmin = true;
                        break;
                    default:
                        myView.DisplayMessage("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static void DisplayUserMenu()
        {
            bool exitUser = false;
            while (!exitUser)
            {
                myView.UserMenu();
                string choice = myView.GetInput();

                switch (choice)
                {
                    case "1":
                        ViewAllInjuries();
                        break;
                    case "2":
                    
                        exitUser = true;
                        break;
                    default:
                        myView.DisplayMessage("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static void ViewAllInjuries()
        {
            List<ProInjuryLogs.Model.Injuries> injuryList = storageManager.GetAllInjuries();
            myView.DisplayInjuries(injuryList);
        }

        private static void UpdateInjuryName()
        {
            myView.DisplayMessage("Enter the injury_id to update: ");
            int injuryId = myView.GetIntInput();
            myView.DisplayMessage("Enter the new injury name: ");
            string injuryName = myView.GetInput();
            int rowsAffected = storageManager.UpdateInjuryName(injuryId, injuryName);
            myView.DisplayMessage($"Rows affected: {rowsAffected}");
        }

        private static void InsertNewInjury()
        {
            myView.DisplayMessage("Enter the new injury name: ");
            string injuryName = myView.GetInput();
            int rowsAffected = storageManager.InsertInjury(injuryName);
            myView.DisplayMessage($"Rows affected: {rowsAffected}");
        }

        private static void DeleteInjuryByName()
        {
            int rowsAffected;
            myView.DisplayMessage("Enter the injury name to delete: ");
            string injuryName = myView.GetInput();
            try
            {
                rowsAffected = storageManager.DeleteInjuriesByName(injuryName);
                if (rowsAffected > 0)
                {
                    myView.DisplayMessage($"Rows affected: {rowsAffected}");
                }
                else
                {
                    myView.DisplayMessage("No injury found with that name.");
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    myView.DisplayMessage("Cannot delete injury because it is referenced by existing injury.");
                }
                else
                {
                    myView.DisplayMessage($"SQL Error occurred while deleting injury: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                myView.DisplayMessage($"Error occurred while deleting injury: {ex.Message}");
                Console.ReadKey();
            }
        }

        private static bool ValidateInput(string input, int maxLength, int minLength, char validationType)
        {
            if (input == null || input.Length < minLength || input.Length > maxLength)
            {
                return false;
            }

            foreach (char c in input)
            {
                switch (validationType)
                {
                    case 'N': // Numeric
                        if (!char.IsDigit(c))
                        {
                            myView.DisplayMessage("Input must be numeric.");
                            return false;
                        }
                        break;
                    case 'A': // Alphabetic
                        if (!char.IsLetter(c))
                        {
                            myView.DisplayMessage("Input must be alphabetic.");
                            return false;
                        }
                        break;
                    case 'M': // Alphanumeric
                        if (!char.IsLetterOrDigit(c))
                        {
                            myView.DisplayMessage("Input must be alphanumeric.");
                            return false;
                        }
                        break;
                }
            }

            return true;
        }

        private static void RunSelectedReport(int choice)
        {
            string query = "";

            switch (choice)
            {
                case 1:
                    query = "SELECT DISTINCT TeamName FROM dbo.Athletes WHERE Sports = 'Football';";
                    break;

                case 2:
                    query = "SELECT Athletes.FirstName, Athletes.LastName, Injury.InjuryType FROM dbo.Athletes JOIN dbo.Injury ON Athletes.Athlete_ID = Injury.Athlete_ID WHERE Injury.InjuryType LIKE '%Knee%';";
                    break;

                case 3:
                    query = "SELECT LastName, FirstName, Sports, TeamName FROM dbo.Athletes ORDER BY TeamName ASC;";
                    break;

                case 4:
                    query = "SELECT Athletes.LastName, Athletes.FirstName, Injury.InjuryType, Injury.StartDate, Injury.RecoveryDate FROM dbo.Athletes JOIN dbo.Injury ON Athletes.Athlete_ID = Injury.Athlete_ID WHERE Athletes.Athlete_ID = 5;";
                    break;

                case 5:
                    query = "SELECT Athletes.LastName, Athletes.FirstName, Injury.StartDate, Injury.RecoveryDate FROM dbo.Athletes JOIN dbo.Injury ON Athletes.Athlete_ID = Injury.Athlete_ID WHERE Injury.StartDate >= '2026-01-01' AND Injury.StartDate <= '2026-02-13';";
                    break;

                case 6:
                    query = "SELECT SportName, LeagueName, TeamsCount FROM dbo.Sports ORDER BY SportName ASC;";
                    break;

                case 7:
                    query = "SELECT TeamName, LeagueName, InjuredPlayerCount FROM dbo.Team ORDER BY InjuredPlayerCount DESC, TeamName ASC;";
                    break;

                case 8:
                    query = "SELECT InjuryType, DATEDIFF(day, StartDate, RecoveryDate) AS DaysDuration FROM dbo.Injury;";
                    break;

                case 9:
                    query = "SELECT Athletes.FirstName, Athletes.LastName, Injury.InjuryType FROM dbo.Athletes JOIN dbo.Injury ON Athletes.Athlete_ID = Injury.Athlete_ID;";
                    break;

                case 10:
                    query = "SELECT FirstName, LastName, TeamName FROM dbo.Athletes WHERE LastName LIKE 'Smit%';";
                    break;

                case 11:
                    query = "SELECT SportName, LeagueName, ManagersCount FROM dbo.Sports ORDER BY ManagersCount DESC;";
                    break;

                case 12:
                    query = "SELECT SportName, LeagueName, ManagersCount FROM dbo.Sports WHERE ManagersCount < 10 ORDER BY ManagersCount DESC;";
                    break;

                case 13:
                    query = "SELECT FirstName, LastName, Phone, TeamName FROM dbo.Athletes WHERE TeamName LIKE 'Warrior%';";
                    break;

                case 14:
                    query = "SELECT FirstName, LastName, Sports, TeamName FROM dbo.Athletes WHERE LastName LIKE 'S%';";
                    break;

                case 15:
                    query = "SELECT Athlete_ID, FirstName, LastName, Phone, TeamName FROM dbo.Athletes WHERE LastName NOT LIKE 'S%';";
                    break;

                default:
                    myView.DisplayMessage("Invalid report choice.");
                    return; 
            }

            
      
        }
    }
}