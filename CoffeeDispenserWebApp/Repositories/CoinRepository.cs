using CoffeeDispenserWebApp.Models;
using System.Security.Cryptography.X509Certificates;

namespace CoffeeDispenserWebApp.Repositories
{
    public class CoinRepository : ICoinRepository
    {
        private List<CoinModel> AvailableCoins;

        public CoinRepository()
        {
            AvailableCoins = new List<CoinModel>();
        }

        public void CreateCoin(CoinModel model)
        {
            AvailableCoins.Add(model);
        }

        public List<CoinModel> GetCoins()
        {
            return AvailableCoins;
        }

        public void AddCoins(List<CoinModel> model)
        {
            foreach (CoinModel coin in model)
            {
                CoinModel existingCoin = AvailableCoins.Find(c => c.Value == coin.Value);
                if (existingCoin != null)
                {
                    existingCoin.Amount += coin.Amount;
                }
                else
                {
                    AvailableCoins.Add(coin);
                }
            }
        }

        public void SubstractCoins(List<CoinModel> model) { 
            foreach (CoinModel coin in model) {
                CoinModel existingCoin = AvailableCoins.Find(c => c.Value == coin.Value);
                if (existingCoin != null)
                {
                    existingCoin.Amount -= coin.Amount;
                }
            }
        }
    }
}
