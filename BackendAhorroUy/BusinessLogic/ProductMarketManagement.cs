using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using RepositoryException;
using System;
using System.Collections.Generic;
using WebApiModels.DataTypes.MarketDTs;

namespace BusinessLogic
{
    public class ProductMarketManagement : IProductMarketManagemenet
    {
        private readonly IProductMarketRepository productMarketRepository;

        public ProductMarketManagement(IProductMarketRepository aProductMarketRepository)
        {
            productMarketRepository = aProductMarketRepository;
        }
        public Tuple<double, double> GetMinimumAndMaximumPrice(Guid productId)
        {
            try
            {
                return productMarketRepository.GetMinimumAndMaximumPrice(productId);
            }
            catch (ClientException e)
            {
                throw new ClientBusinessLogicException(e.Message);
            }
        }

        public List<Product> GetProductsAvailablesInMarkets()
        {
            try
            {
                return productMarketRepository.GetProductsAvailablesInMarkets();
            }
            catch (ClientException e)
            {
                throw new ClientBusinessLogicException(e.Message);
            }
        }

        public List<Product> GetProductsBySearchText(string searchText, int criteria = 0, Guid categoryId = new Guid())
        {
            try
            {
                List<Product> productsBySearchText = productMarketRepository.SearchOfProductsByText(searchText, criteria, categoryId);
                return productsBySearchText;
            }
            catch (ClientException e)
            {
                throw new ClientBusinessLogicException(e.Message);
            }
        }

        public Product GetProductByBarcode(string barcode)
        {
            try
            {
                Product productByBarcode = productMarketRepository.GetProductByBarcode(barcode);
                return productByBarcode;
            }
            catch (ClientException e)
            {
                throw new ClientBusinessLogicException(e.Message);
            }
        }

        public List<ProductMarket> GetProductsWithDiscounts()
        {
            try
            {
                return productMarketRepository.GetProductsWithDiscounts();
            }
            catch (ClientException e)
            {
                throw new ClientBusinessLogicException(e.Message);
            }
        }

        public List<Tuple<Market, double>> BestOptionToBuy(BestOptionRequest[] products, float longitude, float latitude, int maxDistance)
        {
            try
            {
                return productMarketRepository.BestOptionToBuyProducts(products, longitude, latitude, maxDistance);
            }
            catch (ClientException)
            {
                throw new DomainBusinessLogicException(MessageExceptionBusinessLogic.ErrorObteinedBestOpion);
            }
        }
    }
}
