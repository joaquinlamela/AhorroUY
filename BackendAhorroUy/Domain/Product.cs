using System;
using System.Collections.Generic;

namespace Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<ProductMarket> ProductsMarkets { get; set; }
        public virtual List<User> FavoriteOf { get; set; }

        public Product()
        {
            this.FavoriteOf = new List<User>();
        }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Barcode.Equals(product.Barcode);
        }

    }
}
