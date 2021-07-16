using System;
using BusinessLogic.Interface;
using BusinessLogicException;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi.Controllers;

namespace WebApiTest
{

    [TestClass]
    public class PurchaseControllerTest
    {
        Purchase aPurchase;

        [TestInitialize]
        public void SetUp()
        {
            aPurchase = new Purchase()
            {
                Id = Guid.NewGuid(),
                Amount = 1000,
                MarketName = "Disco",
                MarketAddress = "Montevideo",
                PurchaseDate = DateTime.Now
            };
        }

        [TestMethod]
        public void PostPurchaseOk()
        {
            var mock = new Mock<IPurchaseManagement>(MockBehavior.Strict);
            mock.Setup(m => m.SavePurchase(It.IsAny<Guid>(), It.IsAny<Purchase>())).Returns(aPurchase);
            PurchaseController purchaseController = new PurchaseController(mock.Object);
            var result = purchaseController.SavePurchase("829a4761-232e-49b3-96e3-6ba6413b6099", aPurchase);
            var createdResult = result as OkObjectResult;
            var resultValue = createdResult.Value as Purchase;
            mock.VerifyAll();
            Assert.IsTrue(resultValue.Equals(aPurchase));
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void PostPurchaseFailUserInvalid()
        {
            var mock = new Mock<IPurchaseManagement>(MockBehavior.Strict);
            mock.Setup(m => m.SavePurchase(It.IsAny<Guid>(), It.IsAny<Purchase>())).Throws(new DomainBusinessLogicException(""));
            PurchaseController purchaseController = new PurchaseController(mock.Object);
            var result = purchaseController.SavePurchase("829a4761-232e-49b3-96e3-6ba6413b6099", aPurchase);
            var createdResult = result as OkObjectResult;
            var resultValue = createdResult.Value as Purchase;
            mock.VerifyAll();
        }
    }
}
