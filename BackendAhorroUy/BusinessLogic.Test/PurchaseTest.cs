using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Test
{
    [TestClass]
    public class PurchaseTest
    {

        User user;
        User userWithFavorites;
        Product aProduct;
        Purchase purchase; 

        [TestInitialize]
        public void SetUp()
        {
            user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "colo123"
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
            userWithFavorites = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "colo123",
                Favorites = new List<Product>() { aProduct }
            };

            purchase = new Purchase()
            {
                Id = Guid.NewGuid(),
                Amount = 10,
                MarketName = "kinko",
                MarketAddress = "Avenida siempre viva 123"
            };
        }

        [TestMethod]
        public void AddANewPurchaseOk()
        {
            Guid token = Guid.NewGuid(); 

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var purchaseRepositoryMock = new Mock<IRepository<Purchase>>(MockBehavior.Strict);
            purchaseRepositoryMock.Setup(m => m.Add(It.IsAny<Purchase>()));
            var couponsLogicMock = new Mock<ICouponManagement>(MockBehavior.Strict);
            couponsLogicMock.Setup(m => m.TryGenerate(It.IsAny<string>(), It.IsAny<int>())); 

            IPurchaseManagement purchaseManagement = new PurchaseManagement(purchaseRepositoryMock.Object,
                userSessionRepositoryMock.Object,
                userRepositoryMock.Object, couponsLogicMock.Object);

            Purchase result = purchaseManagement.SavePurchase(token, purchase);

            userRepositoryMock.VerifyAll();
            userSessionRepositoryMock.VerifyAll();
            purchaseRepositoryMock.VerifyAll();
            couponsLogicMock.VerifyAll();
            Assert.AreEqual(purchase, result);
        }


        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void FailingOnAddNewPurchaseOfNullUser()
        {
            Guid token = Guid.NewGuid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(value: null);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var purchaseRepositoryMock = new Mock<IRepository<Purchase>>(MockBehavior.Strict);
            purchaseRepositoryMock.Setup(m => m.Add(It.IsAny<Purchase>()));
            var couponsLogicMock = new Mock<ICouponManagement>(MockBehavior.Strict);
            couponsLogicMock.Setup(m => m.TryGenerate(It.IsAny<string>(), It.IsAny<int>()));

            IPurchaseManagement purchaseManagement = new PurchaseManagement(purchaseRepositoryMock.Object,
                userSessionRepositoryMock.Object,
                userRepositoryMock.Object, couponsLogicMock.Object);

            purchaseManagement.SavePurchase(token, purchase);
        }
    }
}


