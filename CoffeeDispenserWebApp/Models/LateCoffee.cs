namespace CoffeeDispenserWebApp.Models
{
    public class LateCoffee : ICoffeeModel
    {
        public string Name { get; set; } 
        public int Stock { get; set; } 
        public int Price { get; set; }
        public string ImageUrl { get; set; }

        public LateCoffee(string name, int stock, int price, string imageurl)
        {
            Name = name;
            Stock = stock;
            Price = price;
            ImageUrl = imageurl;
        }
    }
}
