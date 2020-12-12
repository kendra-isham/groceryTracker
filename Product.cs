//individual product from receipt 

namespace GroceryTracker
{
    class Product
    {

        public class ApiInfo
        {
            public Parsedresult[] ParsedResults { get; set; }
            public int OCRExitCode { get; set; }
            public bool IsErroredOnProcessing { get; set; }
            public float ProcessingTimeInMilliseconds { get; set; }
            public string SearchablePDFURL { get; set; }
        }

        public class Parsedresult
        {
            public Overlay Overlay { get; set; }
            public int FileParseExitCode { get; set; }
            public string TextOrientation { get; set; }
            public string ParsedText { get; set; }
            public string ErrorMessage { get; set; }
            public string ErrorDetails { get; set; }
        }

        public class Overlay
        {
            public Line[] Lines { get; set; }
            public bool HasOverlay { get; set; }
            public string Message { get; set; }
        }

        public class Line
        {
            public string LineText { get; set; }
            public Word[] Words { get; set; }
            public float MaxHeight { get; set; }
            public float MinTop { get; set; }
        }

        public class Word
        {
            public string WordText { get; set; }
            public float Left { get; set; }
            public float Top { get; set; }
            public float Height { get; set; }
            public float Width { get; set; }
        }
    }
}
