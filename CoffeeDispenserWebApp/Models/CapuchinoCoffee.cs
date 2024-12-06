namespace CoffeeDispenserWebApp.Models
{
    public class CapuchinoCoffee : ICoffeeModel
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }

        public CapuchinoCoffee(string name, int stock, int price, string imageurl) 
        {
            Name = name;
            Stock = stock;
            Price = price;
            ImageUrl = imageurl;
        }
    }
}
