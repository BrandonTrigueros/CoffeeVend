using CoffeeDispenserWebApp.Models;

namespace CoffeeDispenserWebApp.Repositories
{
    /// <summary>
    /// Repository interface for managing coffee inventory
    /// </summary>
    public interface ICoffeeRepository
    {
        /// <summary>
        /// Creates a new coffee in the inventory
        /// </summary>
        /// <param name="model">Coffee model to add</param>
        void CreateCoffee(ICoffeeModel model);

        /// <summary>
        /// Gets all available coffees
        /// </summary>
        /// <returns>List of all coffees in inventory</returns>
        List<ICoffeeModel> GetCoffees();

        /// <summary>
        /// Gets a specific coffee by name
        /// </summary>
        /// <param name="name">Name of the coffee to find</param>
        /// <returns>Coffee model if found, null otherwise</returns>
        ICoffeeModel? GetCoffeeByName(string name);

        /// <summary>
        /// Updates coffee stock after selection
        /// </summary>
        /// <param name="selectedCoffees">Dictionary of coffee names and quantities selected</param>
        void SelectCoffees(Dictionary<string, int> selectedCoffees);
    }
}
