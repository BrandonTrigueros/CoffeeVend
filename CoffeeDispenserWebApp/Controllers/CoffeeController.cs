using CoffeeDispenserWebApp.Models;
using CoffeeDispenserWebApp.Repositories;
using CoffeeDispenserWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CoffeeDispenserWebApp.Controllers
{
    /// <summary>
    /// Controller for managing coffee vending operations
    /// </summary>
    public class CoffeeController : Controller
    {
        private readonly ICoffeeRepository _coffeeRepository;
        private readonly ICoinRepository _coinRepository;
        private readonly IChangeCalculatorService _changeCalculatorService;
        private readonly ILogger<CoffeeController> _logger;

        public CoffeeController(
            ICoffeeRepository coffeeRepository, 
            ICoinRepository coinRepository, 
            IChangeCalculatorService changeCalculatorService,
            ILogger<CoffeeController> logger)
        {
            _coffeeRepository = coffeeRepository ?? throw new ArgumentNullException(nameof(coffeeRepository));
            _coinRepository = coinRepository ?? throw new ArgumentNullException(nameof(coinRepository));
            _changeCalculatorService = changeCalculatorService ?? throw new ArgumentNullException(nameof(changeCalculatorService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Initializes the coffee dispenser with default inventory
        /// </summary>
        private void BuildDispenser()
        {
            var existingCoffees = _coffeeRepository.GetCoffees();
            if (existingCoffees.Count == 0)
            {
                _logger.LogInformation("Initializing coffee inventory with default items");
                
                _coffeeRepository.CreateCoffee(new AmericanoCoffee("Americano", 10, 950, "https://coffeforus.com/wp-content/uploads/2022/12/Americano-coffee-recipe.jpg"));
                _coffeeRepository.CreateCoffee(new CapuchinoCoffee("Capuchino", 8, 1200, "https://upload.wikimedia.org/wikipedia/commons/c/c8/Cappuccino_at_Sightglass_Coffee.jpg"));
                _coffeeRepository.CreateCoffee(new LateCoffee("Latte", 10, 1350, "https://coffeeaffection.com/wp-content/uploads/2020/01/how-to-make-a-latte-at-home.jpg"));
                _coffeeRepository.CreateCoffee(new MocachinoCoffee("Mocachino", 15, 1500, "http://cocinillas.obesia.com/images/2020/02febrero/mocachino.jpg"));
            }

            var existingCoins = _coinRepository.GetCoins();
            if (existingCoins.Count == 0)
            {
                _logger.LogInformation("Initializing coin inventory with default denominations");
                
                _coinRepository.CreateCoin(new CoinModel(500, 20));
                _coinRepository.CreateCoin(new CoinModel(100, 30));
                _coinRepository.CreateCoin(new CoinModel(50, 50));
                _coinRepository.CreateCoin(new CoinModel(25, 25));
            }
        }

        /// <summary>
        /// Displays the main coffee selection page
        /// </summary>
        /// <returns>View with available coffees</returns>
        public IActionResult Index()
        {
            try
            {
                BuildDispenser();
                var coffees = _coffeeRepository.GetCoffees();
                
                _logger.LogInformation("Displaying coffee selection page with {CoffeeCount} available coffees", coffees.Count);
                
                return View(coffees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading coffee selection page");
                return View("Error");
            }
        }

        /// <summary>
        /// Processes a coffee order and payment
        /// </summary>
        /// <param name="order">Complete order model with selections and payment</param>
        /// <returns>JSON result with success status and change information</returns>
        [HttpPost]
        public async Task<IActionResult> ConfirmOrder([FromBody] CompleteOrderModel order)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid order model received");
                return Json(new { success = false, message = "Invalid order data" });
            }

            try
            {
                _logger.LogInformation("Processing order with {CoffeeCount} coffee types and {CoinCount} coin types", 
                    order.SelectedCoffees.Count, order.InsertedCoins.Count);

                // Validate coffee availability and calculate total
                int totalToPay = 0;
                var coffeeDetails = new List<string>();

                foreach (var coffee in order.SelectedCoffees)
                {
                    var coffeeModel = _coffeeRepository.GetCoffeeByName(coffee.Key);
                    if (coffeeModel == null)
                    {
                        _logger.LogWarning("Coffee '{CoffeeName}' not found in inventory", coffee.Key);
                        return Json(new { success = false, message = $"Coffee '{coffee.Key}' not found." });
                    }

                    if (coffeeModel.Stock < coffee.Value)
                    {
                        _logger.LogWarning("Insufficient stock for coffee '{CoffeeName}'. Available: {Stock}, Requested: {Requested}", 
                            coffee.Key, coffeeModel.Stock, coffee.Value);
                        return Json(new { success = false, message = $"Insufficient stock for {coffee.Key}. Available: {coffeeModel.Stock}" });
                    }

                    totalToPay += coffeeModel.Price * coffee.Value;
                    coffeeDetails.Add($"{coffee.Value}x {coffee.Key} (₡{coffeeModel.Price} each)");
                }

                _logger.LogInformation("Order details: {CoffeeDetails}. Total: ₡{TotalToPay}", 
                    string.Join(", ", coffeeDetails), totalToPay);

                // Update inventories
                _coffeeRepository.SelectCoffees(order.SelectedCoffees);
                _coinRepository.AddCoins(order.InsertedCoins);

                // Calculate change
                var change = await _changeCalculatorService.CalculateChangeAsync(order.InsertedCoins, totalToPay);

                _logger.LogInformation("Order processed successfully. Change: ₡{ChangeAmount}", change.TotalChange);

                return Json(new { success = true, change = change });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Payment validation failed");
                return Json(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Cannot provide exact change");
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing order");
                return Json(new { success = false, message = "An unexpected error occurred. Please try again." });
            }
        }

        /// <summary>
        /// Displays the current coin inventory
        /// </summary>
        /// <returns>View with current coin availability</returns>
        public IActionResult CoinList()
        {
            try
            {
                var coins = _coinRepository.GetCoins();
                _logger.LogInformation("Displaying coin inventory with {CoinTypes} coin types", coins.Count);
                return View(coins);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading coin inventory");
                return View("Error");
            }
        }
    }
}
