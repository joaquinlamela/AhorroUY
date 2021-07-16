using BusinessLogic;
using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Controllers;
using WebApi.DataTypes.ForResponse.CategoryDTs;

namespace WebApiTest
{
    [TestClass]
    public class CategoryControllerTest
    {

        CategoryModel bakeryCategoryModel;
        CategoryModel butcherCategoryModel;
        Category bakeryCategory;
        Category butcherCategory; 

        [TestInitialize]
        public void SetUp()
        {
            bakeryCategoryModel = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Panadería"
            };

            butcherCategoryModel = new CategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Carnicería"
            };

            bakeryCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Panadería"
            };

            butcherCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Carnicería"
            };


        }

        [TestMethod]
        public void GetAllCategoriesOkTest()
        {
            List<CategoryModel> listOfCategoriesModel = new List<CategoryModel>() { bakeryCategoryModel, butcherCategoryModel };
            List<Category> listOfCategories = new List<Category>() { bakeryCategory, butcherCategory };

            var mock = new Mock<ICategoryManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllCategories()).Returns(listOfCategories);
            CategoryController categoryController = new CategoryController(mock.Object);

            var result = categoryController.Get();
            var createdResult = result as OkObjectResult;
            var resultValue = createdResult.Value as List<CategoryModel>;


            mock.VerifyAll();
            Assert.IsTrue(listOfCategoriesModel.SequenceEqual(resultValue));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientBusinessLogicException))]
        public void GetAllCategoriesNotFoundTest()
        {
            List<CategoryModel> listOfCategoriesModel = new List<CategoryModel>() { bakeryCategoryModel, butcherCategoryModel };
            List<Category> listOfCategories = new List<Category>() { bakeryCategory, butcherCategory };

            var mock = new Mock<ICategoryManagement>(MockBehavior.Strict);
            mock.Setup(m => m.GetAllCategories()).Throws(new ClientBusinessLogicException("ERR_CATEGORIES_NOT_FOUND"));
            CategoryController categoryController = new CategoryController(mock.Object);

            categoryController.Get();
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void GetAllCategoriesServerException()
        {
            List<CategoryModel> listOfCategoriesModel = new List<CategoryModel>() { bakeryCategoryModel, butcherCategoryModel };
            List<Category> listOfCategories = new List<Category>() { bakeryCategory, butcherCategory };

            var mock = new Mock<IRepository<Category>>(MockBehavior.Strict);
            mock.Setup(m => m.GetAll()).Throws(new ServerException("ERR_CAN_NOT_CONNECT_DATABASE"));
            ICategoryManagement categoryManagement = new CategoryManagement(mock.Object);
            CategoryController categoryController = new CategoryController(categoryManagement);

            categoryController.Get();
        }

    }
}
