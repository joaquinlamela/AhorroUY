
using System;

namespace WebApiModels.DataTypes.MarketDTs
{
    public class BestOptionRequest
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public BestOptionRequest()
        {
        }
    }
}
