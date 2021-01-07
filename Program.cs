using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GroceryTracker
{
    class Program
    {
        static void Main()
        {

            Console.WriteLine("Welcome to the check calculator.");
            Image image = new Image();
             
            image.SetImage();
            
            bool isValidSize = image.SizeCheck(image.Path);

            if (isValidSize)
            {
                image.Base64String = image.ConvertImage(image.Path);
                Console.Write("Image Converted to base64!");
            }
            else
            {
                Console.WriteLine("Please resize your image to less than 1MB");
            }

            //api call 
            MainAsync(image).Wait();

        }

        static async Task MainAsync(Image image)
        {
            try
            {
                HttpClient client = new HttpClient();

                MultipartFormDataContent form = new MultipartFormDataContent
                {
                    { new StringContent("d0a73c8a2d88957"), "apikey" },
                    { new StringContent("2"), "ocrengine" },
                    { new StringContent("true"), "detectorientation" },
                    { new StringContent("true"), "scale" },
                    { new StringContent("true"), "istable" },
                    { new StringContent(image.Base64String), "base64Image" }
                };

                HttpResponseMessage response = await client.PostAsync("https://api.ocr.space/parse/image", form);
                
                string strContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("\nafter api call");
                //Product object needs reworked 
                //this is not working
                ApiInfo ocrResult = JsonConvert.DeserializeObject<ApiInfo>(strContent);
                Console.WriteLine("\ninfo after deserialization");

                if (ocrResult.OCRExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Length; i++)
                    {
                        Console.WriteLine(ocrResult.ParsedResults[i].ParsedText);
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: " + strContent);
                }

            }
            catch (Exception e)
            {
                File.WriteAllText("C:\\Users\\khuts\\OneDrive\\Desktop\\apiResults.txt", e.ToString());
            }
        }
    }
}
