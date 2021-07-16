using BusinessLogicForPushNotification;
using BusinessLogicForPushNotification.Interface;
using DataAccessInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;
using WebApi.Controllers;

namespace WebApiTest
{
    [TestClass]
    public class TokenControllerTest
    {
        Token token;
        
        [TestInitialize]
        public void SetUp()
        {
            token = new Token()
            {
                TokenValue = Guid.NewGuid().ToString()
            };
        }

        [TestMethod]
        public void PostACategoryTest()
        {
            var mock = new Mock<IPushNotificationManagement>(MockBehavior.Strict);
            mock.Setup(m => m.Add(It.IsAny<Token>())).Returns(token);
            TokenController tokenController = new TokenController(mock.Object);

            var result = tokenController.Post(token.TokenValue);
            var createdResult = result as OkObjectResult;
            var resultValue = createdResult.Value as Token;

            mock.VerifyAll();
            Assert.AreEqual(token, resultValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void TestPostCategoryWithInternalServerError()
        {
            var mock = new Mock<IRepository<Token>>(MockBehavior.Strict);
            mock.Setup(m => m.Add(It.IsAny<Token>())).Throws(new ServerException());
            IPushNotificationManagement pushNotification = new PushNotificationManagement(mock.Object);
            TokenController tokenController = new TokenController(pushNotification);

            tokenController.Post(token.TokenValue);
        }
    }
}
