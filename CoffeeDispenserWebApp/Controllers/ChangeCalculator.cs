using CoffeeDispenserWebApp.Models;
using CoffeeDispenserWebApp.Repositories;

namespace CoffeeDispenserWebApp.Controllers
{
    public class ChangeCalculator
    {
        string Currency;
        private readonly ICoinRepository _coinRepository;

        public ChangeCalculator(ICoinRepository coinRepository)
        {
            Currency = "Colones";
            _coinRepository = coinRepository;
        }

        public ChangeModel CalculateChange(List<CoinModel> customerPay, int totalToPay)
        {
            ChangeModel change = new ChangeModel
            {
                CoinsCount = new List<CoinModel>(),
                TotalChange = 0
            };
            int totalPaid = customerPay.Sum(c => c.Value * c.Amount);
            int changeAmount = totalPaid - totalToPay;
            if (changeAmount < 0)
            {
                throw new ArgumentException("El cliente no ha pagado lo suficiente.");
            }
            else if (changeAmount == 0)
            {
                return change;
            }
            List<CoinModel> availableCoins = _coinRepository.GetCoins()
                                                              .OrderByDescending(c => c.Value)
                                                              .ToList();
            int remainingChange = changeAmount;
            foreach (CoinModel coin in availableCoins)
            {
                if (remainingChange <= 0)
                    break; 

                int maxCoinsNeeded = remainingChange / coin.Value;
                if (maxCoinsNeeded <= 0)
                    continue;

                int coinsToUse = Math.Min(maxCoinsNeeded, coin.Amount);

                if (coinsToUse > 0)
                {
                    change.CoinsCount.Add(new CoinModel(coin.Value, coinsToUse));
                    remainingChange -= coinsToUse * coin.Value;
                    change.TotalChange += coinsToUse * coin.Value;
                    coin.Amount -= coinsToUse;
                }
            }
            if (remainingChange > 0)
            {
                throw new InvalidOperationException("No hay suficientes monedas para devolver el cambio requerido.");
            }
            _coinRepository.SubstractCoins(change.CoinsCount);
            return change;
        }

    }
}
