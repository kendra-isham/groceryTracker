// prompts for image path 
// validates proper image type and size 
using System.IO;
using System;

namespace GroceryTracker
{
    class Image
    {
        public string Path { get; set; }
        public string Base64String { get; set; }
        
        public Image() { }
        
        //gets image path
        public void SetImage()
        {
            Console.Write("Please enter image path: ");
            string path = Console.ReadLine();
            Path = path;
        }

        //checks that image is less than 1MB for OCR API requirements 
        public bool SizeCheck(string path)
        {
            bool sizeValid = false;
            const int BYTES = 1000000;

            if (new FileInfo(path).Length <= BYTES)
            {
                sizeValid = true;
            }
            return sizeValid;
        }

        //checks file extension is valid for OCR API requirments 
       
    }
}
