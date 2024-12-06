using NUnit.Framework;
using CoffeeDispenserWebApp.Models;
using CoffeeDispenserWebApp.Repositories;
using CoffeeDispenserWebApp.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProjectCoffee
{
    public class ChangeCalculatorTests
    {
        private Mock<ICoinRepository> _coinRepositoryMock;
        private ChangeCalculator _changeCalculator;

        [SetUp]
        public void Setup()
        {
            _coinRepositoryMock = new Mock<ICoinRepository>();
            _changeCalculator = new ChangeCalculator(_coinRepositoryMock.Object);
        }

        /// <summary>
        /// Prueba cuando el cliente paga exactamente el total a pagar. No se requiere cambio.
        /// </summary>
        [Test]
        public void CalculateChange_ExactPayment_NoChangeNeeded()
        {
            // Arrange
            var customerPay = new List<CoinModel>
            {
                new CoinModel(500, 2) // Total pagado: 1000
            };
            int totalToPay = 1000;

            _coinRepositoryMock.Setup(repo => repo.GetCoins()).Returns(new List<CoinModel>
            {
                new CoinModel(500, 2),
                new CoinModel(100, 5),
                new CoinModel(50, 10)
            });

            // Act
            ChangeModel result = _changeCalculator.CalculateChange(customerPay, totalToPay);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.CoinsCount);
            Assert.AreEqual(0, result.TotalChange);
        }

        /// <summary>
        /// Prueba cuando el cliente paga más y hay suficiente cambio disponible.
        /// </summary>
        [Test]
        public void CalculateChange_PaymentMore_SufficientChangeAvailable()
        {
            // Arrange
            var customerPay = new List<CoinModel>
            {
                new CoinModel(500, 3) // Total pagado: 1500
            };
            int totalToPay = 1000;
            // Cambio esperado: 500

            _coinRepositoryMock.Setup(repo => repo.GetCoins()).Returns(new List<CoinModel>
            {
                new CoinModel(500, 2),
                new CoinModel(100, 5),
                new CoinModel(50, 10)
            });

            // Act
            ChangeModel result = _changeCalculator.CalculateChange(customerPay, totalToPay);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CoinsCount.Count);
            Assert.AreEqual(500, result.CoinsCount[0].Value);
            Assert.AreEqual(1, result.CoinsCount[0].Amount);
            Assert.AreEqual(500, result.TotalChange);
        }

        /// <summary>
        /// Prueba cuando el cliente paga más pero no hay suficiente cambio disponible.
        /// </summary>
        [Test]
        public void CalculateChange_PaymentMore_InsufficientChangeAvailable()
        {
            // Arrange
            var customerPay = new List<CoinModel>
            {
                new CoinModel(500, 3) // Total pagado: 1500
            };
            int totalToPay = 1000;
            // Cambio esperado: 500, pero solo hay 0 de 500 disponibles

            _coinRepositoryMock.Setup(repo => repo.GetCoins()).Returns(new List<CoinModel>
            {
                new CoinModel(500, 0),
                new CoinModel(100, 5),
                new CoinModel(50, 10)
            });

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _changeCalculator.CalculateChange(customerPay, totalToPay));
            Assert.AreEqual("No hay suficientes monedas para devolver el cambio requerido.", ex.Message);
        }

        /// <summary>
        /// Prueba cuando el cliente paga menos que el total a pagar. Debería lanzar una excepción.
        /// </summary>
        [Test]
        public void CalculateChange_PaymentLess_ThrowsException()
        {
            // Arrange
            var customerPay = new List<CoinModel>
            {
                new CoinModel(500, 1) // Total pagado: 500
            };
            int totalToPay = 1000;

            _coinRepositoryMock.Setup(repo => repo.GetCoins()).Returns(new List<CoinModel>
            {
                new CoinModel(500, 2),
                new CoinModel(100, 5),
                new CoinModel(50, 10)
            });

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _changeCalculator.CalculateChange(customerPay, totalToPay));
            Assert.AreEqual("El cliente no ha pagado lo suficiente.", ex.Message);
        }

        /// <summary>
        /// Prueba cuando el cliente paga más pero el cambio no puede ser completado con las monedas disponibles.
        /// </summary>
        [Test]
        public void CalculateChange_PaymentMore_ChangeCannotBeCompleted()
        {
            // Arrange
            var customerPay = new List<CoinModel>
            {
                new CoinModel(1000, 1) // Total pagado: 1000
            };
            int totalToPay = 700;
            // Cambio esperado: 300, pero solo hay 2x100 y 0x50

            _coinRepositoryMock.Setup(repo => repo.GetCoins()).Returns(new List<CoinModel>
            {
                new CoinModel(500, 0),
                new CoinModel(100, 2),
                new CoinModel(50, 0)
            });

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _changeCalculator.CalculateChange(customerPay, totalToPay));
            Assert.AreEqual("No hay suficientes monedas para devolver el cambio requerido.", ex.Message);
        }

        /// <summary>
        /// Prueba con múltiples tipos de monedas para calcular el cambio correctamente.
        /// </summary>
        [Test]
        public void CalculateChange_PaymentMore_MultipleCoinTypes()
        {
            // Arrange
            var customerPay = new List<CoinModel>
            {
                new CoinModel(1000, 1) // Total pagado: 1000
            };
            int totalToPay = 275;
            // Cambio esperado: 725
            // Disponibles: 500x2, 100x5, 50x10, 25x20
            _coinRepositoryMock.Setup(repo => repo.GetCoins()).Returns(new List<CoinModel>
            {
                new CoinModel(500, 2),
                new CoinModel(100, 5),
                new CoinModel(50, 10),
                new CoinModel(25, 20)
            });

            // Act
            ChangeModel result = _changeCalculator.CalculateChange(customerPay, totalToPay);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.CoinsCount.Count);
            Assert.AreEqual(500, result.CoinsCount[0].Value);
            Assert.AreEqual(1, result.CoinsCount[0].Amount);
            Assert.AreEqual(200, result.CoinsCount[0].Value * result.CoinsCount[0].Amount);

            Assert.AreEqual(25, result.CoinsCount[1].Value);
            Assert.AreEqual(1, result.CoinsCount[1].Amount);
            Assert.AreEqual(25, result.CoinsCount[1].Value * result.CoinsCount[1].Amount);

            Assert.AreEqual(0, result.TotalChange); // TotalChange es la suma de los valores devueltos
        }

        /// <summary>
        /// Prueba cuando el total a pagar es cero. No se requiere cambio.
        /// </summary>
        [Test]
        public void CalculateChange_TotalToPayZero_NoChangeNeeded()
        {
            // Arrange
            var customerPay = new List<CoinModel>
            {
                new CoinModel(500, 2) // Total pagado: 1000
            };
            int totalToPay = 0;

            _coinRepositoryMock.Setup(repo => repo.GetCoins()).Returns(new List<CoinModel>
            {
                new CoinModel(500, 2),
                new CoinModel(100, 5),
                new CoinModel(50, 10)
            });

            // Act
            ChangeModel result = _changeCalculator.CalculateChange(customerPay, totalToPay);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.CoinsCount);
            Assert.AreEqual(0, result.TotalChange);
        }

        /// <summary>
        /// Prueba cuando no hay monedas disponibles en el repositorio. Debería lanzar una excepción si se requiere cambio.
        /// </summary>
        [Test]
        public void CalculateChange_PaymentMore_NoCoinsAvailable()
        {
            // Arrange
            var customerPay = new List<CoinModel>
            {
                new CoinModel(500, 2) // Total pagado: 1000
            };
            int totalToPay = 800;
            // Cambio esperado: 200, pero no hay monedas disponibles

            _coinRepositoryMock.Setup(repo => repo.GetCoins()).Returns(new List<CoinModel>());

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _changeCalculator.CalculateChange(customerPay, totalToPay));
            Assert.AreEqual("No hay suficientes monedas para devolver el cambio requerido.", ex.Message);
        }
    }
}
