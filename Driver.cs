using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

            // Initial Receipt data cleaning
            List<Product> receipt = CleanItUp.DataCleaning(data);

            // Confirm with user info is correct, correct it if not 
            receipt = AssignItems.ConfirmCorrectInformation(receipt);

            // TODO: Assign products to users 


            // After receipt, check for additional actions
            PromptForMoreRecipts(data);
            
            Thread.Sleep(10);
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
        private static void PromptForMoreRecipts(Product data)
        {
            Console.Write("Do you have another receipt? Y/N ");
            string moreReceiptCheck = Console.ReadLine().ToLower();

            if (moreReceiptCheck == "y")
            {
                /*Console.Write("Do you need to change users? Y/N ");
                string moreUserCheck = Console.ReadLine().ToLower();
                if (moreUserCheck == "y")
                {*/
                    Main();
                /*}
                else if (moreUserCheck == "n")
                {
                    APICall(data);
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please reply Y or N");
                    PromptForMoreRecipts(data);
                }*/
            }
            else if (moreReceiptCheck == "n")
            {
                Console.WriteLine("Goodbye!");
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
                Console.Write("Enter how many people are splitting the cost: ");
                int numOfUsers = Convert.ToInt32(Console.ReadLine());
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
                PromptUserForInfo(users);
            }
        }
    }
}