using CoffeeDispenserWebApp.Models;
using CoffeeDispenserWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CoffeeDispenserWebApp.Controllers
{
    public class CoffeeControler : Controller
    {
        private readonly ICoffeeRepository _coffeeRepository;

        public CoffeeControler(ICoffeeRepository coffeeRepository)
        {
            _coffeeRepository = coffeeRepository;
        }

        public void BuildDDispenser()
        {
            var existingCoffees = _coffeeRepository.GetCoffees();
            if (existingCoffees.Count == 0)
            {
                _coffeeRepository.CreateCoffee(new AmericanoCoffee("Americano", 10, 950, "https://coffeforus.com/wp-content/uploads/2022/12/Americano-coffee-recipe.jpg"));
                _coffeeRepository.CreateCoffee(new CapuchinoCoffee("Capuchino", 8, 1200, "https://upload.wikimedia.org/wikipedia/commons/c/c8/Cappuccino_at_Sightglass_Coffee.jpg"));
                _coffeeRepository.CreateCoffee(new LateCoffee("Late", 10, 1350, "https://coffeeaffection.com/wp-content/uploads/2020/01/how-to-make-a-latte-at-home.jpg"));
                _coffeeRepository.CreateCoffee(new MocachinoCoffee("Mocachino", 15, 1500, "http://cocinillas.obesia.com/images/2020/02febrero/mocachino.jpg"));
            }
        }

        public IActionResult Index()
        {
            BuildDDispenser();
            var coffees = _coffeeRepository.GetCoffees();
            return View(coffees);
        }

        [HttpPost]
        public IActionResult ConfirmOrder([FromBody] Dictionary<string, int> selectedCoffees)
        {
            _coffeeRepository.SelectCoffees(selectedCoffees);
            return Json(new { success = true });
        }
    }
}
