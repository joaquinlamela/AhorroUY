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
    public class ProductTest
    {
        Product aProduct;

        [TestInitialize]
        public void SetUp()
        {
            aProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Barcode = "1234567893",
                Category = new Category(),
                Description = "Descripcion de producto",
                ImageUrl = "urlImage",
                Name = "Asado",
                ProductsMarkets = new List<ProductMarket>()
            };
        }

        [TestMethod]

        public void TestGetAllProductsOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IRepository<Product> productRepository = new BaseRepository<Product>(context);

            productRepository.Add(aProduct);

            List<Product> productsList = new List<Product>();
            List<Product> productsFromDb = productRepository.GetAll().ToList();
            productsList.Add(aProduct);
            Assert.IsTrue(productsList.SequenceEqual(productsFromDb));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetAllProductsNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IRepository<Product> productRepository = new BaseRepository<Product>(context);
            productRepository.GetAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestRemoveProductOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IRepository<Product> productRepository = new BaseRepository<Product>(context);
            productRepository.Add(aProduct);
            productRepository.Remove(aProduct);
            productRepository.GetAll();
        }


        [TestMethod]
        public void TestGetProductByIdOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IRepository<Product> productRepository = new BaseRepository<Product>(context);
            productRepository.Add(aProduct);
            Assert.AreEqual(aProduct, productRepository.Get(aProduct.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(ClientException))]
        public void TestGetProductByIdNotFound()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IRepository<Product> productRepository = new BaseRepository<Product>(context);
            productRepository.Get(aProduct.Id);
        }

        [TestMethod]
        public void TestUpdateProductOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IRepository<Product> productRepository = new BaseRepository<Product>(context);
            productRepository.Add(aProduct);
            aProduct.Name = "New name";
            productRepository.Update(aProduct);
            Assert.AreEqual(aProduct.Name, productRepository.Get(aProduct.Id).Name);
        }

    }
}
