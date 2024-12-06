namespace CoffeeDispenserWebApp.Models
{
    public class CoinModel
    {
        public int Value { get; set; }
        public int Amount { get; set; }

        public CoinModel(int value, int amount)
        {
            Value = value;
            Amount = amount;
        }
    }
}
