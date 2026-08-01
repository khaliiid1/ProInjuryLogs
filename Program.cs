using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using InjuryLogs.controller;
using ProInjuryLogs.Model;
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

                string name = GetValidatedInput("Please enter your name:", minLength: 2, maxLength: 20);
                string password = GetValidatedInput("Please enter your password:", minLength: 5, maxLength: 15);

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
                        myView.DisplayMessage("Role selected: User\n");
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

        private static string GetValidatedInput(string prompt, int minLength, int maxLength)
        {
            while (true)
            {
                myView.DisplayMessage(prompt);
                string input = myView.GetInput()?.Trim() ?? string.Empty;

                if (input.Length < minLength)
                {
                    myView.DisplayMessage($"Error: Input must be at least {minLength} characters long. Try again.\n");
                }
                else if (input.Length > maxLength)
                {
                    myView.DisplayMessage($"Error: Input cannot exceed {maxLength} characters (you entered {input.Length}). Try again.\n");
                }
                else
                {
                    return input;
                }
            }
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
                        DisplayReportsMenu();
                        break;
                    case "6":
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
                        DisplayReportsMenu();
                        break;
                    case "3":
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
            List<Injuries> injuryList = storageManager.GetAllInjuries();
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

            myView.DisplayMessage("\nSaved!");

            while (true)
            {
                myView.DisplayMessage("\nPlease choose an option:");
                myView.DisplayMessage("1. Return to Main Menu");
                myView.DisplayMessage("2. Leave (Close Program)");

                string choice = myView.GetInput();

                if (choice == "1")
                {
                    return;
                }
                else if (choice == "2")
                {
                    myView.DisplayMessage("Goodbye!");
                    Environment.Exit(0);
                }
                else
                {
                    myView.DisplayMessage("Please select a valid option");
                }
            }
        }

        private static void DeleteInjuryByName()
        {
            myView.DisplayMessage("Enter the injury name to delete: ");
            string injuryName = myView.GetInput();

            try
            {
                int rowsAffected = storageManager.DeleteInjuriesByName(injuryName);
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
                    myView.DisplayMessage("Cannot delete injury because it is referenced by existing logs.");
                }
                else
                {
                    myView.DisplayMessage($"SQL Error occurred while deleting injury: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                myView.DisplayMessage($"Error occurred while deleting injury: {ex.Message}");
            }
        }

        private static void DisplayReportsMenu()
        {
            myView.DisplayMessage("\n--- Select Report (1-15) ---");
            string input = myView.GetInput();

            if (int.TryParse(input, out int reportChoice) && reportChoice >= 1 && reportChoice <= 15)
            {
                RunSelectedReport(reportChoice);
            }
            else
            {
                myView.DisplayMessage("Invalid report selection.");
            }
        }

        private static void RunSelectedReport(int choice)
        {
            string? query = choice switch
            {
                1 => "SELECT DISTINCT TeamName FROM dbo.Athletes WHERE Sports = 'Football';",
                2 => "SELECT dbo.Athletes.FirstName, dbo.Athletes.LastName, dbo.Injury.InjuryType FROM dbo.Athletes JOIN dbo.Injury ON dbo.Athletes.Athlete_ID = dbo.Injury.Athlete_ID WHERE dbo.Injury.InjuryType LIKE '%Knee%';",
                3 => "SELECT LastName, FirstName, Sports, TeamName FROM dbo.Athletes ORDER BY TeamName ASC;",
                4 => "SELECT dbo.Athletes.LastName, dbo.Athletes.FirstName, dbo.Injury.InjuryType, dbo.Injury.StartDate, dbo.Injury.RecoveryDate FROM dbo.Athletes JOIN dbo.Injury ON dbo.Athletes.Athlete_ID = dbo.Injury.Athlete_ID WHERE dbo.Athletes.Athlete_ID = 5;",
                5 => "SELECT dbo.Athletes.LastName, dbo.Athletes.FirstName, dbo.Injury.StartDate, dbo.Injury.RecoveryDate FROM dbo.Athletes JOIN dbo.Injury ON dbo.Athletes.Athlete_ID = dbo.Injury.Athlete_ID WHERE dbo.Injury.StartDate >= '2026-01-01' AND dbo.Injury.StartDate <= '2026-02-13';",
                6 => "SELECT SportName, LeagueName, TeamsCount FROM dbo.Sports ORDER BY SportName ASC;",
                7 => "SELECT TeamName, LeagueName, InjuredPlayerCount FROM dbo.Team ORDER BY InjuredPlayerCount DESC, TeamName ASC;",
                8 => "SELECT InjuryType, DATEDIFF(day, StartDate, RecoveryDate) AS DaysDuration FROM dbo.Injury;",
                9 => "SELECT dbo.Athletes.FirstName, dbo.Athletes.LastName, dbo.Injury.InjuryType FROM dbo.Athletes JOIN dbo.Injury ON dbo.Athletes.Athlete_ID = dbo.Injury.Athlete_ID;",
                10 => "SELECT FirstName, LastName, TeamName FROM dbo.Athletes WHERE LastName LIKE 'Smit%';",
                11 => "SELECT SportName, LeagueName, ManagersCount FROM dbo.Sports ORDER BY ManagersCount DESC;",
                12 => "SELECT SportName, LeagueName, ManagersCount FROM dbo.Sports WHERE ManagersCount < 10 ORDER BY ManagersCount DESC;",
                13 => "SELECT FirstName, LastName, Phone, TeamName FROM dbo.Athletes WHERE TeamName LIKE 'Warrior%';",
                14 => "SELECT FirstName, LastName, Sports, TeamName FROM dbo.Athletes WHERE LastName LIKE 'S%';",
                15 => "SELECT Athlete_ID, FirstName, LastName, Phone, TeamName FROM dbo.Athletes WHERE LastName NOT LIKE 'S%';",
                _ => null
            };

            if (query != null)
            {
                DataTable result = storageManager.RunReportsQueries(query);
                myView.DisplayMessage($"Report {choice} executed. Returned {result.Rows.Count} rows.");
            }
        }
    }
}