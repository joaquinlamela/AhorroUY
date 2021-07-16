using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Interface;
using BusinessLogicException;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi.Controllers;
using WebApiModels.DataTypes.MarketDTs;

namespace WebApiTest
{
    [TestClass]
    public class BestOptionControllerTest
    {
        BestOptionRequest bestOption;
        Product aProduct;
        Market aMarket;


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

            bestOption = new BestOptionRequest()
            {
                ProductId = aProduct.Id,
                Quantity = 10
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


        }
        [TestMethod]
        public void BestOptionToBuyOk()
        {
            BestOptionRequest[] product = new BestOptionRequest[] { bestOption };
            List<Tuple<Market, double>> bestOptionsMarkets = new List<Tuple<Market, double>>();
            Tuple<Market, double> marketAndTotalPrice = new Tuple<Market, double>(aMarket, 2000);
            bestOptionsMarkets.Add(marketAndTotalPrice);
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.BestOptionToBuy(It.IsAny<BestOptionRequest[]>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<int>())).Returns(bestOptionsMarkets);
            BestOptionController bestOptionController = new BestOptionController(mock.Object);
            var result = bestOptionController.BestMarketOptionForProducts("829a4761-232e-49b3-96e3-6ba6413b6099", product, 15.0f, 15.0f, 10);
            var createdResult = result as OkObjectResult;
            var resultValue = createdResult.Value as List<BestOptionResponse>;
            mock.VerifyAll();
            BestOptionResponse response = new BestOptionResponse()
            {
                MarketName = aMarket.Name,
                MarketAddress = aMarket.Address,
                PriceForProducts = 2000,
                MarketLongitude = aMarket.Longitude,
                MarketLatitude = aMarket.Latitude,
                OpenTimeToday = aMarket.OpeningTime,
                CloseTimeToday = aMarket.ClosingTime,
                MarketLogo = aMarket.Logo
            };
            response.CalculateDifferenceToCloseInHours();
            List<BestOptionResponse> expected = new List<BestOptionResponse>();
            expected.Add(response);
            Assert.IsTrue(resultValue.SequenceEqual(expected));
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void BestOptionToBuyNotFound()
        {
            BestOptionRequest[] product = new BestOptionRequest[] { bestOption };
            var mock = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mock.Setup(m => m.BestOptionToBuy(It.IsAny<BestOptionRequest[]>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<int>())).Throws(new DomainBusinessLogicException(""));
            BestOptionController bestOptionController = new BestOptionController(mock.Object);
            bestOptionController.BestMarketOptionForProducts("829a4761-232e-49b3-96e3-6ba6413b6099", product, 15.0f, 15.0f, 10);
        }
    }
}
