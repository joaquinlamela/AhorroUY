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
    public class CategoryTest
    {
        [TestMethod]
        public void GetAllCategoriesTest()
        {

            List<Category> listOfCategories = new List<Category>();
            Category bakeryCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Panadería"
            };
            listOfCategories.Add(bakeryCategory);
            Category butcherShopCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Carnicería"
            };
            listOfCategories.Add(butcherShopCategory);

            var categoryMock = new Mock<IRepository<Category>>(MockBehavior.Strict);
            categoryMock.Setup(m => m.GetAll()).Returns(new List<Category>(listOfCategories));

            CategoryManagement categoryLogic = new CategoryManagement(categoryMock.Object);

            List<Category> result = categoryLogic.GetAllCategories();

            categoryMock.VerifyAll();
            CollectionAssert.AreEqual(listOfCategories, result);
        }


        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetExceptionObteinedAllCategoriesTest()
        {

            List<Category> listOfCategories = new List<Category>();
            Category bakeryCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Panadería"
            };
            listOfCategories.Add(bakeryCategory);
            Category butcherShopCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Carnicería"
            };
            listOfCategories.Add(butcherShopCategory);

            var categoryMock = new Mock<IRepository<Category>>(MockBehavior.Strict);
            categoryMock.Setup(m => m.GetAll()).Throws(new ServerException());

            CategoryManagement categoryLogic = new CategoryManagement(categoryMock.Object);

            categoryLogic.GetAllCategories();
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetClientExceptionObteinedAllCategoriesTest()
        {
            List<Category> listOfCategories = new List<Category>();
            Category bakeryCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Panadería"
            };
            listOfCategories.Add(bakeryCategory);
            Category butcherShopCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Carnicería"
            };
            listOfCategories.Add(butcherShopCategory);

            var categoryMock = new Mock<IRepository<Category>>(MockBehavior.Strict);
            categoryMock.Setup(m => m.GetAll()).Throws(new ClientException());

            CategoryManagement categoryLogic = new CategoryManagement(categoryMock.Object);

            categoryLogic.GetAllCategories();
        }
    }
}
