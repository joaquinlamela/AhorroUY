using BusinessLogic.Interface;
using BusinessLogicException;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Controllers;
using WebApi.DataTypes.ForResponse.FavoriteDTs;

namespace WebApiTest
{
    [TestClass]
    public class FavoritesControllerTest
    {
        Product aProduct;
        Product aProduct2;
        FavoritesGetResponse favoriteProduct1;
        FavoritesGetResponse favoriteProduct2; 

        [TestInitialize]
        public void SetUp()
        {
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

            favoriteProduct1 = new FavoritesGetResponse()
            {
                Id = aProduct.Id,
                Description = aProduct.Description,
                Name = aProduct.Name,
                ImageUrl = aProduct.ImageUrl,
                MaxPrice = 100,
                MinPrice = 50
            };

            favoriteProduct2 = new FavoritesGetResponse()
            {
                Id = aProduct2.Id,
                Description = aProduct2.Description,
                Name = aProduct2.Name,
                ImageUrl = aProduct2.ImageUrl,
                MaxPrice = 100,
                MinPrice = 50
            };
        }

        [TestMethod]
        public void GetAllProductsFavoritesOfUserTestOk()
        {
            List<Product> favoritesProductsOfUser = new List<Product>() { aProduct, aProduct2 };
            Tuple<double, double> minAndMaxPrice = new Tuple<double, double>(50, 100);

            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllFromUser(It.IsAny<Guid>())).Returns(favoritesProductsOfUser);
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mockProductMarket.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Returns(minAndMaxPrice); 

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.GetAll(Guid.NewGuid().ToString());
            var createdResult = result as OkObjectResult;
            var resultValue = createdResult.Value as List<FavoritesGetResponse>;
            List<FavoritesGetResponse> listExpected = new List<FavoritesGetResponse>() { favoriteProduct1, favoriteProduct2 }; 

            mock.VerifyAll();
            mockProductMarket.VerifyAll();
            Assert.IsTrue(listExpected.SequenceEqual(resultValue));
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void GetAllProductsFavoritesOfUserNotFoundTest()
        {
            List<Product> favoritesProductsOfUser = new List<Product>() { aProduct, aProduct2 };
            Tuple<double, double> minAndMaxPrice = new Tuple<double, double>(50, 100);

            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllFromUser(It.IsAny<Guid>())).Throws(new DomainBusinessLogicException(""));
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mockProductMarket.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Returns(minAndMaxPrice);

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.GetAll(Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetAllProductsFavoritesOfUserErrorTest()
        {
            List<Product> favoritesProductsOfUser = new List<Product>() { aProduct, aProduct2 };
            Tuple<double, double> minAndMaxPrice = new Tuple<double, double>(50, 100);

            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllFromUser(It.IsAny<Guid>())).Throws(new ClientBusinessLogicException());
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mockProductMarket.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Returns(minAndMaxPrice);

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.GetAll(Guid.NewGuid().ToString());
        }


        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetAllProductsFavoritesOfUserInternalServerErrorTest()
        {
            List<Product> favoritesProductsOfUser = new List<Product>() { aProduct, aProduct2 };
            Tuple<double, double> minAndMaxPrice = new Tuple<double, double>(50, 100);

            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllFromUser(It.IsAny<Guid>())).Throws(new ServerException());
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mockProductMarket.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Returns(minAndMaxPrice);

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.GetAll(Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetAllProductsFavoritesOfUserNotFoundProductPriceTest()
        {
            List<Product> favoritesProductsOfUser = new List<Product>() { aProduct, aProduct2 };
            Tuple<double, double> minAndMaxPrice = new Tuple<double, double>(50, 100);

            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllFromUser(It.IsAny<Guid>())).Returns(favoritesProductsOfUser);
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mockProductMarket.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Throws(new ClientBusinessLogicException());

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.GetAll(Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetAllProductsFavoritesOfUserInternalServerErrorMinAndMaxTest()
        {
            List<Product> favoritesProductsOfUser = new List<Product>() { aProduct, aProduct2 };
            Tuple<double, double> minAndMaxPrice = new Tuple<double, double>(50, 100);

            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllFromUser(It.IsAny<Guid>())).Returns(favoritesProductsOfUser);
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);
            mockProductMarket.Setup(m => m.GetMinimumAndMaximumPrice(It.IsAny<Guid>())).Throws(new ServerException());

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.GetAll(Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void PostFavoriteProductTestOk()
        {
            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.AddProduct(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(aProduct); 
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.Post(Guid.NewGuid().ToString(), aProduct.Id.ToString());
            var createdResult = result as CreatedResult;
            var favoriteResult = createdResult.Value as FavoritesPostResponse; 

            FavoritesPostResponse expected = new FavoritesPostResponse()
            {
                Id = aProduct.Id,
                Name = aProduct.Name,
                Description = aProduct.Description,
                ImageUrl = aProduct.ImageUrl,
                MinPrice = 0,
                MaxPrice = 0,
                IsFavorite = true
            };

            mock.VerifyAll();
            Assert.AreEqual(expected, favoriteResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void PostFavoriteProductTestInternalServerError()
        {
            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.AddProduct(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new ServerException());
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.Post(Guid.NewGuid().ToString(), aProduct.Id.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void PostFavoriteProductTestNotFound()
        {
            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.AddProduct(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new ClientBusinessLogicException());
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.Post(Guid.NewGuid().ToString(), aProduct.Id.ToString());
        }

        [TestMethod]
        public void DeleteFavoriteProductTestOk()
        {
            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.DeleteProduct(It.IsAny<Guid>(), It.IsAny<Guid>()));
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.Delete(Guid.NewGuid().ToString(), aProduct.Id.ToString());
            var createdResult = result as OkObjectResult;

            mock.VerifyAll();
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void DeleteFavoriteProductTestInternalServerError()
        {
            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.DeleteProduct(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new ServerException());
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.Delete(Guid.NewGuid().ToString(), aProduct.Id.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void DeleteFavoriteProductTestNotFoundProduct()
        {
            var mock = new Mock<IFavoritesManagement>(MockBehavior.Strict);
            mock.Setup(m => m.DeleteProduct(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new ClientBusinessLogicException());
            var mockProductMarket = new Mock<IProductMarketManagemenet>(MockBehavior.Strict);

            FavoritesController productMarketController = new FavoritesController(mock.Object, mockProductMarket.Object);
            var result = productMarketController.Delete(Guid.NewGuid().ToString(), aProduct.Id.ToString());
        }
    }
}
