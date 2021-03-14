using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryTracker
{
    class AssignItems
    {
        // check to make sure all items and prices are correct 
        public static List<Product> ConfirmCorrectInformation(List<Product> receipt)
        {
            foreach (Product p in receipt)
            {
                Console.WriteLine(p.PurchaseDate + " " + p.ProductNumber + " " + p.ProductPrice + " " + p.ProductName);
            }
            Console.Write("Is this information correct? Y/N ");
            string userInput = Console.ReadLine().ToLower();

            if(userInput == "y")
            {
                // call cycle through products method 

            } else if(userInput == "n")
            {
                receipt = CorrectReceiptInformation(receipt);
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again");
                ConfirmCorrectInformation(receipt);
            }
            
            return receipt;
        }

        // incorrect info correction 
        private static List<Product> CorrectReceiptInformation(List<Product> receipt)
        {
            int incorrectCategory = -1; 

            while (incorrectCategory != 5)
            {
                //prompt for incorrect info 
                Console.WriteLine("\nPlease select the incorrect info category: \n" +
                    "\t1. Purchase Date\n" +
                    "\t2. Product Number\n" +
                    "\t3. Product Name\n" +
                    "\t4. Product Price\n" +
                    "\t5. All information is correct\n");
                incorrectCategory = Convert.ToInt32(Console.ReadLine());
               
                switch (incorrectCategory)
                {
                    case 1:
                        Console.WriteLine("Please enter the incorrect purchase date: ");
                        string incorrectInfo = Console.ReadLine();

                        Console.WriteLine("What should it be? ");
                        string correctInfo = Console.ReadLine();

                        var found = receipt.Where(item => item.PurchaseDate == incorrectInfo);
                        foreach (var f in found)
                        {
                            f.PurchaseDate = correctInfo;
                        }
                        break;

                    case 2:
                        Console.WriteLine("Please enter the incorrect product number: ");
                        incorrectInfo = Console.ReadLine();

                        Console.WriteLine("What should it be? ");
                        correctInfo = Console.ReadLine();

                        found = receipt.Where(item => item.ProductNumber == incorrectInfo);
                        foreach (var f in found)
                        {
                            f.ProductNumber= correctInfo;
                        }
                        break;

                    case 3:
                        Console.WriteLine("What incorrect product name's product number? ");
                        string targetProductNum = Console.ReadLine();

                        Console.WriteLine("What should the name be? ");
                        correctInfo = Console.ReadLine();

                        found = receipt.Where(item => item.ProductNumber == targetProductNum);
                        foreach (var f in found)
                        {
                            f.ProductName = correctInfo;
                        }
                        break;

                    case 4:
                        Console.WriteLine("What product number goes with the incorrect price? ");
                        targetProductNum = Console.ReadLine();

                        Console.WriteLine("What should the price be? ");
                        correctInfo = Console.ReadLine();

                        found = receipt.Where(item => item.ProductNumber == targetProductNum);
                        foreach (var f in found)
                        {
                            f.ProductPrice = Convert.ToDouble(correctInfo);
                        }
                        break;

                    case 5:
                        break;

                    default:
                        Console.WriteLine("Err. Please try again");
                        break;
                }
            }
            return receipt;
        }

        // cycle through list of products to assign to user

        // input info to grocery table 
    }
}
