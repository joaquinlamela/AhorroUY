using System;
namespace WebApiModels.DataTypes.MarketDTs
{
    public class BestOptionResponse
    {
        public string MarketName { get; set; }

        public string MarketAddress { get; set; }

        public double PriceForProducts { get; set; }

        public float MarketLongitude { get; set; }

        public float MarketLatitude { get; set; }

        public DateTime OpenTimeToday { get; set; }

        public DateTime CloseTimeToday { get; set; }

        public double Difference { get; set; }

        public string MarketLogo { get; set; }

        public void CalculateDifferenceToCloseInHours()
        {
            Difference = (CloseTimeToday - DateTime.Now).TotalHours;
        }

        public override bool Equals(object obj)
        {
            return obj is BestOptionResponse response &&
                   MarketName == response.MarketName &&
                   MarketAddress == response.MarketAddress &&
                   PriceForProducts == response.PriceForProducts &&
                   MarketLongitude == response.MarketLongitude &&
                   MarketLatitude == response.MarketLatitude;
        }

        public BestOptionResponse()
        {
        }
    }
}
