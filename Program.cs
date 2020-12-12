using Newtonsoft.Json;
using System;
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
            Console.Write("Please enter image path: ");
            string path = Console.ReadLine();

            Image image = new Image(path);
            bool isValidSize = image.SizeCheck(path);

            if (isValidSize)
            {
                image.Base64String = image.ConvertImage(path);
                Console.Write("Image Converted to base64!");
            }
            else
            {
                Console.WriteLine("Please resize your image to less than 1MB");
            }
            //image.Base64String = image.ConvertImage(path); 

            MainAsync(image.Base64String).Wait();

        }

        static async Task MainAsync(string path)
        {
            try
            {
                HttpClient client = new HttpClient();

                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("d0a73c8a2d88957"), "apikey");
                form.Add(new StringContent("2"), "ocrengine");
                form.Add(new StringContent("true"), "detectorientation");
                form.Add(new StringContent("true"), "scale");
                form.Add(new StringContent("true"), "istable");
                form.Add(new StringContent(path), "base64Image");

                HttpResponseMessage response = await client.PostAsync("https://api.ocr.space/parse/image", form);

                string strContent = await response.Content.ReadAsStringAsync();

                //Product object needs reworked 
                Product result = JsonConvert.DeserializeObject<Product>(strContent);
                File.WriteAllText("C:\\Users\\khuts\\OneDrive\\Desktop\\apiResults.txt", "completed");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
