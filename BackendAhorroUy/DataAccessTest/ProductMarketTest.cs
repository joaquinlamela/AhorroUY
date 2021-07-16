using DataAccess;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryException;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiModels.DataTypes.MarketDTs;

namespace DataAccessTest
{
    [TestClass]
    public class ProductMarketTest
    {
        Market aMarket;
        Market aMarket2;
        Product aProduct;
        Product aProduct2;
        Product chocolateCake;
        ProductMarket aProductMarket;
        ProductMarket aProductMarket2;
        ProductMarket aProductMarket3;
        ProductMarket aProductMarket4;
        ProductMarket aProductMarket5;


        [TestInitialize]
        public void SetUp()
        {
            aMarket = new Market()
            {
                Id = Guid.NewGuid(),
                Longitude = 18.6f,
                Latitude = 17.2f,
                Logo = "miLogo.png",
                Name = "Disco Centro",
                OpeningTime = DateTime.Now,
                ClosingTime = new DateTime(2021, 06, 13, 23, 00, 00),
                ProductsMarkets = new List<ProductMarket>()
            };

            aMarket2 = new Market()
            {
                Id = Guid.NewGuid(),
                Longitude = 30.6f,
                Latitude = 40.0f,
                Logo = "miLogo.png",
                Name = "Tienda Inglesa",
                OpeningTime = DateTime.Now,
                ClosingTime = new DateTime(2021, 06, 13, 23, 00, 00),
                ProductsMarkets = new List<ProductMarket>()
            };

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
                Barcode = "1234567899",
                Category = new Category(),
                Description = "Descripcion de producto",
                ImageUrl = "urlImage",
                Name = "Torta",
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
                Market = aMarket2,
                Product = aProduct2,
                MarketId = aMarket2.Id,
                ProductId = aProduct2.Id,
                QuantityAvailable = 2,
                CurrentPrice = 70.0,
                RegularPrice = 70.0
            };

            aProductMarket3 = new ProductMarket()
            {
                Market = aMarket,
                Product = aProduct2,
                MarketId = aMarket.Id,
                ProductId = aProduct2.Id,
                QuantityAvailable = 2,
                CurrentPrice = 125.0,
                RegularPrice = 150.0
            };

            chocolateCake = new Product()
            {
                Id = Guid.NewGuid(),
                Barcode = "1234567893",
                Category = new Category(),
                Description = "Descripcion de producto",
                ImageUrl = "urlImage",
                Name = "Torta de chocolate",
                ProductsMarkets = new List<ProductMarket>()
            };

            aProductMarket4 = new ProductMarket()
            {
                Market = aMarket,
                Product = chocolateCake,
                MarketId = aMarket.Id,
                ProductId = chocolateCake.Id,
                QuantityAvailable = 2,
                CurrentPrice = 150.0,
                RegularPrice = 150.0
            };

            aProductMarket5 = new ProductMarket()
            {
                Market = aMarket2,
                Product = chocolateCake,
                MarketId = aMarket2.Id,
                ProductId = chocolateCake.Id,
                QuantityAvailable = 2,
                CurrentPrice = 100.0,
                RegularPrice = 125.0
            };

        }

        [TestMethod]
        public void TestGetAllProductMarketsOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());

            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            ProductMarket productMarket = new ProductMarket()
            {
                Market = aMarket,
                Product = aProduct,
                ProductId = aProduct.Id,
                MarketId = aMarket.Id,
                QuantityAvailable = 2,
                CurrentPrice = 100,
                RegularPrice = 110
            };
            productMarketRepository.Add(productMarket);

            List<ProductMarket> productMarketFromDb = productMarketRepository.GetAll().ToList();
            Assert.IsTrue(productMarket.Equals(productMarketFromDb.First()));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetAllProductMarketsNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);
            productMarketRepository.GetAll();
        }

        [TestMethod]
        public void TestGetAllDistinctsProductMarketOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());

            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            ProductMarket productMarket = new ProductMarket()
            {
                Market = aMarket,
                Product = aProduct,
                ProductId = aProduct.Id,
                MarketId = aMarket.Id,
                QuantityAvailable = 2,
                CurrentPrice = 100,
                RegularPrice = 110
            };
            productMarketRepository.Add(productMarket);

            List<Product> productMarketFromDb = productMarketRepository.GetProductsAvailablesInMarkets();
            Assert.IsTrue(productMarket.Product.Equals(productMarketFromDb.First()));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetAllDistinctsProductMarketNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);
            productMarketRepository.GetProductsAvailablesInMarkets();
        }


        [TestMethod]
        public void TestGetMinimumAndMaximumPriceOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);
            ProductMarket productMarket = new ProductMarket()
            {
                Market = aMarket,
                Product = aProduct,
                ProductId = aProduct.Id,
                MarketId = aMarket.Id,
                QuantityAvailable = 2,
                CurrentPrice = 100.0,
                RegularPrice = 110.0
            };

            ProductMarket productMarket2 = new ProductMarket()
            {
                Market = aMarket2,
                Product = aProduct,
                ProductId = aProduct.Id,
                MarketId = aMarket2.Id,
                QuantityAvailable = 2,
                CurrentPrice = 90.0,
                RegularPrice = 110.0
            };
            productMarketRepository.Add(productMarket);
            productMarketRepository.Add(productMarket2);

            Tuple<double, double> result = new Tuple<double, double>(90.0, 100.0);

            Assert.AreEqual(result, productMarketRepository.GetMinimumAndMaximumPrice(aProduct.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetMinimumAndMaximumPriceNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);
            productMarketRepository.GetMinimumAndMaximumPrice(aProduct.Id);
        }

        [TestMethod]
        public void TestGetProductsWithDiscountsOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            productMarketRepository.Add(aProductMarket);
            productMarketRepository.Add(aProductMarket2);

            List<ProductMarket> productsWithDiscount = productMarketRepository.GetProductsWithDiscounts();
            List<ProductMarket> productsExpected = new List<ProductMarket>();
            productsExpected.Add(aProductMarket);

            CollectionAssert.AreEqual(productsExpected, productsWithDiscount);

        }

        [TestMethod]
        public void TestGetProductsWithDiscountsTwoProductsOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            productMarketRepository.Add(aProductMarket);
            aProductMarket2.CurrentPrice = 20.0;
            productMarketRepository.Add(aProductMarket2);

            List<ProductMarket> productsWithDiscount = productMarketRepository.GetProductsWithDiscounts();
            List<ProductMarket> productsExpected = new List<ProductMarket>();


            productsExpected.Add(aProductMarket2);
            productsExpected.Add(aProductMarket);

            CollectionAssert.AreEqual(productsExpected, productsWithDiscount);
        }

        [TestMethod]
        public void TestGetTheSameProductWithDiscountInManyMarketsOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);


            productMarketRepository.Add(aProductMarket);
            aProductMarket2.CurrentPrice = 20.0;
            aProductMarket2.Product = aProduct;
            aProductMarket2.ProductId = aProduct.Id;
            productMarketRepository.Add(aProductMarket2);

            List<ProductMarket> productsWithDiscount = productMarketRepository.GetProductsWithDiscounts();
            List<ProductMarket> productsExpected = new List<ProductMarket>();
            productsExpected.Add(aProductMarket2);

            CollectionAssert.AreEqual(productsExpected, productsWithDiscount);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetProductsWithDiscountNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);
            productMarketRepository.GetProductsWithDiscounts();
        }

        [TestMethod]
        public void TestGetProductsByTextOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            productMarketRepository.Add(aProductMarket2);
            productMarketRepository.Add(aProductMarket3);

            List<Product> productsObteined = productMarketRepository.SearchOfProductsByText("tor", 1, Guid.Empty);
            List<Product> productsExpected = new List<Product>() { aProduct2 };

            CollectionAssert.AreEqual(productsExpected, productsObteined);

        }

        [TestMethod]
        public void TestGetProductsByEmptyText()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            productMarketRepository.Add(aProductMarket2);
            productMarketRepository.Add(aProductMarket3);

            List<Product> productsObteined = productMarketRepository.SearchOfProductsByText("", 1, Guid.Empty);
            List<Product> productsExpected = new List<Product>() { aProduct2 };

            CollectionAssert.AreEqual(productsExpected, productsObteined);
        }


        [TestMethod]
        public void TestGetAProductByTextOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            productMarketRepository.Add(aProductMarket);

            List<Product> productsObteined = productMarketRepository.SearchOfProductsByText("a", 0, Guid.Empty);
            List<Product> productsExpected = new List<Product>() { aProduct };

            CollectionAssert.AreEqual(productsExpected, productsObteined);
        }
        [TestMethod]
        public void TestGetProductsALotsByTextOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            productMarketRepository.Add(aProductMarket);
            productMarketRepository.Add(aProductMarket2);

            List<Product> productsObteined = productMarketRepository.SearchOfProductsByText("a", 0, Guid.Empty);
            List<Product> productsExpected = new List<Product>() { aProduct, aProduct2 };

            CollectionAssert.AreEqual(productsExpected, productsObteined);
        }

        [TestMethod]
        public void TestGetProductsWithCakesByTextOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            productMarketRepository.Add(aProductMarket2);
            productMarketRepository.Add(aProductMarket3);
            productMarketRepository.Add(aProductMarket4);
            productMarketRepository.Add(aProductMarket5);

            List<Product> productsObteined = productMarketRepository.SearchOfProductsByText("torta", 2, Guid.Empty);
            List<Product> productsExpected = new List<Product>() { aProduct2, chocolateCake };

            CollectionAssert.AreEqual(productsExpected, productsObteined);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetProductNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);
            productMarketRepository.Add(aProductMarket);
            productMarketRepository.SearchOfProductsByText("tio", 0, Guid.Empty);
        }

        [TestMethod]
        public void TestGetProductsWithCakesByMayorPriceOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            productMarketRepository.Add(aProductMarket2);
            productMarketRepository.Add(aProductMarket3);
            productMarketRepository.Add(aProductMarket4);
            productMarketRepository.Add(aProductMarket5);

            List<Product> productsObteined = productMarketRepository.SearchOfProductsByText("torta", 0, Guid.Empty);
            List<Product> productsExpected = new List<Product>() { chocolateCake, aProduct2 };

            CollectionAssert.AreEqual(productsExpected, productsObteined);
        }

        [TestMethod]
        public void TestGetProductsWithCakesByMinorPriceOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            productMarketRepository.Add(aProductMarket2);
            productMarketRepository.Add(aProductMarket3);
            productMarketRepository.Add(aProductMarket4);
            productMarketRepository.Add(aProductMarket5);

            List<Product> productsObteined = productMarketRepository.SearchOfProductsByText("torta", 2, Guid.Empty);
            List<Product> productsExpected = new List<Product>() { aProduct2, chocolateCake };

            CollectionAssert.AreEqual(productsExpected, productsObteined);
        }



        [TestMethod]
        public void TestGetAProductByBarcodeOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);

            productMarketRepository.Add(aProductMarket);

            Product productObteined = productMarketRepository.GetProductByBarcode("1234567893");
            Product productExpected = aProduct;

            Assert.AreEqual(productExpected, productObteined);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetProductNotFoundByBarcode()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);
            productMarketRepository.Add(aProductMarket);
            productMarketRepository.GetProductByBarcode("123");
        }


        [TestMethod]
        public void TestBestOptionToBuyProductsOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);
            productMarketRepository.Add(aProductMarket);
            BestOptionRequest bestOption = new BestOptionRequest()
            {
                ProductId = aProduct.Id,
                Quantity = 1
            };
            BestOptionRequest[] request = new BestOptionRequest[] { bestOption };
            List<Tuple<Market, double>> expected = new List<Tuple<Market, double>>();
            Tuple<Market, double> option = new Tuple<Market, double>(aMarket, 110);
            expected.Add(option);
            List<Tuple<Market, double>> result = productMarketRepository.BestOptionToBuyProducts(request, 10.0f, 10.0f, 1000000);
            Assert.IsTrue(result.SequenceEqual(expected));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestBestOptionToBuyProductsNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IProductMarketRepository productMarketRepository = new ProductMarketRepository(context);
            productMarketRepository.Add(aProductMarket);
            BestOptionRequest bestOption = new BestOptionRequest()
            {
                ProductId = aProduct.Id,
                Quantity = 1
            };
            BestOptionRequest[] request = new BestOptionRequest[] { bestOption };
            List<Tuple<Market, double>> expected = new List<Tuple<Market, double>>();
            Tuple<Market, double> option = new Tuple<Market, double>(aMarket, 110);
            expected.Add(option);
            List<Tuple<Market, double>> result = productMarketRepository.BestOptionToBuyProducts(request, 10.0f, 10.0f, 1);
        }
    }
}
