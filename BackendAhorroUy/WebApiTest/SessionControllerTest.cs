using BusinessLogic.Interface;
using BusinessLogicException;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;
using WebApi.Controllers;
using WebApi.DataTypes.ForRequest.SessionDTs;
using WebApi.DataTypes.ForResponse.SessionDTs;

namespace WebApiTest
{
    [TestClass]
    public class SessionControllerTest
    {
        SessionPostRequest sessionPostRequest;
        Guid tokenSession; 



        [TestInitialize]
        public void SetUp()
        {
            sessionPostRequest = new SessionPostRequest()
            {
                Username = "colo20",
                Password = "martin1234"
            };

            tokenSession = new Guid("90911b44-1ae9-452e-b161-1e556d4696b7");
        }

        [TestMethod]
        public void LogInOkTest()
        {
            var userMock = new Mock<IUserSessionManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Login(It.IsAny<User>())).Returns(tokenSession);
            SessionsController userController = new SessionsController(userMock.Object);
            var result = userController.Login(sessionPostRequest);
            var createdResult = result as CreatedResult;
            var tokenSessionObteined = createdResult.Value as SessionPostResponse;
            userMock.VerifyAll();
            Assert.AreEqual(tokenSession, tokenSessionObteined.Token);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void LogInUserAndPasswordIncorrectTest()
        {
            var userMock = new Mock<IUserSessionManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Login(It.IsAny<User>())).Throws(new ClientBusinessLogicException());
            SessionsController userController = new SessionsController(userMock.Object);
            var result = userController.Login(sessionPostRequest);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void LogInFailInternalServerErrorTest()
        {
            var userMock = new Mock<IUserSessionManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Login(It.IsAny<User>())).Throws(new ServerException());
            SessionsController userController = new SessionsController(userMock.Object);
            var result = userController.Login(sessionPostRequest);
        }

        [TestMethod]
        public void LogOutOk()
        {
            var userMock = new Mock<IUserSessionManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Logout(It.IsAny<Guid>()));
            SessionsController userController = new SessionsController(userMock.Object);
            var result = userController.Logout(tokenSession.ToString());
            var createdResult = result as OkObjectResult;
            userMock.VerifyAll();
            Assert.AreEqual(200, createdResult.StatusCode);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void LogOutInvalidTokenTest()
        {
            var userMock = new Mock<IUserSessionManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Logout(It.IsAny<Guid>())).Throws(new ClientBusinessLogicException());
            SessionsController userController = new SessionsController(userMock.Object);
            var result = userController.Logout(tokenSession.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]

        public void LogOutInternalErrorTest()
        {
            var userMock = new Mock<IUserSessionManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Logout(It.IsAny<Guid>())).Throws(new ServerException());
            SessionsController userController = new SessionsController(userMock.Object);
            var result = userController.Logout(tokenSession.ToString());
        }

    }
}
