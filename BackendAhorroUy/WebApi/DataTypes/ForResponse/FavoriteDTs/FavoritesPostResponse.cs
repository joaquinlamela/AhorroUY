using Domain;
using System;

namespace WebApi.DataTypes.ForResponse.FavoriteDTs
{
    public class FavoritesPostResponse : ModelBaseForResponse<Product, FavoritesPostResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public bool IsFavorite { get; set; }

        public FavoritesPostResponse() { }

        protected override FavoritesPostResponse SetModel(Product entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            ImageUrl = entity.ImageUrl;
            IsFavorite = true;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is FavoritesPostResponse response &&
                   Name == response.Name &&
                   Description == response.Description &&
                   ImageUrl == response.ImageUrl &&
                   MinPrice == response.MinPrice &&
                   MaxPrice == response.MaxPrice;
        }
    }
}
