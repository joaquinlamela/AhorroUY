using BusinessLogic;
using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Controllers;
using WebApi.DataTypes.ForResponse.ProductDTs;

namespace WebApiTest
{
    [TestClass]
    public class ProductMarketControllerTest
    {
        Product aProduct;
        Product aProduct2;
        ProductMarket aProductMarket;
        ProductMarket aProductMarket2;
        Market aMarket;
        DiscountProductModel productWithDiscount; 

        [TestInitialize]
        public void SetUp()
        {
            aMarket = new Market()
            {
                Id = Guid.NewGuid(),
                Longitude = 18.6f,
                Latitude = 17.2f,
                OpeningTime = DateTime.Now,
                ClosingTime = DateTime.Now,
                Logo = "image.png",
                Name = "Disco centro",
                ProductsMarkets = new List<ProductMarket>()
            };
            aProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Azucar",
                Category = new Category(),
                Barcode = "1234567895",
                Description = "La mejor azucar",
                ImageUrl = "urlImage",
                ProductsMarkets = new List<ProductMarket>()
            };
            aProduct2 = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Harina",
                Category = new Category(),
                Barcode = "1234567897",
                Description = "La mejor harina",
                ImageUrl = "urlImageHarina",
                ProductsMarkets = new List<ProductMarket>()
            };
            aProductMarket = new ProductMarket()
            {
                Market = aMarket,
                Product = aProduct,
                MarketId = aMarket.Id,
                ProductId = aProduct.Id,
                QuantityAvailable = 2,
                CurrentPrice = 110.0,
                RegularPrice = 120.0
            };

            aProductMarket2 = new ProductMarket()
            {
                Market = aMarket,
                Product = aProduct2,
                MarketId = aMarket.Id,
                ProductId = aProduct2.Id,
                QuantityAvailable = 2,
                CurrentPrice = 50.0,
                RegularPrice = 50.0
            };

            productWithDiscount = DiscountProductModel.ToModel(aProductMarket);
            productWithDiscount.MinPrice = 50;
            productWithDiscount.MaxPrice = 100;
        }

        [TestMethod]
        public void GetAllProductsAvailableOk()
        {
            Tuple<double, double> expected = new Tuple<double, double>(10.0, 90.0);
            List<Product> productsExpected = new List<Product>();
            productsExpected.Add(aProduct);
            productsExpected.Add(aProduct2);
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductsAvailablesInMarkets()).Returns(productsExpected);
            mock.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Returns(expected);
            ProductsMarketsController productMarketController = new ProductsMarketsController(mock.Object);

            var result = productMarketController.Get();
            var createdResult = result as OkObjectResult;
            IEnumerable<ProductModel> listOfProductsObteined = createdResult.Value as IEnumerable<ProductModel>;
            mock.VerifyAll();
            IEnumerable<ProductModel> expectedResult = ProductModel.ToModel(productsExpected);
            expectedResult.ElementAt(0).MinPrice = expected.Item1;
            expectedResult.ElementAt(0).MaxPrice = expected.Item2;

            expectedResult.ElementAt(1).MinPrice = expected.Item1;
            expectedResult.ElementAt(1).MaxPrice = expected.Item2;
            Assert.IsTrue(expectedResult.SequenceEqual(listOfProductsObteined));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetAllProductsAvailableNotFound()
        {
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductsAvailablesInMarkets()).Throws(new ClientBusinessLogicException());
            ProductsMarketsController productMarketController = new ProductsMarketsController(mock.Object);

            productMarketController.Get();
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetAllProductsAvailableInternalServerError()
        {
            var mock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductsAvailablesInMarkets()).Throws(new ServerException());
            IProductMarketManagemenet productMarket = new ProductMarketManagement(mock.Object);            
            ProductsMarketsController productMarketController = new ProductsMarketsController(productMarket);

            productMarketController.Get();
        }

        [TestMethod]
        public void GetProductsWithDiscountOk()
        {
            Tuple<double, double> minAndMaxPrice = new Tuple<double, double>(50, 100); 
            List<ProductMarket> productsExpected = new List<ProductMarket>();
            productsExpected.Add(aProductMarket);
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductsWithDiscounts()).Returns(productsExpected);
            mock.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Returns(minAndMaxPrice); 
            ProductsMarketsController productMarketController = new ProductsMarketsController(mock.Object);

            var result = productMarketController.GetProductsWithDiscount();
            var createdResult = result as OkObjectResult;
            IEnumerable<DiscountProductModel> listOfProductsObteined = createdResult.Value as IEnumerable<DiscountProductModel>;
            List<DiscountProductModel> productWithDiscountExpected = new List<DiscountProductModel>() { productWithDiscount }; 
            
            mock.VerifyAll();
            Assert.IsTrue(productWithDiscountExpected.SequenceEqual(listOfProductsObteined));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetProductsWithDiscountNotFound()
        {
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductsWithDiscounts()).Throws(new ClientBusinessLogicException());
            ProductsMarketsController productMarketController = new ProductsMarketsController(mock.Object);

            productMarketController.GetProductsWithDiscount();
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetProductsWithDiscountInternalServerError()
        {
            var mock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductsWithDiscounts()).Throws(new ServerException());
            IProductMarketManagemenet productMarket = new ProductMarketManagement(mock.Object);
            ProductsMarketsController productMarketController = new ProductsMarketsController(productMarket);

            productMarketController.GetProductsWithDiscount();
        }

        [TestMethod]
        public void GetProductsOfSearchOk()
        {

            Tuple<double, double> expected = new Tuple<double, double>(110.0, 120.0);
            List<Product> productsExpected = new List<Product>();
            productsExpected.Add(aProduct);
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductsBySearchText(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>())).Returns(productsExpected);
            mock.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Returns(expected);
            ProductsMarketsController productMarketController = new ProductsMarketsController(mock.Object);

            var result = productMarketController.GetProductsBySearchText("Azucar", 0, Guid.Empty.ToString());
            var createdResult = result as OkObjectResult;
            IEnumerable<ProductModel> listOfProductsObteined = createdResult.Value as IEnumerable<ProductModel>;
            mock.VerifyAll();
            IEnumerable<ProductModel> expectedResult = ProductModel.ToModel(productsExpected);
            expectedResult.ElementAt(0).MinPrice = expected.Item1;
            expectedResult.ElementAt(0).MaxPrice = expected.Item2;

            Assert.IsTrue(expectedResult.SequenceEqual(listOfProductsObteined));
        }


        [TestMethod]
        public void GetProductsWithRealGuidOfSearchOk()
        {

            Tuple<double, double> expected = new Tuple<double, double>(110.0, 120.0);
            List<Product> productsExpected = new List<Product>();
            productsExpected.Add(aProduct);
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductsBySearchText(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>())).Returns(productsExpected);
            mock.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Returns(expected);
            ProductsMarketsController productMarketController = new ProductsMarketsController(mock.Object);

            var result = productMarketController.GetProductsBySearchText("Azucar", 0, "551A8F85-8ADF-4285-B6FB-0D73DD0E93B2");
            var createdResult = result as OkObjectResult;
            IEnumerable<ProductModel> listOfProductsObteined = createdResult.Value as IEnumerable<ProductModel>;
            mock.VerifyAll();
            IEnumerable<ProductModel> expectedResult = ProductModel.ToModel(productsExpected);
            expectedResult.ElementAt(0).MinPrice = expected.Item1;
            expectedResult.ElementAt(0).MaxPrice = expected.Item2;

            Assert.IsTrue(expectedResult.SequenceEqual(listOfProductsObteined));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetAllProductsOfSearchNotFound()
        {
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductsBySearchText(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>())).Throws(new ClientBusinessLogicException());
            ProductsMarketsController productMarketController = new ProductsMarketsController(mock.Object);

            productMarketController.GetProductsBySearchText("Azucar",0, Guid.Empty.ToString()); 
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetAllProductsOfSearchInternalServerError()
        {
            var mock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            mock.Setup(m => m.SearchOfProductsByText(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>())).Throws(new ServerException());
            IProductMarketManagemenet productMarket = new ProductMarketManagement(mock.Object);
            ProductsMarketsController productMarketController = new ProductsMarketsController(productMarket);

            productMarketController.GetProductsBySearchText("Azucar", 0, Guid.Empty.ToString());
        }


        [TestMethod]
        public void GetProductsOfSearchByBarcodeOk()
        {

            Tuple<double, double> expected = new Tuple<double, double>(110.0, 120.0);
            Product productExpected = aProduct;
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductByBarcode(It.IsAny<string>())).Returns(productExpected);
            mock.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Returns(expected);
            ProductsMarketsController productMarketController = new ProductsMarketsController(mock.Object);

            var result = productMarketController.GetProductByBarcode("1234567895");
            var createdResult = result as OkObjectResult;
            ProductModel productObteined = createdResult.Value as ProductModel;
            mock.VerifyAll();
            ProductModel expectedResultProduct = ProductModel.ToModel(productExpected);
            expectedResultProduct.MinPrice = expected.Item1;
            expectedResultProduct.MaxPrice = expected.Item2;

            Assert.IsTrue(expectedResultProduct.Equals(productObteined));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetAllProductsOfSearchByBarcodeNotFound()
        {
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductByBarcode(It.IsAny<string>())).Throws(new ClientBusinessLogicException());
            ProductsMarketsController productMarketController = new ProductsMarketsController(mock.Object);

            productMarketController.GetProductByBarcode("1234567895");
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetAllProductsOfSearchByBarcodeInternalServerError()
        {
            var mock = new Mock<IProductMarketRepository>(MockBehavior.Strict);
            mock.Setup(m => m.GetProductByBarcode(It.IsAny<string>())).Throws(new ServerException());
            IProductMarketManagemenet productMarket = new ProductMarketManagement(mock.Object);
            ProductsMarketsController productMarketController = new ProductsMarketsController(productMarket);

            productMarketController.GetProductByBarcode("1234567895");
        }
    }
}
