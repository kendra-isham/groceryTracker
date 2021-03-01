using System;
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

            // Create a client
            ComputerVisionClient client = OCRCall.Authenticate(endpoint, subscriptionKey);
            
            // Get image path 
            string imagePath = OCRCall.GetImagePath();

            // Extract text (OCR) from a local image using the Read API and call CleanData
            OCRCall.ReadFileLocal(client, imagePath, data).Wait();

            // Initial Receipt Cleaning 
            CleanItUp.DataCleaning(data);

            Console.WriteLine(data.PurchaseDate);
            data.ProductPrices.ForEach(Console.WriteLine);
            data.ProductNumbers.ForEach(Console.WriteLine);
            data.ProductNames.ForEach(Console.WriteLine);

            Thread.Sleep(300000);
        }
    }
}