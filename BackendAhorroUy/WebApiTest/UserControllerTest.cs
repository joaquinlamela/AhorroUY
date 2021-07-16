using BusinessLogic.Interface;
using BusinessLogicException;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;
using System.Collections.Generic;
using WebApi.Controllers;
using WebApi.DataTypes.ForRequest.UserDTs;
using WebApi.DataTypes.ForResponse.UserDTs;

namespace WebApiTest
{   [TestClass]
    public class UserControllerTest
    {
        User aUser;

        UserPostRequest userPostRequest;

        UserPostResponse userPostResponse; 

        [TestInitialize]
        public void SetUp()
        {

            aUser = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "martin1234"
            };

            userPostRequest = new UserPostRequest()
            {
                Name = "Martin",
                Username = "colo20",
                Password = "martin1234"
            };

            userPostResponse = new UserPostResponse()
            {
                Name = "Martin",
                Username = "colo20",
                Id = aUser.Id.ToString()
            }; 
        } 
        
        [TestMethod]
        public void PostUserOk()
        {
            var userMock = new Mock<IUserManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Create(aUser)).Returns(aUser);
            UsersController userController = new UsersController(userMock.Object);
            var result = userController.Post(userPostRequest);
            var createdResult = result as CreatedResult;
            var userResult = createdResult.Value as UserPostResponse;
            userMock.VerifyAll();
            Assert.AreEqual(userPostResponse, userResult);
        }

        [TestMethod]
        public void PostUserNullOk()
        {
            var userMock = new Mock<IUserManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Create(aUser)).Returns(value: null);
            UsersController userController = new UsersController(userMock.Object);
            var result = userController.Post(userPostRequest);
            var createdResult = result as CreatedResult;
            var userResult = createdResult.Value as UserPostResponse;
            userMock.VerifyAll();
            Assert.AreEqual(null, userResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void PostUserInvalidInternalServerError()
        {
            var userMock = new Mock<IUserManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Create(It.IsAny<User>())).Throws(new ServerException());
            UsersController userController = new UsersController(userMock.Object);
            var result = userController.Post(userPostRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainBusinessLogicException))]
        public void PostUserInvalidInformation()
        {
            var userMock = new Mock<IUserManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Create(It.IsAny<User>())).Throws(new DomainBusinessLogicException(""));
            UsersController userController = new UsersController(userMock.Object);
            var result = userController.Post(userPostRequest);
        }


        [TestMethod]
        public void GetUserByIdOk()
        {
            var userMock = new Mock<IUserManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(aUser);
            UsersController userController = new UsersController(userMock.Object);
            var result = userController.Get(aUser.Id);
            var createdResult = result as OkObjectResult;
            var userModelResult = createdResult.Value as UserPostResponse;
            userMock.VerifyAll();
            Assert.AreEqual(userPostResponse, userModelResult);
        }


        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetUserByIdInternalError()
        {
            var userMock = new Mock<IUserManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(new ServerException());
            UsersController userController = new UsersController(userMock.Object);
            var result = userController.Get(aUser.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetUserByIdNotFound()
        {
            var userMock = new Mock<IUserManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.Get(It.IsAny<Guid>())).Throws(new ClientBusinessLogicException());
            UsersController userController = new UsersController(userMock.Object);
            var result = userController.Get(aUser.Id);
        }

        [TestMethod]
        public void GetAllUsersOk()
        {
            List<User> userList = new List<User> { aUser };
            List<UserPostResponse> userModelList = new List<UserPostResponse> { userPostResponse };
            var userMock = new Mock<IUserManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.GetAll()).Returns(userList);
            UsersController userController = new UsersController(userMock.Object);
            var result = userController.GetAll();
            var createdResult = result as OkObjectResult;
            var userModelResult = createdResult.Value as List<UserPostResponse>;
            userMock.VerifyAll();
            CollectionAssert.AreEqual(userModelList, userModelResult);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetAllUsersNotFound()
        {
            var userMock = new Mock<IUserManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.GetAll()).Throws(new ClientBusinessLogicException());
            UsersController userController = new UsersController(userMock.Object);
            var result = userController.GetAll();
        }
        
        
        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetAllUsersInternalError()
        {
            var userMock = new Mock<IUserManagement>(MockBehavior.Strict);
            userMock.Setup(m => m.GetAll()).Throws(new ServerException());
            UsersController userController = new UsersController(userMock.Object);
            var result = userController.GetAll();
        }

    }
}
