using System;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System.Text.RegularExpressions;
using System.Threading;

namespace GroceryTracker
{
    static class Driver
    {
        static string subscriptionKey = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
        static string endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");
        
        public static void Main()
        {
            Data data = new Data();

            // Create a client
            ComputerVisionClient client = OCRCall.Authenticate(endpoint, subscriptionKey);
            
            // Get image path 
            string imagePath = OCRCall.GetImagePath();

            // Extract text (OCR) from a local image using the Read API and call CleanData
            OCRCall.ReadFileLocal(client, imagePath, data).Wait();

            // Clean Text 
            CleanData(data.PreCleanedText);
        }

        //all the regex
        public static void CleanData(string preCleanedText)
        {
            // removes everything at and above member ID and AID in footer
            string pattern = @"\d{12}"; //gets 12 digits
            var rx = new Regex(pattern, RegexOptions.IgnoreCase);
            var splitResult = rx.Split(preCleanedText);
            string removeCostcoHeader = splitResult[1];

            // removes everything at and below the word "total"
            pattern = @"\b(TOTAL)\b";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeCostcoHeader);
            string removeBelowTotal = splitResult[0];

            // removes everything at and below the word "subtotal"
            pattern = @"\b(SUBTOTAL)\b"; 
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeBelowTotal);
            string removeBelowSubtotal = splitResult[0];

            // removes anything measured per pound -> ex 3 @ 4.49
            pattern = @"\d\s@\s\d[.]\d{2}";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeBelowSubtotal);
            string removePerPound = "";
            foreach(string strings in splitResult)
            {
                removePerPound += strings;
            }

            // removes astricks, _A_, and _E_ 
            pattern = @"[/*]|\s[A]\s|\s[E]\s";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removePerPound);
            string removeAstricksAE = "";
            foreach (string strings in splitResult)
            {
                removeAstricksAE += strings;
            }

            // removes multiple m's 
            pattern = @"\s[M]";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeAstricksAE);
            string removeM = "";
            foreach (string strings in splitResult)
            {
                removeM += strings;
            }

            // get all products
            pattern = @"[\d]+[.]+[\d][\d]";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(removeM);
            string getProducts = "";
            foreach (string strings in splitResult)
            {
                getProducts += strings + "\n";
            }
           
            // removes any 1 char alone on a line after separating into products 
            pattern = @"\s\w\s";
            rx = new Regex(pattern, RegexOptions.IgnoreCase);
            splitResult = rx.Split(getProducts);
            string removeExtraCharOnLine = "";
            foreach (string strings in splitResult)
            {
                removeExtraCharOnLine += strings + "\n";
            }
            Console.WriteLine(removeExtraCharOnLine);
            Thread.Sleep(200000);
        }
    }
}