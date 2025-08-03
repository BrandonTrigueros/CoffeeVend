namespace CoffeeDispenserWebApp.Models
{
    /// <summary>
    /// Model representing the change to be returned to the customer
    /// </summary>
    public class ChangeModel
    {
        /// <summary>
        /// List of coins that make up the change
        /// </summary>
        public required List<CoinModel> CoinsCount { get; set; } = new();

        /// <summary>
        /// Total value of the change
        /// </summary>
        public int TotalChange { get; set; }
    }
}
