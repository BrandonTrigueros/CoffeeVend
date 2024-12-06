using CoffeeDispenserWebApp.Models;

namespace CoffeeDispenserWebApp.Repositories
{
    public interface ICoinRepository
    {
        public void CreateCoin(CoinModel model);
        public List<CoinModel> GetCoins();
        void AddCoins(Dictionary<int, int> selectedCoins);
        void SubstractCoins(Dictionary<int, int> selectedCoins);
    }
}
