using InjuryLogs.controller;
using ProInjuryLogs.View;
using Microsoft.Data.SqlClient;

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
                myView.DisplayMessage("Welcome to the Pro injurylogs");
                myView.DisplayMessage("please enter 1. for admin");
                myView.DisplayMessage("please enter 2. for user");

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
                        myView.DisplayMessage("Invalid option. Please enter 1 or 2.");
                        break;
                }
            }

            storageManager.CloseConnection();
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
    }
}