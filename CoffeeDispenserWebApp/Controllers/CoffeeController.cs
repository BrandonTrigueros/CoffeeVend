using CoffeeDispenserWebApp.Models;
using CoffeeDispenserWebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CoffeeDispenserWebApp.Controllers
{
    public class CoffeeController : Controller
    {
        private readonly ICoffeeRepository _coffeeRepository;
        private readonly ICoinRepository _coinRepository;
        private readonly ChangeCalculator _changeCalculator;

        public CoffeeController(ICoffeeRepository coffeeRepository, ICoinRepository coinRepository, ChangeCalculator changeCalculator)
        {
            _coffeeRepository = coffeeRepository;
            _coinRepository = coinRepository;
            _changeCalculator = changeCalculator;
        }

        public void BuildDispenser()
        {
            var existingCoffees = _coffeeRepository.GetCoffees();
            if (existingCoffees.Count == 0)
            {
                _coffeeRepository.CreateCoffee(new AmericanoCoffee("Americano", 10, 950, "https://coffeforus.com/wp-content/uploads/2022/12/Americano-coffee-recipe.jpg"));
                _coffeeRepository.CreateCoffee(new CapuchinoCoffee("Capuchino", 8, 1200, "https://upload.wikimedia.org/wikipedia/commons/c/c8/Cappuccino_at_Sightglass_Coffee.jpg"));
                _coffeeRepository.CreateCoffee(new LateCoffee("Late", 10, 1350, "https://coffeeaffection.com/wp-content/uploads/2020/01/how-to-make-a-latte-at-home.jpg"));
                _coffeeRepository.CreateCoffee(new MocachinoCoffee("Mocachino", 15, 1500, "http://cocinillas.obesia.com/images/2020/02febrero/mocachino.jpg"));
            }

            var existingCoins = _coinRepository.GetCoins();
            if (existingCoins.Count == 0)
            {
                _coinRepository.CreateCoin(new CoinModel(500, 20));
                _coinRepository.CreateCoin(new CoinModel(100, 30));
                _coinRepository.CreateCoin(new CoinModel(50, 50));
                _coinRepository.CreateCoin(new CoinModel(25, 25));
            }
        }

        public IActionResult Index()
        {
            BuildDispenser();
            var coffees = _coffeeRepository.GetCoffees();
            return View(coffees);
        }

        [HttpPost]
        public IActionResult ConfirmOrder([FromBody] CompleteOrderModel order)
        {
            try
            {
                _coffeeRepository.SelectCoffees(order.SelectedCoffees);
                _coinRepository.AddCoins(order.InsertedCoins);
                int totalToPay = 0;
                foreach (var coffee in order.SelectedCoffees)
                {
                    var coffeeModel = _coffeeRepository.GetCoffeeByName(coffee.Key);
                    if (coffeeModel == null)
                        throw new Exception($"Coffee '{coffee.Key}' not found.");
                    totalToPay += coffeeModel.Price * coffee.Value;
                }
                ChangeModel change = _changeCalculator.CalculateChange(order.InsertedCoins, totalToPay);
                return Json(new { success = true, change = change });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult CoinList()
        {
            var coins = _coinRepository.GetCoins();
            return View(coins);
        }
    }
}
