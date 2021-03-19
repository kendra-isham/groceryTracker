using System;
using System.Collections.Generic;
using System.Linq;

namespace GroceryTracker
{
    class CorrectItems
    {
        public static List<Product> ConfirmCorrectInformation(List<Product> receipt)
        {
            Console.Clear(); 
            foreach (Product p in receipt)
            {
                Console.WriteLine(p.PurchaseDate + " " + p.ProductNumber + " " + p.ProductPrice + " " + p.ProductName);
            }
            Console.Write("Is this information correct? Y/N ");
            string userInput = Console.ReadLine().ToLower();
            if(userInput == "y")
            {
                return receipt; 
            }else if (userInput == "n")
            {
                receipt = CorrectReceiptInformation(receipt);
            }else
            {
                Console.WriteLine("Invalid input. Please try again");
                ConfirmCorrectInformation(receipt);
            }

            return receipt;
        }

        private static List<Product> CorrectReceiptInformation(List<Product> receipt)
        {
            int incorrectCategory = -1;

            while (incorrectCategory != 6)
            {
                try
                {
                    Console.WriteLine("\nPlease select the incorrect info category: \n" +
                        "\t1. Purchase Date\n" +
                        "\t2. Product Number\n" +
                        "\t3. Product Name\n" +
                        "\t4. Product Price\n" +
                        "\t5. Display Updated List\n" +
                        "\t6. All information is correct\n");
                    incorrectCategory = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("You made a mistake. Now you have to start all over because I haven't put in proper error handling.");
                    Driver.Main();
                }
                switch (incorrectCategory)
                {
                    case 1:
                        Console.WriteLine("Please enter the incorrect purchase date: ");
                        string incorrectInfo = Console.ReadLine();

                        Console.WriteLine("What should it be? ");
                        string correctInfo = Console.ReadLine();
                        DateTime date;
                        
                        if (DateTime.TryParse(correctInfo, out date))
                        {
                            var foundDate = receipt.Where(item => item.PurchaseDate == incorrectInfo);
                            foreach (var f in foundDate)
                            {
                                f.PurchaseDate = String.Format("{0:yyyy-MM-dd}", date);
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\nError! Please format date yyyy-MM-dd!");
                            break;
                        }

                    case 2:
                        Console.WriteLine("Please enter the incorrect product number: ");
                        incorrectInfo = Console.ReadLine();

                        Console.WriteLine("What should it be? ");
                        correctInfo = Console.ReadLine();

                        var found = receipt.Where(item => item.ProductNumber == incorrectInfo);
                        foreach (var f in found)
                        {
                            f.ProductNumber = correctInfo;
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
                        foreach (Product p in receipt)
                        {
                            Console.WriteLine(p.PurchaseDate + " " + p.ProductNumber + " " + p.ProductPrice + " " + p.ProductName);
                        }
                        break;

                    case 6:
                        break;

                    default:
                        Console.WriteLine("Err. Please try again");
                        break;
                }
            }
            return receipt;
        }
    }
}
