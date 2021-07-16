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
    public class CouponTest
    {
        User user;
        Market market;
        User userWithCoupons;
        Coupon coupon; 

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

            market = new Market()
            {
                Id = Guid.NewGuid(),
                Address = "Cassinoni 1190",
                Logo = "imageUrlLogo",
                Name = "Disco"
            };

            coupon = new Coupon()
            {
                Id = new Guid(),
                Deadline = new DateTime(),
                Market = market,
                Value = 50
            };

            userWithCoupons = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "colo123",
                Coupons = new List<Coupon>() { coupon }
            }; 
        }

        [TestMethod]
        public void GenerateCouponTestOk()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);
            marketRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Market>() { market});

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.TryGenerate(token.ToString(), 10000);

            userRepositoryMock.VerifyAll();
            userSessionRepositoryMock.VerifyAll();
            marketRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void GenerateCouponTestNotFoundUserOk()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(new DomainBusinessLogicException("")); 
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);
            marketRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Market>() { market });

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.TryGenerate(token.ToString(), 5000);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void GenerateCouponTestNotFoundUserOnUpdateOk()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>())).Throws(new DomainBusinessLogicException(""));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);
            marketRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Market>() { market});

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.TryGenerate(token.ToString(), 5000);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void GenerateCouponTestNotFoundMarketOk()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);
            marketRepositoryMock.Setup(m => m.GetAll()).Throws(new DomainBusinessLogicException(""));

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.TryGenerate(token.ToString(), 5000);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void GenerateCouponTestNotFoundTokenOk()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Throws(new DomainBusinessLogicException(""));
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);
            marketRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Market>() { market });

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.TryGenerate(token.ToString(), 5000);
        }


        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GenerateCouponTestInternalServerError()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(new ServerException());
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);
            marketRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Market>() { market });

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.TryGenerate(token.ToString(), 5000);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GenerateCouponTestInternalServerErrorUpdate()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>())).Throws(new ServerException());
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);
            marketRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Market>() { market });

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.TryGenerate(token.ToString(), 5000);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GenerateCouponTestInternalServerErrorToken()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Throws(new ServerException());
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);
            marketRepositoryMock.Setup(m => m.GetAll()).Returns(new List<Market>() { market });

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.TryGenerate(token.ToString(), 5000);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GenerateCouponTestInternalServerErrorMarket()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            userRepositoryMock.Setup(m => m.Update(It.IsAny<User>()));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);
            marketRepositoryMock.Setup(m => m.GetAll()).Throws(new ServerException());

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.TryGenerate(token.ToString(), 5000);
        }
        
        [TestMethod]
        public void GetAllCouponsTestOk()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithCoupons(It.IsAny<Guid>())).Returns(userWithCoupons);
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            List<Coupon> listOfCouponsObteined = couponManagement.GetAllFromUser(token);
            List<Coupon> listOfCouponsExpected = new List<Coupon>() { coupon };

            CollectionAssert.AreEqual(listOfCouponsExpected, listOfCouponsObteined); 
            userRepositoryMock.VerifyAll();
            userSessionRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void GetAllCouponsTestNotFoundUser()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithCoupons(It.IsAny<Guid>())).Returns(value: null);
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.GetAllFromUser(token);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetAllCouponsTestInternalServerErrorUser()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithCoupons(It.IsAny<Guid>())).Throws(new ServerException());
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Returns(token);
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.GetAllFromUser(token);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetAllCouponsTestInternalServerErrorToken()
        {
            Guid token = new Guid();

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByIdWithCoupons(It.IsAny<Guid>())).Returns(userWithCoupons);
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.GetTokenUserId(It.IsAny<Guid>())).Throws(new ServerException());
            var marketRepositoryMock = new Mock<IRepository<Market>>(MockBehavior.Strict);

            ICouponManagement couponManagement = new CouponManagement(userRepositoryMock.Object
                , marketRepositoryMock.Object, userSessionRepositoryMock.Object);

            couponManagement.GetAllFromUser(token);
        }



    }
}
