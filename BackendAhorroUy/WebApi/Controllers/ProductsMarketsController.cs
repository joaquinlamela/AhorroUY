using BusinessLogic.Interface;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApi.DataTypes.ForResponse.ProductDTs;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ExceptionFilter]
    [Route("api/products")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProductsMarketsController : ControllerBase
    {
        private readonly IProductMarketManagemenet productMarketManagement;

        public ProductsMarketsController(IProductMarketManagemenet productMarketLogic)
        {
            productMarketManagement = productMarketLogic;
        }

        public IActionResult Get()
        {
            List<Product> allProducts = productMarketManagement.GetProductsAvailablesInMarkets();
            IEnumerable<ProductModel> productModelList = ProductModel.ToModel(allProducts);
            foreach (ProductModel aProductModel in productModelList)
            {
                Tuple<double, double> minAndMaxPrice = productMarketManagement.GetMinimumAndMaximumPrice(aProductModel.Id);
                aProductModel.MinPrice = minAndMaxPrice.Item1;
                aProductModel.MaxPrice = minAndMaxPrice.Item2;
            }
            return Ok(productModelList);
        }

        [HttpGet("discountProducts")]
        public IActionResult GetProductsWithDiscount()
        {
            List<ProductMarket> discountProducts = productMarketManagement.GetProductsWithDiscounts();
            IEnumerable<DiscountProductModel> productModelList = DiscountProductModel.ToModel(discountProducts);
            foreach (DiscountProductModel aProductModel in productModelList)
            {
                Tuple<double, double> minAndMaxPrice = productMarketManagement.GetMinimumAndMaximumPrice(aProductModel.Id);
                aProductModel.MinPrice = minAndMaxPrice.Item1;
                aProductModel.MaxPrice = minAndMaxPrice.Item2;
            }
            return Ok(productModelList);
        }

        [HttpGet("productsByText")]
        public IActionResult GetProductsBySearchText([FromQuery] string searchText = "", [FromQuery] int criteria = 0, [FromQuery] string categoryId = "")
        {
            Guid idCategory = new Guid(categoryId);
            List<Product> productsWithSearchText = productMarketManagement.GetProductsBySearchText(searchText, criteria, idCategory);
            IEnumerable<ProductModel> productsWithSearchTextModelList = ProductModel.ToModel(productsWithSearchText);
            foreach (ProductModel aProductModel in productsWithSearchTextModelList)
            {
                Tuple<double, double> minAndMaxPrice = productMarketManagement.GetMinimumAndMaximumPrice(aProductModel.Id);
                aProductModel.MinPrice = minAndMaxPrice.Item1;
                aProductModel.MaxPrice = minAndMaxPrice.Item2;
            }
            return Ok(productsWithSearchTextModelList);
        }

        [HttpGet("productsByBarcode")]
        public IActionResult GetProductByBarcode([FromQuery] string barcode)
        {
            Product productByBarcode = productMarketManagement.GetProductByBarcode(barcode);
            ProductModel productWithModel = ProductModel.ToModel(productByBarcode);
            Tuple<double, double> minAndMaxPrice = productMarketManagement.GetMinimumAndMaximumPrice(productWithModel.Id);
            productWithModel.MinPrice = minAndMaxPrice.Item1;
            productWithModel.MaxPrice = minAndMaxPrice.Item2;
            return Ok(productWithModel);
        }
    }
}
