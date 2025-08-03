using CoffeeDispenserWebApp.Models;

namespace CoffeeDispenserWebApp.Services
{
    /// <summary>
    /// Service interface for calculating change in vending machine operations
    /// </summary>
    public interface IChangeCalculatorService
    {
        /// <summary>
        /// Calculates the change to be returned to the customer
        /// </summary>
        /// <param name="customerPayment">List of coins paid by the customer</param>
        /// <param name="totalAmount">Total amount to be paid</param>
        /// <returns>ChangeModel containing the coins to be returned as change</returns>
        /// <exception cref="ArgumentException">Thrown when payment is insufficient</exception>
        /// <exception cref="InvalidOperationException">Thrown when exact change cannot be provided</exception>
        Task<ChangeModel> CalculateChangeAsync(List<CoinModel> customerPayment, int totalAmount);
    }
}
