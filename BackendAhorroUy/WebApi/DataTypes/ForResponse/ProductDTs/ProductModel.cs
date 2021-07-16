using Domain;
using System;

namespace WebApi.DataTypes.ForResponse.ProductDTs
{
    public class ProductModel : ModelBaseForResponse<Product, ProductModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }

        protected override ProductModel SetModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            ImageUrl = product.ImageUrl;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is ProductModel model &&
                   Name.Equals(model.Name) 
                   && Description.Equals(model.Description) &&
                   MinPrice == model.MinPrice 
                   && ImageUrl.Equals(model.ImageUrl)
                   && MaxPrice == model.MaxPrice; 
        }
    }
}
