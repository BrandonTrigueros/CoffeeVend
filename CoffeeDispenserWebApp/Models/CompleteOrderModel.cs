namespace CoffeeDispenserWebApp.Models
{
    public class CompleteOrderModel
    {
        public Dictionary<string, int> SelectedCoffees { get; set; }
        public List<CoinModel> InsertedCoins { get; set; }
    }
}