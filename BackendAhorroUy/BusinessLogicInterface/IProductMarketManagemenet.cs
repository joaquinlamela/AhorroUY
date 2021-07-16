using Domain;
using System;
using System.Collections.Generic;
using WebApiModels.DataTypes.MarketDTs;

namespace BusinessLogic.Interface
{
    public interface IProductMarketManagemenet
    {
        public Tuple<double, double> GetMinimumAndMaximumPrice(Guid productId);

        public List<ProductMarket> GetProductsWithDiscounts();

        public List<Product> GetProductsAvailablesInMarkets();

        public List<Product> GetProductsBySearchText(string searchText, int criteria = 0, Guid categoryId = new Guid());

        public Product GetProductByBarcode(string barcode);

        public List<Tuple<Market, double>> BestOptionToBuy(BestOptionRequest[] products, float longiute, float latitude, int maxDistance);
    }
}
