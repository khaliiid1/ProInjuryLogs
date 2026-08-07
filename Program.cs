using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using InjuryLogs.controller;
using ProInjuryLogs.Model;
using ConsoleView = ProInjuryLogs.View.ConsoleView;

namespace ProInjuryLogs
{
    internal class Program
    {
        private static StorageManager storageManager;
        private static ConsoleView myView;

        static void Main(string[] args)
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=InjuryLogs;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

            storageManager = new StorageManager(connectionString);
            myView = new ConsoleView();

            bool exit = false;
            while (!exit)
            {
                myView.DisplayMessage("=== Welcome to Pro Injury Logs ===");
                myView.DisplayMessage("1. Login");
                myView.DisplayMessage("2. Create Account");
                myView.DisplayMessage("3. Exit");

                string mainChoice = myView.GetInput();

                switch (mainChoice)
                {
                    case "1":
                        HandleLogin();
                        break;

                    case "2":
                        CreateAccount();
                        break;

                    case "3":
                        exit = true;
                        break;

                    default:
                        myView.DisplayMessage("Invalid option. Please enter 1, 2, or 3.\n");
                        break;
                }
            }

            storageManager.CloseConnection();
            myView.DisplayMessage("Goodbye!");
        }

        private static void HandleLogin()
        {
            myView.DisplayMessage("\n=== Account Login ===");

            string username = GetValidatedInput("Please enter your username:", minLength: 3, maxLength: 20);
            string password = GetValidatedInput("Please enter your password:", minLength: 5, maxLength: 15);

            // 1. Check if user exists
            if (!storageManager.UserExists(username))
            {
                myView.DisplayMessage("\nError: Username does not exist. Please create an account first.\n");
                return;
            }

            // 2. Validate password
            bool isValidCredentials = storageManager.ValidateUserCredentials(username, password);

            if (!isValidCredentials)
            {
                myView.DisplayMessage("\nError: Incorrect password. Access denied.\n");
                return;
            }

            // 3. Select Role after successful login
            myView.DisplayMessage($"\nWelcome back, {username}! Please select your role:");
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
                    myView.DisplayMessage("Invalid option. Returning to main menu.\n");
                    break;
            }
        }

        private static void CreateAccount()
        {
            myView.DisplayMessage("\n=== Create New Account ===");

            string username;
            while (true)
            {
                username = GetValidatedInput("Enter new username (3-20 characters, no spaces):", minLength: 3, maxLength: 20);
                if (username.Contains(" "))
                {
                    myView.DisplayMessage("Error: Username cannot contain spaces. Try again.\n");
                    continue;
                }

                if (storageManager.UserExists(username))
                {
                    myView.DisplayMessage("Error: Username already exists. Please choose a different one.\n");
                    continue;
                }

                break;
            }

            string password;
            while (true)
            {
                password = GetValidatedInput("Enter new password (6-15 characters):", minLength: 6, maxLength: 15);

                myView.DisplayMessage("Confirm your password:");
                string confirmPassword = myView.GetInput()?.Trim() ?? string.Empty;

                if (password != confirmPassword)
                {
                    myView.DisplayMessage("Error: Passwords do not match. Try again.\n");
                }
                else
                {
                    break;
                }
            }

            try
            {
                int rows = storageManager.CreateUser(username, password);
                if (rows > 0)
                {
                    myView.DisplayMessage($"\nAccount successfully created and saved for '{username}'!");

                    myView.DisplayMessage("\nWould you like to log in now? (1 = Yes, 2 = No)");
                    string choice = myView.GetInput();
                    if (choice == "1")
                    {
                        HandleLogin();
                    }
                }
                else
                {
                    myView.DisplayMessage("\nFailed to create account. Please try again.");
                }
            }
            catch (Exception ex)
            {
                myView.DisplayMessage($"\nError saving account to database: {ex.Message}");
            }
        }

        private static string GetValidatedInput(string prompt, int minLength, int maxLength)
        {
            while (true)
            {
                myView.DisplayMessage(prompt);
                string input = myView.GetInput()?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    myView.DisplayMessage("Error: Input cannot be empty. Try again.\n");
                }
                else if (input.Length < minLength)
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
                        CreateAccount();
                        break;
                    case "4":
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
                myView.DisplayMessage($"\n--- Report {choice} Results ({result.Rows.Count} rows) ---");
                myView.DisplayDataTable(result);
            }
        }
    }
}