using System;

namespace Domain
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public string MarketName { get; set; }
        public string MarketAddress { get; set; }
        public DateTime PurchaseDate { get; set; }

        public Purchase()
        {
            PurchaseDate = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            return obj is Purchase purchase &&
                   Amount == purchase.Amount &&
                   MarketName == purchase.MarketName &&
                   MarketAddress == purchase.MarketAddress;
        }
    }
}
