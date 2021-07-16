using BusinessLogicException;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Test
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void CreateValidUserTestOk()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Nombre1",
                Username = "colo20",
                Password = "martin1234"
            };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByUsername(user.Username)).Returns(value: null);
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            User result = userLogic.Create(user);
            userRepositoryMock.VerifyAll();

            User userToCompare = new User()
            {
                Id = result.Id,
                Name = result.Name,
                Username = result.Username,
                Password = result.Password
            };

            Assert.IsTrue(result.Equals(userToCompare));
        }


        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void CreateInvalidUserRepositoryExceptionTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Nombre1",
                Username = "colo20",
                Password = "martin1234"
            };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByUsername(user.Username)).Returns(value: null);
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>())).Throws(new ServerException());
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            User result = userLogic.Create(user);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void CreateInvalidUserEmptyNameTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "",
                Username = "colo20",
                Password = "martin1234"
            };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            User result = userLogic.Create(user);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void CreateInvalidUserEmptyUserNameTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "",
                Password = "martin1234"
            };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            User result = userLogic.Create(user);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void CreateInvalidUserEmptyPasswordTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = ""
            };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            User result = userLogic.Create(user);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void CreateInvalidUserAlredyExistTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "pass"
            };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByUsername(user.Username)).Returns(user);
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            User result = userLogic.Create(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void CreateInvalidUserInternalErrorWhenCheckIfUserExistTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "pass"
            };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetUserByUsername(user.Username)).Throws(new ServerException());
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            User result = userLogic.Create(user);
        }

        [TestMethod]
        public void GetUserByIdOkTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "soyelcolobienfachero"
            };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(user);
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            User userResult = userLogic.Get(user.Id);
            userRepositoryMock.VerifyAll();
            Assert.AreEqual(user, userResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetUserByIdInternalErrorTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "soyelcolobienfachero"
            };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(new ServerException());
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            User userResult = userLogic.Get(user.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetUserByIdClientErrorTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "soyelcolobienfachero"
            };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(new ClientException());
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            User userResult = userLogic.Get(user.Id);
        }

        [TestMethod]
        public void GetAllUsersOkTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "soyelcolobienfachero"
            };
            User user2 = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Joaquin",
                Username = "joac123",
                Password = "joaco12345"
            };
            List<User> expectedResult = new List<User>() { user, user2 };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.GetAll()).Returns(expectedResult);
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            List<User> obteinedResult = userLogic.GetAll().ToList();
            userRepositoryMock.VerifyAll();
            CollectionAssert.AreEqual(expectedResult, obteinedResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]

        public void GetAllUsersInvalidTest()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "soyelcolobienfachero"
            };

            List<User> expectedResult = new List<User>() { user };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            userRepositoryMock.Setup(m => m.GetAll()).Throws(new ServerException());
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            userLogic.GetAll().ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]

        public void GetAllUsersInvalidTestExceptionClient()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "soyelcolobienfachero"
            };

            List<User> expectedResult = new List<User>() { user };
            var userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepositoryMock.Setup(m => m.Add(It.IsAny<User>()));
            userRepositoryMock.Setup(m => m.GetAll()).Throws(new ClientException());
            UserManagement userLogic = new UserManagement(userRepositoryMock.Object);
            userLogic.GetAll().ToList();
        }
       
    }
}

