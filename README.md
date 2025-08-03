# Coffee Dispenser Web Application

A sophisticated ASP.NET Core MVC web application that simulates a coffee vending machine with comprehensive change calculation functionality.

## 🚀 Features

- **Interactive Coffee Selection**: Browse and select from 4 different coffee types with images and pricing
- **Dynamic Order Management**: Real-time order total calculation and management
- **Coin-based Payment System**: Accept multiple coin denominations (₡1000, ₡500, ₡100, ₡50, ₡25)
- **Smart Change Calculator**: Automatic change calculation with optimal coin distribution
- **Inventory Management**: Track coffee stock and coin availability
- **Responsive UI**: Bootstrap-powered responsive design with modal dialogs
- **Comprehensive Testing**: Unit tests for change calculation logic

## ☕ Available Coffee Types

| Coffee Type | Price (₡) | Default Stock |
|-------------|-----------|---------------|
| Americano   | 950       | 10            |
| Capuchino   | 1,200     | 8             |
| Latte       | 1,350     | 10            |
| Mocachino   | 1,500     | 15            |

## 💰 Supported Coin Denominations

- ₡1000 (1000 colones)
- ₡500 (500 colones)
- ₡100 (100 colones)
- ₡50 (50 colones)
- ₡25 (25 colones)

## 🏗️ Architecture

### Project Structure

```
CoffeeDispenserWebApp/
├── Controllers/
│   ├── CoffeeController.cs       # Main coffee ordering logic
│   ├── ChangeCalculator.cs       # Change calculation algorithms
│   └── HomeController.cs         # Home and error pages
├── Models/
│   ├── Coffee Models/
│   │   ├── ICoffeeModel.cs       # Coffee interface
│   │   ├── AmericanoCoffee.cs    # Americano implementation
│   │   ├── CapuchinoCoffee.cs    # Capuchino implementation
│   │   ├── LateCoffee.cs         # Latte implementation
│   │   └── MocachinoCoffee.cs    # Mocachino implementation
│   ├── ChangeModel.cs            # Change calculation result
│   ├── CoinModel.cs              # Coin representation
│   ├── CompleteOrderModel.cs     # Order request model
│   └── ErrorViewModel.cs         # Error handling model
├── Repositories/
│   ├── ICoffeeRepository.cs      # Coffee repository interface
│   ├── CoffeeRepository.cs       # Coffee inventory management
│   ├── ICoinRepository.cs        # Coin repository interface
│   └── CoinRepository.cs         # Coin inventory management
├── Views/
│   ├── Coffee/
│   │   ├── Index.cshtml          # Main coffee selection interface
│   │   └── CoinList.cshtml       # Coin inventory view
│   └── Shared/                   # Shared layout and error views
└── wwwroot/                      # Static assets (CSS, JS, images)

TestProjectCoffee/
└── UnitTest1.cs                  # Comprehensive change calculator tests
```

### Design Patterns

- **Repository Pattern**: Separation of data access logic for coffees and coins
- **Dependency Injection**: Loose coupling between components
- **MVC Pattern**: Clear separation of concerns
- **Interface Segregation**: Dedicated interfaces for different coffee types

## 🛠️ Technologies Used

- **Backend**: ASP.NET Core 8.0 MVC
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5
- **Testing**: NUnit, Moq
- **Language**: C# with nullable reference types enabled

## 📋 Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code
- Web browser (Chrome, Firefox, Safari, Edge)

## 🚀 Getting Started

### Installation

1. **Clone or download the repository**
   ```bash
   git clone <repository-url>
   cd CoffeeVend
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

### Running the Application

1. **Run the web application**
   ```bash
   cd CoffeeDispenserWebApp
   dotnet run
   ```

2. **Access the application**
   - HTTP: `http://localhost:5161`
   - HTTPS: `https://localhost:7122`

3. **Alternative: Use Visual Studio**
   - Open `CoffeeDispenserWebApp.sln`
   - Press F5 or click "Start Debugging"

## 🧪 Testing

### Running Unit Tests

```bash
cd TestProjectCoffee
dotnet test
```

### Test Coverage

The test suite includes comprehensive scenarios for the change calculator:

- ✅ Exact payment (no change required)
- ✅ Overpayment with sufficient change available
- ✅ Overpayment with insufficient change available
- ✅ Underpayment error handling
- ✅ Multiple coin type change calculations
- ✅ Edge cases (zero payment, no coins available)

## 💻 Usage

### Making a Coffee Order

1. **Browse Coffee Selection**: View available coffees with images, prices, and stock levels

2. **Add to Order**: 
   - Select quantity for desired coffee(s)
   - Click "Add to Order" button
   - Review order summary with total cost

3. **Payment Process**:
   - Click "Add Payment" to open payment modal
   - Insert coins using the quantity inputs
   - View real-time payment total
   - Click "Confirm Payment" when sufficient funds inserted

4. **Order Completion**:
   - System calculates and dispenses change
   - Order confirmation with change breakdown
   - Inventory automatically updated

### Administrative Features

- **Coin Inventory**: Access `/coins` endpoint to view current coin availability
- **Automatic Initialization**: Coffee and coin inventories are automatically seeded on first run

## 🔧 Configuration

### Default Configuration

The application uses in-memory repositories with the following defaults:

**Coffee Inventory:**
- Americano: 10 units at ₡950 each
- Capuchino: 8 units at ₡1,200 each  
- Latte: 10 units at ₡1,350 each
- Mocachino: 15 units at ₡1,500 each

**Coin Inventory:**
- ₡500 coins: 20 units
- ₡100 coins: 30 units
- ₡50 coins: 50 units
- ₡25 coins: 25 units

### Customization

To modify default inventory or add new coffee types:

1. **Edit coffee initialization** in `CoffeeController.BuildDispenser()`
2. **Create new coffee model** implementing `ICoffeeModel`
3. **Adjust coin denominations** in `CoffeeController.BuildDispenser()`

## 🏃‍♂️ API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Main coffee selection page |
| GET | `/coins` | View coin inventory |
| POST | `/Coffee/ConfirmOrder` | Process coffee order and payment |

## 🚨 Error Handling

The application includes comprehensive error handling for:

- **Insufficient Payment**: Clear messaging when payment is below total cost
- **Insufficient Change**: Handles cases where exact change cannot be provided
- **Out of Stock**: Prevents ordering when coffee inventory is depleted
- **Invalid Orders**: Validates order data and coin inputs

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🔮 Future Enhancements

- **Database Integration**: Replace in-memory repositories with persistent storage
- **User Authentication**: Add user accounts and order history
- **Inventory Management**: Admin interface for restocking and pricing
- **Analytics Dashboard**: Sales reporting and inventory analytics

---

**Made with ☕ and ❤️ by Brandon Trigueros**
