using Castle.Core.Internal;
using DataAccessInterface;
using Domain;
using Geolocation;
using Microsoft.EntityFrameworkCore;
using RepositoryException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiModels.DataTypes.MarketDTs;

namespace DataAccess
{
    public class ProductMarketRepository : BaseRepository<ProductMarket>, IProductMarketRepository
    {
        private readonly DbContext context;

        public ProductMarketRepository(DbContext context) : base(context)
        {
            this.context = context;
        }

        public Tuple<double, double> GetMinimumAndMaximumPrice(Guid productId)
        {
            if (Context.Database.CanConnect())
            {
                List<ProductMarket> productsOrdered = context.Set<ProductMarket>().Where(prod => prod.ProductId.Equals(productId)).OrderBy(prod => prod.CurrentPrice).ToList();
                Tuple<double, double> minAndMaxPrice;
                if (productsOrdered.IsNullOrEmpty())
                {
                    throw new ClientException(RepositoryMessagesException.ErrorNotFoundMinAndMaxPrice);
                }
                else
                {
                    minAndMaxPrice = new Tuple<double, double>(productsOrdered.First().CurrentPrice, productsOrdered.Last().CurrentPrice);
                }
                return minAndMaxPrice;
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }

        public List<Product> GetProductsAvailablesInMarkets()
        {
            if (Context.Database.CanConnect())
            {
                List<ProductMarket> productsAvailableInMarkets = context.Set<ProductMarket>().ToList();
                List<Product> distinctProducts = productsAvailableInMarkets
                   .GroupBy(p => p.ProductId)
                   .Select(p => p.First())
                   .ToList().ConvertAll(p => p.Product);
                if (distinctProducts.IsNullOrEmpty())
                {
                    throw new ClientException(RepositoryMessagesException.ErrorNotFoundProducts);
                }
                return distinctProducts;
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }

        public List<ProductMarket> GetProductsWithDiscounts()
        {
            if (Context.Database.CanConnect())
            {
                List<ProductMarket> productsOfCategorySelected = context.Set<ProductMarket>().Where(prod => prod.CurrentPrice < prod.RegularPrice).ToList();

                List<ProductMarket> distinctProducts = productsOfCategorySelected
                 .OrderBy(p => p.CurrentPrice)
                .GroupBy(p => p.ProductId)
                .Select(p => p.First())
                .ToList();

                if (distinctProducts.IsNullOrEmpty())
                {
                    throw new ClientException(RepositoryMessagesException.ErrorNotFoundProductsWithDiscount);
                }
                return distinctProducts;
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }

        public List<Product> SearchOfProductsByText(string searchText, int criteria, Guid categoryId)
        {
            if (Context.Database.CanConnect())
            {
                if (String.IsNullOrEmpty(searchText))
                {
                    searchText = "";
                }
                searchText = searchText.ToLower();

                List<ProductMarket> productsByText = context.Set<ProductMarket>().Where(pm => categoryId == Guid.Empty ?
               pm.Product.Name.ToLower().Contains(searchText) : pm.Product.Name.ToLower().Contains(searchText) && pm.Product.Category.Id.Equals(categoryId)).ToList();
                if (productsByText.IsNullOrEmpty())
                {
                    throw new ClientException(RepositoryMessagesException.ErrorNotFoundProductsByText);
                }
                List<ProductMarket> productsOrdered = OrderSearchByCriteria(productsByText, criteria);
                return productsOrdered.GroupBy(pm => pm.Product.Name).Select(pm => pm.First()).ToList().ConvertAll(pm => pm.Product);
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }

        private List<ProductMarket> OrderSearchByCriteria(List<ProductMarket> products, int criteria)
        {
            List<ProductMarket> productsOrdered = new List<ProductMarket>();

            if (criteria == 0)
            {
                productsOrdered = products.OrderByDescending(p => p.CurrentPrice).ToList();
            }
            else if (criteria == 1)
            {
                productsOrdered = products.OrderBy(p => p.CurrentPrice).ToList();
            }
            else if (criteria == 2)
            {
                productsOrdered = products.OrderBy(p => p.Product.Name).ToList();
            }
            return productsOrdered;
        }

        public Product GetProductByBarcode(string barcode)
        {
            if (Context.Database.CanConnect())
            {
                ProductMarket productSearched = context.Set<ProductMarket>().Where(prod => prod.Product.Barcode.Equals(barcode)).FirstOrDefault();
                Product productObteined = new Product();
                if (productSearched != null)
                {
                    productObteined = productSearched.Product;
                }
                else
                {
                    throw new ClientException(RepositoryMessagesException.ErrorNotFoundProductByBarcode);
                }
                return productObteined;
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }

        public List<Tuple<Market, double>> BestOptionToBuyProducts(BestOptionRequest[] products, float longitude, float latitude, int maxDistance)
        {
            Coordinate userCoordinates = new Coordinate(longitude, latitude);

            List<ProductMarket> auxList = new List<ProductMarket>();

            foreach (BestOptionRequest productAndQuantity in products)
            {
                List<ProductMarket> markets = context.Set<ProductMarket>().AsEnumerable().
                    Where(pm => pm.ProductId.Equals(productAndQuantity.ProductId) &&
                    pm.QuantityAvailable >= productAndQuantity.Quantity && new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.Now.Day, pm.Market.ClosingTime.Hour, pm.Market.ClosingTime.Minute, 0) > DateTime.Now &&
                    pm.Market.DistanceToUser(longitude, latitude) <= maxDistance).ToList();
                auxList.AddRange(markets);
            }

            products = products.OrderBy(prod => prod.ProductId).ToArray();

            IEnumerable<IGrouping<Guid, ProductMarket>> marketsAvailableToSellProducts = auxList.GroupBy(pm => pm.MarketId)
                .Where(pm => pm.Count() == products.Count())
                .Select(pm => pm);

            List<Tuple<Market, double>> result = new List<Tuple<Market, double>>();
            double price = 0;

            foreach (var market in marketsAvailableToSellProducts)
            {
                price = 0;
                Market marketToAdd = new Market();
                var productsInMarket = market.OrderBy(pm => pm.ProductId);
                for (var i = 0; i < productsInMarket.Count(); i++)
                {
                    marketToAdd = productsInMarket.ElementAt(0).Market;
                    price = price + productsInMarket.ElementAt(i).CurrentPrice * products.ElementAt(i).Quantity;
                }
                Tuple<Market, double> marketTotalPriceTuple = new Tuple<Market, double>(marketToAdd, price);
                result.Add(marketTotalPriceTuple);
            }

            if (result.IsNullOrEmpty())
            {
                throw new ClientException();
            }

            return result.OrderBy(res => res.Item2).ToList();
        }
    }
}
