namespace CoffeeDispenserWebApp.Models
{
    public class MocachinoCoffee : ICoffeeModel
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }

        public MocachinoCoffee(string name, int stock, int price , string imageUrl)
        {
            Name = name;
            Stock = stock;
            Price = price;
            ImageUrl = imageUrl;
        }
    }
}
