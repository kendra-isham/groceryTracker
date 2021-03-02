using System;
using System.Collections.Generic;
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
            TextData data = new TextData();
            List<User> users = new List<User>();

            PromptUserForInfo(users);

            APICall(data);

            // Initial Receipt data cleaning
            CleanItUp.DataCleaning(data);

            // After first receipt, check for additional actions
            PromptForMoreRecipts(data);
            
            Thread.Sleep(10);
        }

        private static void APICall(TextData data)
        {
            // Create a client
            ComputerVisionClient client = OCRCall.Authenticate(endpoint, subscriptionKey);

            // Get image path 
            string imagePath = OCRCall.GetImagePath();

            // Extract text (OCR) from a local image using the Read API and call CleanData
            OCRCall.ReadFileLocal(client, imagePath, data).Wait();
        }
        private static void PromptForMoreRecipts(TextData data)
        {
            Console.Write("Do you have another receipt? Y/N ");
            string moreReceiptCheck = Console.ReadLine().ToLower();

            if (moreReceiptCheck == "y")
            {
                Console.Write("Do you need to change users? Y/N ");
                string moreUserCheck = Console.ReadLine().ToLower();
                if (moreUserCheck == "y")
                {
                    Main();
                } else if (moreUserCheck == "n")
                {
                    APICall(data);
                } else
                {
                    Console.WriteLine("Invalid Input. Please reply Y or N");
                    Console.Write("Do you need to change users? Y/N ");
                    moreUserCheck = Console.ReadLine().ToLower();
                }
            } else if (moreReceiptCheck == "n")
            {
                Console.WriteLine("Goodbye!");
            } else
            {
                Console.WriteLine("Invalid Input. Please reply Y or N");
                PromptForMoreRecipts(data);
            }

            // TODO: If more receipts, check fora different users 
        }
        private static void PromptUserForInfo(List<User> users)
        {
            Console.Write("Enter how many people are splitting the cost: ");
            int numOfUsers = Convert.ToInt32(Console.ReadLine());

            for(int i = 0; i < numOfUsers; i++) { 
                User user = new User();
                Console.Write("Enter user's name: ");
                user.Name = Console.ReadLine(); 
                users.Add(user);
            }

        }
    }
}