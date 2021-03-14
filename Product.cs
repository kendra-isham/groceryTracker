using System.Collections.Generic;

namespace GroceryTracker
{
    class Product
    {
        public string PreCleanedText { get; set; }
        public string PurchaseDate { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public List<Product> Receipt { get; set; }
    }

}
