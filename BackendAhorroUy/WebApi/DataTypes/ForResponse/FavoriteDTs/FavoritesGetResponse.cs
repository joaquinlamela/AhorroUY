using Domain;
using System;

namespace WebApi.DataTypes.ForResponse.FavoriteDTs
{
    public class FavoritesGetResponse : ModelBaseForResponse<Product, FavoritesGetResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }

        public FavoritesGetResponse() { }

        protected override FavoritesGetResponse SetModel(Product entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            ImageUrl = entity.ImageUrl;
            return this; 
        }

        public override bool Equals(object obj)
        {
            return obj is FavoritesGetResponse response &&
                   Id.Equals(response.Id) &&
                   Name == response.Name &&
                   Description == response.Description &&
                   ImageUrl == response.ImageUrl &&
                   MinPrice == response.MinPrice &&
                   MaxPrice == response.MaxPrice;
        }
    }
}
