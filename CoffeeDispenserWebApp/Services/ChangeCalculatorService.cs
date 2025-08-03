using CoffeeDispenserWebApp.Models;
using CoffeeDispenserWebApp.Repositories;

namespace CoffeeDispenserWebApp.Services
{
    /// <summary>
    /// Service for calculating change in vending machine operations
    /// </summary>
    public class ChangeCalculatorService : IChangeCalculatorService
    {
        private readonly ICoinRepository _coinRepository;
        private readonly ILogger<ChangeCalculatorService> _logger;
        private const string Currency = "Colones";

        public ChangeCalculatorService(ICoinRepository coinRepository, ILogger<ChangeCalculatorService> logger)
        {
            _coinRepository = coinRepository ?? throw new ArgumentNullException(nameof(coinRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<ChangeModel> CalculateChangeAsync(List<CoinModel> customerPayment, int totalAmount)
        {
            if (customerPayment == null)
                throw new ArgumentNullException(nameof(customerPayment));

            if (totalAmount < 0)
                throw new ArgumentException("Total amount cannot be negative", nameof(totalAmount));

            _logger.LogInformation("Calculating change for payment of {PaymentCount} coins, total amount: {TotalAmount} {Currency}", 
                customerPayment.Count, totalAmount, Currency);

            var change = new ChangeModel
            {
                CoinsCount = new List<CoinModel>(),
                TotalChange = 0
            };

            // Calculate total paid amount
            int totalPaid = customerPayment.Sum(c => c.Value * c.Amount);
            int changeAmount = totalPaid - totalAmount;

            _logger.LogDebug("Total paid: {TotalPaid}, Change needed: {ChangeAmount}", totalPaid, changeAmount);

            // Validate payment
            if (changeAmount < 0)
            {
                var insufficientMessage = $"Insufficient payment. Required: {totalAmount} {Currency}, Paid: {totalPaid} {Currency}";
                _logger.LogWarning(insufficientMessage);
                throw new ArgumentException(insufficientMessage);
            }

            // If exact payment, no change needed
            if (changeAmount == 0)
            {
                _logger.LogInformation("Exact payment received, no change required");
                return change;
            }

            // Get available coins for change, ordered by value descending (greedy algorithm)
            var availableCoins = _coinRepository.GetCoins()
                .OrderByDescending(c => c.Value)
                .ToList();

            _logger.LogDebug("Available coins for change: {AvailableCoins}", 
                string.Join(", ", availableCoins.Select(c => $"{c.Value}x{c.Amount}")));

            // Calculate change using greedy algorithm
            int remainingChange = changeAmount;
            var changeCoins = new List<CoinModel>();

            foreach (var coin in availableCoins)
            {
                if (remainingChange <= 0) break;

                int maxCoinsNeeded = remainingChange / coin.Value;
                if (maxCoinsNeeded <= 0) continue;

                int coinsToUse = Math.Min(maxCoinsNeeded, coin.Amount);

                if (coinsToUse > 0)
                {
                    changeCoins.Add(new CoinModel(coin.Value, coinsToUse));
                    remainingChange -= coinsToUse * coin.Value;
                    change.TotalChange += coinsToUse * coin.Value;
                    
                    _logger.LogDebug("Using {CoinsToUse} coins of value {CoinValue}, remaining change: {RemainingChange}", 
                        coinsToUse, coin.Value, remainingChange);
                }
            }

            // Check if we can provide exact change
            if (remainingChange > 0)
            {
                var insufficientChangeMessage = $"Cannot provide exact change. Missing: {remainingChange} {Currency}";
                _logger.LogError(insufficientChangeMessage);
                throw new InvalidOperationException(insufficientChangeMessage);
            }

            change.CoinsCount = changeCoins;

            // Update coin repository (subtract used coins)
            await Task.Run(() => _coinRepository.SubstractCoins(changeCoins));

            _logger.LogInformation("Change calculated successfully. Total change: {TotalChange} {Currency} using {CoinTypes} coin types", 
                change.TotalChange, Currency, changeCoins.Count);

            return change;
        }
    }
}
