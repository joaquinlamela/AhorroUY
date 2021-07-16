using BusinessLogicException;
using DataAccessInterface;
using Domain;
using DomainException;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;


namespace BusinessLogic.Test
{
    [TestClass]
    public class UserSessionTest
    {
        User user;

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
        }

        [TestMethod]
        public void LogInOkNotExistUserSessionTest()
        {
            Guid tokenSession = new Guid(); 
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByUsernameAndPassword(user.Username, user.Password)).Returns(user); 
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.Add(It.IsAny<UserSession>()));
            UserSessionManagement userLogic = new UserSessionManagement(userSessionRepositoryMock.Object, userRepositoryMock.Object);
            Guid userSessionTokenResult = userLogic.Login(user);
            userRepositoryMock.VerifyAll();
            Assert.AreNotEqual(tokenSession, userSessionTokenResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void LogInFailedPassIncorrectTest()
        {
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByUsernameAndPassword("colo20", "colo123")).Throws(new ClientException());
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            UserSessionManagement userLogic = new UserSessionManagement(userSessionRepositoryMock.Object, userRepositoryMock.Object);
            userLogic.Login(user);
        }

        [TestMethod]
        [ExpectedException(typeof(UserException))]
        public void LogInFailedPassIncorrectUserTest()
        {
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByUsernameAndPassword("colo20", "colo123")).Throws(new UserException(""));
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            UserSessionManagement userLogic = new UserSessionManagement(userSessionRepositoryMock.Object, userRepositoryMock.Object);
            userLogic.Login(user);
        }


        [TestMethod]
        public void LogOutOk()
        {
            Guid aToken = Guid.NewGuid();

            UserSession userSession = new UserSession()
            {
                UserId = user.Id,
                Token = aToken
            };

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(userSession);
            userSessionRepositoryMock.Setup(m => m.Remove(It.IsAny<UserSession>()));
            UserSessionManagement userLogic = new UserSessionManagement(userSessionRepositoryMock.Object, userRepositoryMock.Object);
            userLogic.Logout(aToken);
            userRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void LogOutFailedWhenGetSessionByToken()
        {
            Guid aToken = Guid.NewGuid();

            UserSession userSession = new UserSession()
            {
                UserId = user.Id,
                Token = aToken
            };

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.Get(aToken)).Throws(new ClientException());
            userSessionRepositoryMock.Setup(m => m.Remove(It.IsAny<UserSession>()));
            UserSessionManagement userLogic = new UserSessionManagement(userSessionRepositoryMock.Object, userRepositoryMock.Object);
            userLogic.Logout(aToken);
        }


        [TestMethod]
        public void IsLoggedOk()
        {
            Guid aToken = Guid.NewGuid();

            UserSession userSession = new UserSession()
            {
                UserId = user.Id,
                Token = aToken
            };

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            var userSessionRepositoryMock = new Mock<IUserSessionRepository>(MockBehavior.Strict);
            userSessionRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(userSession);
            UserSessionManagement userLogic = new UserSessionManagement(userSessionRepositoryMock.Object, userRepositoryMock.Object);
            UserSession userSessionResult = userLogic.IsLogged(aToken);
            Assert.AreEqual(aToken, userSessionResult.Token); 
            userRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void TestValidateLogin()
        {
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);

            User userObteined = userLogic.Get(user.Id);
            userObteined.ValidateLogin();
            userRepositoryMock.VerifyAll(); 
        }

        [TestMethod]
        [ExpectedException(typeof(UserException))]
        public void TestValidateLoginWithEmptyUsername()
        {

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);

            User userObteined = userLogic.Get(user.Id);
            userObteined.Username = ""; 
            userObteined.ValidateLogin();
        }

        [TestMethod]
        [ExpectedException(typeof(UserException))]
        public void TestValidateLoginWithEmptyPassword()
        {

            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);

            User userObteined = userLogic.Get(user.Id);
            userObteined.Password = "";
            userObteined.ValidateLogin();
        }

    }
}
