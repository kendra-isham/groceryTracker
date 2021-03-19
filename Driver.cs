using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace GroceryTracker
{
    static class Driver
    {
        static string subscriptionKey = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
        static string endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");

        public static void Main()
        {
            Product data = new Product();
            List<User> users = new List<User>();

            PromptUserForInfo(users);
            APICall(data);
            List<Product> receipt = CleanItUp.DataCleaning(data);
            receipt = CorrectItems.ConfirmCorrectInformation(receipt);

            if (users.Count > 1)
            {
                User shared = new User();
                shared.Name = "Shared";
                users.Add(shared);
                receipt = AssignItems.AssignResponsibleParty(receipt, users);
                users = SplitShared(users, receipt);
                users = CalculateCosts(users, receipt);
                users.Remove(shared);
            }
            else
            {
                foreach (Product p in receipt)
                {
                    p.ResponsibleParty = users[0];
                    users[0].TotalCharges += p.ProductPrice;
                }
            }

            DisplayTotalCharges(users);
            DBConnection.SQLInsert(receipt);
            PromptForMoreRecipts(data);
        }
        private static void DisplayTotalCharges(List<User> users)
        {
            Console.Clear();
            Console.WriteLine("Total charges: \n");
            foreach (User u in users)
            {
                Console.WriteLine(u.Name +" $"+ Math.Round(u.TotalCharges, 2));
            }
        }
        private static void APICall(Product data)
        {
            // Create a client
            ComputerVisionClient client = OCRCall.Authenticate(endpoint, subscriptionKey);

            // Get image path
            string imagePath = OCRCall.GetImagePath();

            // Extract text (OCR) from a local image using the Read API and call CleanData
            OCRCall.ReadFileLocal(client, imagePath, data).Wait();
        }
        private static List<User> CalculateCosts(List<User> users, List<Product> receipt)
        {
            //target individual users with foreach
            foreach(User u in users)
            {
                foreach (Product p in receipt)
                {
                    if(p.ResponsibleParty.Name.Equals(u.Name))
                    {
                        u.TotalCharges += p.ProductPrice;
                    }
                }                    
            }
            return users;
        }
        private static List<User> SplitShared(List<User> users, List<Product> receipt)
        {
            double priceToSplit; 
            foreach(Product p in receipt)
            {
                // there is probably a better way to do this 
                // target shared user and split prices amongst rest of users 
                if (p.ResponsibleParty == users.Find(x => x.Name == "Shared")){
                    // -1 to remove "shared" user from count 
                    priceToSplit = p.ProductPrice / (users.Count-1);
                    
                    // add shared cost item cost to user's total charges
                    for(int i = 0; i < users.Count-1; i++)
                    {
                        users[i].TotalCharges += priceToSplit;
                    }
                }
            }
            return users; 
        }
        private static void PromptForMoreRecipts(Product data)
        {
            Console.Write("Do you have another receipt? Y/N ");
            string moreReceiptCheck = Console.ReadLine().ToLower();

            if (moreReceiptCheck == "y")
            {
                Main();
            }
            else if (moreReceiptCheck == "n")
            {
                Console.WriteLine("Goodbye!");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid Input. Please reply Y or N");
                PromptForMoreRecipts(data);
            }
        }
        private static void PromptUserForInfo(List<User> users)
        {
            try
            {
                Console.Write("Welcome to the Grocery Tracker!\nEnter how many people are splitting the cost: ");
                int numOfUsers = Convert.ToInt32(Console.ReadLine());
                if(numOfUsers < 1)
                {
                    Console.Write("Must be greater than 0.\nEnter how many people are splitting the cost: ");
                    numOfUsers = Convert.ToInt32(Console.ReadLine());
                }

                for (int i = 0; i < numOfUsers; i++)
                {
                    User user = new User();
                    Console.Write("Enter name: ");
                    user.Name = Console.ReadLine();
                    users.Add(user);
                }
            }catch (Exception)
            {
                Console.WriteLine("Error. Please try again");
                Main();
            }
        }
    }
}