using Domain;
using System;

namespace WebApi.DataTypes.ForResponse.ProductDTs
{
    public class DiscountProductModel : ModelBaseForResponse<ProductMarket, DiscountProductModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double CurrentPrice { get; set; }
        public double RegularPrice { get; set; }
        public double DiscountPercentage { get; set; }
        public string MarketName { get; set; }

        protected override DiscountProductModel SetModel(ProductMarket product)
        {
            Id = product.Product.Id;
            Name = product.Product.Name;
            Description = product.Product.Description;
            ImageUrl = product.Product.ImageUrl;
            CurrentPrice = product.CurrentPrice;
            RegularPrice = product.RegularPrice;
            double percentage = ((RegularPrice - CurrentPrice) * 100) / RegularPrice;
            DiscountPercentage = Math.Floor(percentage);
            MarketName = product.Market.Name;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is DiscountProductModel model &&
                   Name.Equals(model.Name) &&
                   Description.Equals(model.Description) &&
                   DiscountPercentage == model.DiscountPercentage &&
                   MarketName.Equals(model.MarketName)
                   && ImageUrl.Equals(model.ImageUrl)
                   && MinPrice == model.MinPrice
                   && MaxPrice == model.MaxPrice; 
        }
    }
}
