using System;
using System.Collections.Generic;
using System.Data;
using ProInjuryLogs.Model;

namespace ProInjuryLogs.View
{
    public class ConsoleView
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public string GetInput()
        {
            return Console.ReadLine();
        }

        public int GetIntInput()
        {
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                return result;
            }
            return 0;
        }

        internal void DisplayInjuries(List<Injuries> injuryList)
        {
            if (injuryList == null || injuryList.Count == 0)
            {
                Console.WriteLine("\nNo injuries found.\n");
                return;
            }

            Console.WriteLine("\n--- All Injuries ---");
            foreach (var injury in injuryList)
            {
                Console.WriteLine($"ID: {injury.InjuryID}, Name: {injury.InjuryName}");
            }
            Console.WriteLine();
        }

        internal void DisplayInjuryMenu()
        {
            Console.WriteLine("\n--- Injury Menu ---");
            Console.WriteLine("1. View All Injuries");
            Console.WriteLine("2. Back");
        }

        public void AdminMenu()
        {
            Console.WriteLine("\n--- ADMIN MENU ---");
            Console.WriteLine("1. View All Injuries");
            Console.WriteLine("2. Update Injury Name");
            Console.WriteLine("3. Insert New Injury");
            Console.WriteLine("4. Delete Injury By Name");
            Console.WriteLine("5. View Reports");
            Console.WriteLine("6. Back to Role Selection");
        }

        public void UserMenu()
        {
            Console.WriteLine("\n--- USER MENU ---");
            Console.WriteLine("1. View All Injuries");
            Console.WriteLine("2. View Reports");
            Console.WriteLine("3. Create New Account");
            Console.WriteLine("4. Back to Role Selection");
        }

        internal void DisplayDataTable(DataTable result)
        {
            if (result == null || result.Rows.Count == 0)
            {
                Console.WriteLine("\nNo data found to display.\n");
                return;
            }

            Console.WriteLine();

            for (int columnIndex = 0; columnIndex < result.Columns.Count; columnIndex++)
            {
                Console.Write($"{result.Columns[columnIndex].ColumnName,-20}");
            }
            Console.WriteLine();

            Console.WriteLine(new string('-', result.Columns.Count * 20));

            for (int rowIndex = 0; rowIndex < result.Rows.Count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < result.Columns.Count; columnIndex++)
                {
                    string cellValue = result.Rows[rowIndex][columnIndex]?.ToString() ?? string.Empty;
                    Console.Write($"{cellValue,-20}");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}