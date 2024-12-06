using CoffeeDispenserWebApp.Models;

namespace CoffeeDispenserWebApp.Repositories
{
    public interface ICoinRepository
    {
        public void CreateCoin(CoinModel model);
        public List<CoinModel> GetCoins();
        void AddCoins(List<CoinModel> selectedCoins);
        void SubstractCoins(List<CoinModel> selectedCoins);
    }
}
