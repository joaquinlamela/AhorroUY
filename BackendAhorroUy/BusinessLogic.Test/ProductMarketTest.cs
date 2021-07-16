using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiModels.DataTypes.MarketDTs;

namespace BusinessLogic.Test
{

    [TestClass]
    public class ProductMarketTest
    {
        Product aProduct;
        Product aProduct2;
        Market aMarket;
        BestOptionRequest bestOption; 

        [TestInitialize]
        public void SetUp()
        {
            aProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Barcode = "1234567893",
                Category = new Category(),
                Description = "Descripcion de producto",
                ImageUrl = "urlImage",
                Name = "Asado",
                ProductsMarkets = new List<ProductMarket>()
            };

            aProduct2 = new Product()
            {
                Id = Guid.NewGuid(),
                Barcode = "1234333893",
                Category = new Category(),
                Description = "Descripcion de producto",
                ImageUrl = "urlImage",
                Name = "Torta",
                ProductsMarkets = new List<ProductMarket>()
            };

            aMarket = new Market()
            {
                Id = Guid.NewGuid(),
                Longitude = 18.6f,
                Latitude = 17.2f,
                Logo = "miLogo.png",
                Name = "Disco Centro",
                OpeningTime = DateTime.Now,
                ClosingTime = DateTime.Now,
                ProductsMarkets = new List<ProductMarket>()
            };

            bestOption = new BestOptionRequest()
            {
                ProductId = aProduct.Id,
                Quantity = 10
            }; 

        }

        [TestMethod]
        public void TestGetMinimumAndMaximumPriceOk()
        {
            Tuple<double, double> minAndMaxPrice = new Tuple<double, double>(30.0, 50.0);
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Returns(minAndMaxPrice);
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            Tuple<double, double> result = productMarketLogic.GetMinimumAndMaximumPrice(Guid.NewGuid());
            productMarketMock.VerifyAll();
            Assert.AreEqual(minAndMaxPrice, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void TestGetMinimumAndMaximumPriceNotFound()
        {
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Throws(new ClientException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            productMarketLogic.GetMinimumAndMaximumPrice(Guid.NewGuid());
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void TestGetMinimumAndMaximumPriceInternalServerError()
        {
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Throws(new ServerException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            productMarketLogic.GetMinimumAndMaximumPrice(Guid.NewGuid());
        }

        [TestMethod]
        public void TestGetProductsAvailablesInMarketsOk()
        {
            List<Product> returnList = new List<Product>();
            returnList.Add(aProduct);
            returnList.Add(aProduct2);
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetProductsAvailablesInMarkets()).Returns(returnList);
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            List<Product> result = productMarketLogic.GetProductsAvailablesInMarkets();
            productMarketMock.VerifyAll();
            Assert.IsTrue(returnList.SequenceEqual(result));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void TestGetProductsAvailablesInMarketsNotFoundError()
        {
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetProductsAvailablesInMarkets()).Throws(new ClientException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            productMarketLogic.GetProductsAvailablesInMarkets();
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void TestGetProductsAvailablesInMarketsInternalServerError()
        {
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetProductsAvailablesInMarkets()).Throws(new ServerException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            productMarketLogic.GetProductsAvailablesInMarkets();
        }

        [TestMethod]
        public void TestGetProductsWithDiscountsOk()
        {
            ProductMarket aProductMarket = new ProductMarket()
            {
                Product = aProduct,
                Market = aMarket,
                ProductId = aProduct.Id,
                MarketId = aMarket.Id,
                QuantityAvailable = 1,
                CurrentPrice = 100.0,
                RegularPrice = 130.0
            };
            ProductMarket aProductMarket2 = new ProductMarket()
            {
                Product = aProduct2,
                Market = aMarket,
                ProductId = aProduct2.Id,
                MarketId = aMarket.Id,
                QuantityAvailable = 1,
                CurrentPrice = 50.0,
                RegularPrice = 100.0
            };
            List<ProductMarket> returnList = new List<ProductMarket>();
            returnList.Add(aProductMarket);
            returnList.Add(aProductMarket2);
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetProductsWithDiscounts()).Returns(returnList);
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            List<ProductMarket> result = productMarketLogic.GetProductsWithDiscounts();
            productMarketMock.VerifyAll();
            Assert.IsTrue(returnList.SequenceEqual(result));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void TestGetProductsWithDiscountsNotFoundError()
        {
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetProductsWithDiscounts()).Throws(new ClientException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            productMarketLogic.GetProductsWithDiscounts();
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void TestGetProductsWithDiscountsInternalServerError()
        {
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetProductsWithDiscounts()).Throws(new ServerException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            productMarketLogic.GetProductsWithDiscounts();
        }

        [TestMethod]
        public void TestGetProductsSearchedsOk()
        {
            List<Product> returnList = new List<Product>();
            returnList.Add(aProduct);
            returnList.Add(aProduct2);
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.SearchOfProductsByText(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>())).Returns(returnList);
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);

            List<Product> result = productMarketLogic.GetProductsBySearchText("a", 1, Guid.Empty);
            productMarketMock.VerifyAll();
            Assert.IsTrue(returnList.SequenceEqual(result));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void TestGetProductsOfSearchNotFoundError()
        {
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.SearchOfProductsByText(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>())).Throws(new ClientException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            productMarketLogic.GetProductsBySearchText("hola tio", 1, Guid.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void TestGetProductsOfSearchServerError()
        {
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.SearchOfProductsByText(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>())).Throws(new ServerException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            productMarketLogic.GetProductsBySearchText("hola tio");
        }

        [TestMethod]
        public void TestGetProductsSearchedByBarcodeOk()
        {

            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetProductByBarcode(It.IsAny<string>())).Returns(aProduct);
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);

            Product productObteined = productMarketLogic.GetProductByBarcode("1234567893");
            productMarketMock.VerifyAll();
            Assert.IsTrue(aProduct.Equals(productObteined));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void TestGetProductsOfSearchByBarcodeNotFoundError()
        {
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetProductByBarcode(It.IsAny<string>())).Throws(new ClientException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            productMarketLogic.GetProductByBarcode("123");
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void TestGetProductsOfSearchByBarcodeServerError()
        {
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.GetProductByBarcode(It.IsAny<string>())).Throws(new ServerException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            productMarketLogic.GetProductByBarcode("12");
        }


        [TestMethod]
        public void TestGetBestOption()
        {
            BestOptionRequest[] product = new BestOptionRequest[] { bestOption };
            List<Tuple<Market, double>> bestOptionsMarkets = new List<Tuple<Market, double>>();
            Tuple<Market, double> marketAndTotalPrice = new Tuple<Market, double>(aMarket, 2000);
            bestOptionsMarkets.Add(marketAndTotalPrice); 

            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.BestOptionToBuyProducts(It.IsAny<BestOptionRequest[]>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<int>())).Returns(bestOptionsMarkets);
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);

            List<Tuple<Market, double>> result = productMarketLogic.BestOptionToBuy(product, 15.0f, 15.0f, 10); 
            productMarketMock.VerifyAll();
            Assert.IsTrue(bestOptionsMarkets.SequenceEqual(result));
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void TestGetBestOptionNotFound()
        {
            BestOptionRequest[] product = new BestOptionRequest[] { bestOption };
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.BestOptionToBuyProducts(It.IsAny<BestOptionRequest[]>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<int>())).Throws(new ClientException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            List<Tuple<Market, double>> result = productMarketLogic.BestOptionToBuy(product, 15.0f, 15.0f, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void TestGetBestOptionServerException()
        {
            BestOptionRequest[] product = new BestOptionRequest[] { bestOption };
            var productMarketMock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            productMarketMock.Setup(m => m.BestOptionToBuyProducts(It.IsAny<BestOptionRequest[]>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<int>())).Throws(new ServerException());
            IProductMarketManagemenet productMarketLogic = new ProductMarketManagement(productMarketMock.Object);
            List<Tuple<Market, double>> result = productMarketLogic.BestOptionToBuy(product, 15.0f, 15.0f, 10);
        }

    }
}
