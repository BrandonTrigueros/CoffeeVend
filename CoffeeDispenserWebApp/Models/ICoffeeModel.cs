namespace CoffeeDispenserWebApp.Models
{
    public interface ICoffeeModel
    {
        string Name { get; set; }
        int Stock { get; set; }
        int Price { get; set; }
        string ImageUrl { get; set; }
    }
}
