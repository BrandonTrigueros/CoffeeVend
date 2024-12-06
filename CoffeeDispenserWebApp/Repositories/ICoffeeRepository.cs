using CoffeeDispenserWebApp.Models;

namespace CoffeeDispenserWebApp.Repositories
{
    public interface ICoffeeRepository
    {
        public void CreateCoffee(ICoffeeModel model);
        public List<ICoffeeModel> GetCoffees();
        public ICoffeeModel GetCoffeeByName(string name);
        void SelectCoffees(Dictionary<string, int> selectedCoffees);
    }
}
