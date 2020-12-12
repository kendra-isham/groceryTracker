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

        public byte[] Array { get; set; }
        public Image(string path)
        {
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

        //Converts image to base64
        public string ConvertImage(string path)
        {
            byte[] imageArray = File.ReadAllBytes(path);
            string base64ImageRepresentation = "data:image/jpeg;base64," + Convert.ToBase64String(imageArray);

            return base64ImageRepresentation;
        }

    }
}
