using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryTracker
{
    class AssignItems
    {
        public static List<Product> AssignResponsibleParty(List<Product> receipt, List<User> users)
        {
            Console.Clear();
            Console.WriteLine("Separate the check!\n" +
                "It's time to assign each item to individual users\n");

            int count = 0;
            foreach (User u in users)
            {
                count++;
                Console.WriteLine("Users to choose from: " + count + " " + u.Name);
            }

            try
            {
                foreach (Product p in receipt)
                {
                    Console.Write("Assign the following item: " + p.ProductName +
                        "\n\tEnter number for the user (listed above):  ");
                    int userSelection = Convert.ToInt32(Console.ReadLine());
                    p.ResponsibleParty = users[userSelection - 1];
                }
            }
            catch
            {
                Console.WriteLine("Invalid input.\nI will eventually add in error handling, but this is as good as it gets for now.\nYou have to start over.");
                Environment.Exit(0); 
            }
            return receipt; 
        }
    }
}
