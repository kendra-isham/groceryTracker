using System;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;

namespace GroceryTracker
{
    class OCRCall
    {
        public static string GetImagePath()
        {
            Console.Write("Please enter Costco receipt image path: ");
            string path = Console.ReadLine();

            return path;
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public static async Task ReadFileLocal(ComputerVisionClient client, string localFile, Data data)
        {
            // Read text from URL
            var textHeaders = await client.ReadInStreamAsync(File.OpenRead(localFile), language: "en");
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;
            
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

            // Stringify precleaned text data 
            Console.WriteLine();
            string preCleanedText = "";
            var textUrlFileResults = results.AnalyzeResult.ReadResults;
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    preCleanedText += line.Text + "\n";
                }
            }

            // Set Data object 
            data.PreCleanedText = preCleanedText; 

        }
    }
}
