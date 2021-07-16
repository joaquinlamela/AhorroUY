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
    public class CategoryTest
    {

        [TestMethod]

        public void TestGetAllCategoriesOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IRepository<Category> categoryRepository = new BaseRepository<Category>(context);

            Category category = new Category()
            {
                Id = Guid.NewGuid(),
                ImageUrl = "https://image.freepik.com/vector-gratis/stock-icono-fruta-pera-manzana_24640-20205.jpg",
                Name = "Frutas y verduras"
            };
            categoryRepository.Add(category);

            List<Category> categoriesList = new List<Category>();
            List<Category> categoriesFromDb = categoryRepository.GetAll().ToList();
            categoriesList.Add(category);
            Assert.IsTrue(categoriesList.SequenceEqual(categoriesFromDb));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetAllCategoriesNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IRepository<Category> categoryRepository = new BaseRepository<Category>(context);
            categoryRepository.GetAll(); 
        }
    }
}
