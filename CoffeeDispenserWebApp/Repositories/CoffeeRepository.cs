using CoffeeDispenserWebApp.Models;

namespace CoffeeDispenserWebApp.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private List<ICoffeeModel> CoffeeInventory;

        public CoffeeRepository()
        {
            CoffeeInventory = new List<ICoffeeModel>();
        }

        public void CreateCoffee(ICoffeeModel model)
        {
            CoffeeInventory.Add(model);
        }

        public List<ICoffeeModel> GetCoffees()
        {
            return CoffeeInventory;
        }

        public ICoffeeModel GetCoffeeByName(string name)
        {
            return CoffeeInventory.Find(coffee => coffee.Name == name);
        }

        public void SelectCoffees(Dictionary<string, int> selectedCoffees)
        {
            foreach (var coffee in CoffeeInventory)
            {
                if (selectedCoffees.ContainsKey(coffee.Name))
                {
                    coffee.Stock -= selectedCoffees[coffee.Name];
                }
            }
        }
    }
}
