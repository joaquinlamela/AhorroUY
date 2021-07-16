using DataAccess;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryException;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessTest
{
    [TestClass]
    public class UserTest
    {
        User userToAdd;
        User userToAdd2;
        User userToGetCouponsAndFavorites; 
        

        [TestInitialize]
        public void SetUp()
        {
            userToAdd = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "martin1234"
            };

            userToAdd2 = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Joaquin",
                Username = "joaco19",
                Password = "joaquinl19"
            };

            userToGetCouponsAndFavorites = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Username = "colo20",
                Password = "martin1234",
                Coupons = new List<Coupon>(), 
                Favorites = new List<Product>()
            }; 
        }
        [TestMethod]
        public void TestAddUserOK()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserRepository userRepo = new UserRepository(context);
            userRepo.Add(userToAdd);
            List<User> listOfUsers = userRepo.GetAll().ToList();
            Assert.AreEqual(userToAdd, listOfUsers[0]);
        }

        [TestMethod]
        public void TestGetUserOK()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserRepository userRepo = new UserRepository(context);
            userRepo.Add(userToAdd);
            User userOfDb = userRepo.Get(userToAdd.Id);
            Assert.AreEqual(userToAdd, userOfDb);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetUserBad()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserRepository userRepo = new UserRepository(context);
            userRepo.Get(userToAdd.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestRemoveUserOK()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserRepository userRepo = new UserRepository(context);
            userRepo.Add(userToAdd);
            userRepo.Remove(userToAdd);
            userRepo.GetAll();
        }

        [TestMethod]
        public void TestGetAllUsersOK()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserRepository userRepo = new UserRepository(context);
            userRepo.Add(userToAdd);
            userRepo.Add(userToAdd2);
            List<User> listTest = new List<User>();
            listTest.Add(userToAdd);
            listTest.Add(userToAdd2);
            List<User> listOfUsers = userRepo.GetAll().ToList();
            CollectionAssert.AreEqual(listTest, listOfUsers);
        }

        [TestMethod]
        public void TestGetUserByUsernameAndPasswordOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserRepository userRepo = new UserRepository(context);
            userRepo.Add(userToAdd);
            User userObtained = userRepo.GetUserByUsernameAndPassword("colo20", "martin1234");
            Assert.AreEqual(userToAdd, userObtained);
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetUserByUsernameAndPasswordNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserRepository userRepo = new UserRepository(context);
            userRepo.GetUserByUsernameAndPassword("colo20", "martin1234");
        }

        [TestMethod]
        public void TestGetUserByUsernameOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserRepository userRepo = new UserRepository(context);
            userRepo.Add(userToAdd);
            User userObtained = userRepo.GetUserByUsername("colo20");
            Assert.AreEqual(userToAdd, userObtained);
        }

        [TestMethod]
        public void TestGetUserByIdWithCouponsOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserRepository userRepo = new UserRepository(context);
            userRepo.Add(userToGetCouponsAndFavorites);
            User userObtained = userRepo.GetUserByIdWithCoupons(userToGetCouponsAndFavorites.Id);
            Assert.AreEqual(userToGetCouponsAndFavorites, userObtained);
        }

        [TestMethod]
        public void TestGetUserByIdWithFavoritesOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserRepository userRepo = new UserRepository(context);
            userRepo.Add(userToGetCouponsAndFavorites);
            User userObtained = userRepo.GetUserByIdWithFavorites(userToGetCouponsAndFavorites.Id);
            Assert.AreEqual(userToGetCouponsAndFavorites, userObtained);
        }
    }
}
