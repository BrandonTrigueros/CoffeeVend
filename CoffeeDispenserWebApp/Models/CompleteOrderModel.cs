using System.ComponentModel.DataAnnotations;

namespace CoffeeDispenserWebApp.Models
{
    /// <summary>
    /// Model representing a complete coffee order with payment
    /// </summary>
    public class CompleteOrderModel
    {
        /// <summary>
        /// Dictionary of selected coffees and their quantities
        /// </summary>
        [Required]
        public required Dictionary<string, int> SelectedCoffees { get; set; }

        /// <summary>
        /// List of coins inserted by the customer
        /// </summary>
        [Required]
        public required List<CoinModel> InsertedCoins { get; set; }
    }
}