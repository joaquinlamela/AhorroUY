using System;
using System.Collections.Generic;

namespace Domain
{
    public class Market
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public DateTime OpeningTime { get; set; }

        public DateTime ClosingTime { get; set; }

        public string Logo { get; set; }

        public string LogoCoupon { get; set; }

        public virtual List<ProductMarket> ProductsMarkets { get; set; }


        public Market() { }

        public override bool Equals(object obj)
        {
            return obj is Market market &&
                   Name.Equals(market.Name) && Longitude == market.Longitude &&
                   Latitude == market.Latitude;
        }

        public double DistanceToUser(float userLongitude, float userLatitude)
        {
            var d1 = userLatitude * (Math.PI / 180.0);
            var num1 = userLongitude * (Math.PI / 180.0);
            var d2 = Latitude * (Math.PI / 180.0);
            var num2 = Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return (6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))) / 1000);
        }
    }
}
