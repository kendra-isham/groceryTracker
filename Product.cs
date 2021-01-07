//individual product from receipt 

namespace GroceryTracker
{
    public class ApiInfo
    {
        public Parsedresult[] ParsedResults { get; set; }
        public int OCRExitCode { get; set; }
        public bool IsErroredOnProcessing { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }


    public class Parsedresult
    {
        public int FileParseExitCode { get; set; }
        public string TextOrientation { get; set; }
        public string ParsedText { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }
}
