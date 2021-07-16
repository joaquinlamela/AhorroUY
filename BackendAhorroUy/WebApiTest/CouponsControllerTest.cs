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
using WebApi.DataTypes.ForResponse.CouponDTs;

namespace WebApiTest
{
    [TestClass]
    public class CouponsControllerTest
    {
        Coupon coupon;
        Market aMarket;
        CouponsGetResponse couponResponse; 


        [TestInitialize]
        public void SetUp()
        {
            aMarket = new Market()
            {
                Id = Guid.NewGuid(),
                Address = "18 de Julio 1234",
                Logo = "image.png",
                LogoCoupon = "logoCoupon.png",
                Name = "Disco centro",
                ProductsMarkets = new List<ProductMarket>()
            };
            coupon = new Coupon()
            {
                Id = Guid.NewGuid(),
                Deadline = DateTime.Now.AddDays(30),
                Value = 50,
                Market = aMarket
            };
            couponResponse = new CouponsGetResponse()
            {
                Value = 50,
                MarketCouponLogoURL = aMarket.LogoCoupon,
                Deadline = coupon.Deadline,
                Url = "127.0.0.1:5000/" + coupon.Id
            };
        }

        [TestMethod]
        public void GetAllCouponsFromUserTestOk()
        {
            List<Coupon> listOfCoupon = new List<Coupon>() { coupon };
            List<CouponsGetResponse> listOfCouponsModel = new List<CouponsGetResponse>() { couponResponse }; 

            var mock = new Mock<ICouponManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllFromUser(It.IsAny<Guid>())).Returns(listOfCoupon);
            CouponsController couponController = new CouponsController(mock.Object);

            var result = couponController.GetAllFromUser(Guid.NewGuid().ToString());
            var createdResult = result as OkObjectResult;
            var resultValue = createdResult.Value as List<CouponsGetResponse>;


            mock.VerifyAll();
            Assert.IsTrue(listOfCouponsModel.SequenceEqual(resultValue));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetAllCouponsFromUserNotFoundTest()
        {
            List<Coupon> listOfCoupon = new List<Coupon>() { coupon };

            var mock = new Mock<ICouponManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllFromUser(It.IsAny<Guid>())).Throws(new ClientBusinessLogicException());
            CouponsController couponController = new CouponsController(mock.Object);
            var result = couponController.GetAllFromUser(Guid.NewGuid().ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetAllCouponsFromUserInternalServerErrorTest()
        {
            List<Coupon> listOfCoupon = new List<Coupon>() { coupon };

            var mock = new Mock<ICouponManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllFromUser(It.IsAny<Guid>())).Throws(new ServerException());
            CouponsController couponController = new CouponsController(mock.Object);
            var result = couponController.GetAllFromUser(Guid.NewGuid().ToString());
        }
    }
}
