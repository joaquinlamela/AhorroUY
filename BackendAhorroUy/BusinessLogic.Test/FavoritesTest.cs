using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;
using System.Collections.Generic;


namespace BusinessLogic.Test
{        
    [TestClass]
    public class FavoritesTest
    {
        User user;
        User userWithFavorites; 
        Product aProduct;

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
        }

        [TestMethod]
        public void AddFavoriteProductTestOk()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);
            productRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(aProduct);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object, 
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.AddProduct(token, aProduct.Id); 
            
            userRepositoryMock.VerifyAll();
            userSessionRepositoryMock.VerifyAll();
            productRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void AddFavoriteProductTestNotFoundUser()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(value: null);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);
            productRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(aProduct);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.AddProduct(token, aProduct.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void AddFavoriteProductTestInternalServerErrorGettingUser()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(new ServerException());
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);
            productRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(aProduct);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.AddProduct(token, aProduct.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void AddFavoriteProductTestInternalServerErrorUpdate()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>())).Throws(new ServerException());
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);
            productRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(aProduct);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.AddProduct(token, aProduct.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void AddFavoriteProductTestInternalServerGettingToken()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Throws(new ServerException());
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);
            productRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(aProduct);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.AddProduct(token, aProduct.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void AddFavoriteProductTest()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);
            productRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(new ServerException());

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.AddProduct(token, aProduct.Id);
        }

        [TestMethod]
        public void GetFavoriteProductOfUserTestOk()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithFavorites(It.IsAny<Guid>())).Returns(userWithFavorites);
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            List<Product> listProductsObteined = favoritesManagement.GetAllFromUser(token);
            List<Product> listToCompare = new List<Product>() { aProduct};

            userRepositoryMock.VerifyAll();
            userSessionRepositoryMock.VerifyAll();
            CollectionAssert.AreEqual(listToCompare, listProductsObteined); 
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void GetFavoriteProductsOfUserNotFoundUserTest()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithFavorites(It.IsAny<Guid>())).Returns(value: null);
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.GetAllFromUser(token);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetFavoriteProductsOfUserInternalServerErrorTest()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithFavorites(It.IsAny<Guid>())).Throws(new ServerException());
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.GetAllFromUser(token);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetFavoriteProductsOfUserInternalServerErrorWithGetTokenTest()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithFavorites(It.IsAny<Guid>())).Returns(userWithFavorites);
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Throws(new ServerException());
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.GetAllFromUser(token);
        }

        [TestMethod]
        public void RemoveFavoriteProductTestOk()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithFavorites(It.IsAny<Guid>())).Returns(userWithFavorites);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.DeleteProduct(token, aProduct.Id);

            userRepositoryMock.VerifyAll();
            userSessionRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void RemoveFavoriteProductTestNotFoundUser()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithFavorites(It.IsAny<Guid>())).Returns(value: null);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.DeleteProduct(token, aProduct.Id);
        }


        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void RemoveFavoriteProductTestInternalServerErrorUser()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithFavorites(It.IsAny<Guid>())).Throws(new ServerException());
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.DeleteProduct(token, aProduct.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void RemoveFavoriteProductTestInternalServerErrorToken()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithFavorites(It.IsAny<Guid>())).Returns(userWithFavorites);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Throws(new ServerException());
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.DeleteProduct(token, aProduct.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void RemoveFavoriteProductTestInternalServerErrorUpdate()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithFavorites(It.IsAny<Guid>())).Returns(userWithFavorites);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>())).Throws(new ServerException());
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var productRepositoryMock = new Mock<IRepository<Product>>(MockBehavior.Strict);

            IFavoritesManagement favoritesManagement = new FavoritesManagement(userSessionRepositoryMock.Object,
                userRepositoryMock.Object, productRepositoryMock.Object);

            favoritesManagement.DeleteProduct(token, aProduct.Id);
        }
    }
}
