using InjuryLogs.controller;
using InjuryLogs.View;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

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
                myView.DisplayBrandMenu();
                string choice = myView.GetInput();
                switch (choice)
                {
                    case "1":
                        ViewAllInjuries(); // this cases allows for actions such as delete add and edit factors to be carried out succesfully.
                        break;
                    case "2":
                        UpdateInjuryName();
                        break;
                    case "3":
                        InsertNewInjury();
                        break;
                    case "4":
                        DeleteInjuryByName();
                        //Need to ensure that can't delete if the linked to an exisisting relationships and catches errors
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        myView.DisplayMessage("Invalid option. Please try again.");
                        break;
                }
            }
            storageManager.CloseConnection();
        }

        private static void InsertInjury()
        {
            throw new NotImplementedException();
        }

        private static void ViewAllInjuries()
        {
            List<Injury> injuryList = storageManager.GetAllInjuries();
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
            int generatedId = storageManager.InsertInjury(injuryName);
            myView.DisplayMessage($"New injury inserted with ID: {generatedId}");
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
                    myView.DisplayMessage($"Rows affected: {rowsAffected}"); // this means that the adding or deletion of injuuries was successful and the injury name provided was found in the database and deleted successfully  
                }
                else
                {
                    myView.DisplayMessage("No injury found with that name."); // if this message appears, it means the injury name provided does not exist in the database  
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) 
                {
                    myView.DisplayMessage("Cannot delete injury because it is referenced by existing products.");
                }
                else
                {
                    myView.DisplayMessage($"SQL Error occurred while deleting injury: {ex.Message}"); // primary error message
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

