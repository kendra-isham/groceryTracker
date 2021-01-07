using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;


namespace GroceryTracker
{
    class Program
    {
        static async Task Main()
        {

            Console.WriteLine("Welcome to the check calculator.");
            Image image = new Image();
             
            image.SetImage();

            bool isValidSize = image.SizeCheck(image.Path);

            if (isValidSize)
            {
                ImageToBase64(image);
                Console.Write("Image Converted to base64!");
            }
            else
            {
                Console.WriteLine("Please resize your image to less than 1MB");
            }

            //api call 
            await MainAsync(image);
            Console.WriteLine("\nafter api call");
            Console.ReadLine();
        }

        public static void ImageToBase64(Image image)
        {
            //this string needs to be dependent on file extension, hard coding is not the best option here 
            image.Base64String = "data:image/jpeg;base64,"+Convert.ToBase64String(File.ReadAllBytes(image.Path));
            File.WriteAllText("C:\\Users\\khuts\\Desktop\\apiResults.txt", image.Base64String);
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
                    { new StringContent("false"), "istable" },
                    { new StringContent(image.Base64String), "base64Image" }
                };
                Console.WriteLine("About to api");
                HttpResponseMessage response = await client.PostAsync("https://api.ocr.space/parse/image", form);

                string strContent = await response.Content.ReadAsStringAsync();

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
                    Console.WriteLine("ERROR: " + ocrResult.ErrorMessage);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
