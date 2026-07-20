using ProInjuryLogs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjuryLogs.View
{
    public class ConsoleView
    {
        public void DisplayBrandMenu()
        {
            Console.WriteLine("Injury Menu:");
            Console.WriteLine("1. View all records in Injury table");
            Console.WriteLine("2. Update a injury's name by injury_id");
            Console.WriteLine("3. Insert a new brand");
            Console.WriteLine("4. Delete a injury by injury_name");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");
        }
        public void DisplayBrands(List<Injury> brandsList)
        {
            foreach (Injuries brandsObject in brandsList)
            {
                Console.WriteLine($"{brandsObject.InjuryID}, {brandsObject.InjuryName}");
            }
        }
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
            return int.Parse(Console.ReadLine());
        }
    }
}
