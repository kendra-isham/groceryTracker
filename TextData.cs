using System.Collections.Generic;

namespace GroceryTracker
{
    class TextData
    {
        public string PreCleanedText { get; set; }

        //TODO: Dictionary instead of List for SQL? 
        public List<string> ProductNumbers { get; set; }
        public List<string> ProductNames { get; set; }
        public List<string> ProductPrices { get; set; }
    }
}
