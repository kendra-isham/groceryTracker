using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;


//api call 
namespace GroceryTracker
{
    class ReceiptReader
    {
        //POST
        public async void SendData(string path) {
            try
            {
                Console.WriteLine("in sendData");
                
                HttpClient client = new HttpClient();
                //this happens
                string testing2 = "before call";
                File.WriteAllText("C:\\Users\\khuts\\OneDrive\\Desktop\\apiResults.txt", testing2);

                /*                var headers = new Dictionary<string, string>();
                                headers.Add("d0a73c8a2d88957", "apikey");
                                headers.Add("2", "ocrengine");
                                headers.Add("true", "detectionOrientation");
                                headers.Add("true", "scale");
                                headers.Add("true", "istable");
                                */
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("d0a73c8a2d88957"), "apikey");
                form.Add(new StringContent("2"), "ocrengine");
                form.Add(new StringContent("true"), "detectionOrientation");
                form.Add(new StringContent("true"), "scale");
                form.Add(new StringContent("true"), "istable");

                byte[] imageData = File.ReadAllBytes(path);
                foreach(var thing in imageData)
                {
                    Console.Write(thing);
                }
                form.Add(new ByteArrayContent(imageData, 0, path.Length), "base64Image");


                HttpResponseMessage response = await client.PostAsync("https://api.ocr.space/parse/image", form);
                Console.WriteLine("after api post");
 
                string strContent = await response.Content.ReadAsStringAsync();
                File.WriteAllText("C:\\Users\\khuts\\OneDrive\\Desktop\\apiResults.txt", strContent);
 
            } catch(Exception e)
                {
                File.WriteAllText("C:\\Users\\khuts\\OneDrive\\Desktop\\apiResults.txt", e.ToString());
            }
        }

    }
}
