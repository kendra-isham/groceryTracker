//individual product from receipt 

namespace GroceryTracker
{
    class Product
    {
        public string Name { get; set; }
        public string Store { get; set; }
        public double Price { get; set; }
        public int OCRExitCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
