using Domain;
using System;
using System.Collections.Generic;
using WebApiModels.DataTypes.MarketDTs;

namespace DataAccessInterface
{
    public interface IProductMarketRepository : IRepository<ProductMarket>
    {
        public Tuple<double, double> GetMinimumAndMaximumPrice(Guid productId);

        public List<ProductMarket> GetProductsWithDiscounts();

        public List<Product> GetProductsAvailablesInMarkets();

        public List<Product> SearchOfProductsByText(string searchText, int criteria, Guid categoryId);

        public Product GetProductByBarcode(string barcode);

        public List<Tuple<Market, double>> BestOptionToBuyProducts(BestOptionRequest[] products, float longiute, float latitude, int maxDistance);
    }
}
