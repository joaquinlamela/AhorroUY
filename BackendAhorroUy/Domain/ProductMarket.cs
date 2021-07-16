using System;

namespace Domain
{
    public class ProductMarket
    {
        public Guid ProductId { get; set; }
        public Guid MarketId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Market Market { get; set; }
        public double RegularPrice { get; set; }
        public double CurrentPrice { get; set; }
        public int QuantityAvailable { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ProductMarket productMarket &&
                   Product.Equals(productMarket.Product) && Market.Equals(productMarket.Market);
        }
    }
}
