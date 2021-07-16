using DataAccess;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryException;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessTest
{
    [TestClass]
    public class MarketTest
    {
        Market aMarket;

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
                ClosingTime = DateTime.Now,
                ProductsMarkets = new List<ProductMarket>()
            };
        }
        [TestMethod]
        public void TestGetAllMarketsOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IRepository<Market> marketRepository = new BaseRepository<Market>(context);
            marketRepository.Add(aMarket);
            List<Market> marketList = new List<Market>();
            List<Market> marketsFromDb = marketRepository.GetAll().ToList();
            marketList.Add(aMarket);
            Assert.IsTrue(marketList.SequenceEqual(marketsFromDb));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetAllCategoriesNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IRepository<Market> marketRepository = new BaseRepository<Market>(context);
            marketRepository.GetAll(); 
        }
    }
}
