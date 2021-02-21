using System;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace receiptReader
{
    static class Program
    {
        static string subscriptionKey = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
        static string endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");

        public static void Main()
        {
            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
            string READ_TEXT_LOCAL_IMAGE = GetImagePath();
            // Extract text (OCR) from a local image using the Read API
            ReadFileLocal(client, READ_TEXT_LOCAL_IMAGE).Wait();
        }

        private static string GetImagePath()
        {
            Console.Write("Please enter image path: ");
            string path = Console.ReadLine();

            return path;
        }

        //Authenticates the API Client
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }


        public static async Task ReadFileLocal(ComputerVisionClient client, string localFile)
        {
            Console.WriteLine("READ FILE FROM LOCAL");

            // Read text from URL
            var textHeaders = await client.ReadInStreamAsync(File.OpenRead(localFile), language: "en");
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            Console.WriteLine($"Reading text from local file {Path.GetFileName(localFile)}...");
            Console.WriteLine();
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            // Display the found text.
            Console.WriteLine();
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    Console.WriteLine(line.Text);
                }
            }
            Thread.Sleep(20000);
            Console.WriteLine();
        }
    }
}