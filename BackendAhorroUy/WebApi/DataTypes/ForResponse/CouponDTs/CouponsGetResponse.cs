using Domain;
using System;

namespace WebApi.DataTypes.ForResponse.CouponDTs
{
    public class CouponsGetResponse : ModelBaseForResponse<Coupon, CouponsGetResponse>
    {
        public string Url { get; set; }
        public DateTime Deadline { get; set; }
        public int Value { get; set; }
        public string MarketCouponLogoURL { get; set; }

        public CouponsGetResponse() { }

        protected override CouponsGetResponse SetModel(Coupon entity)
        {

            Url = "127.0.0.1:5000/" + entity.Id;
            Deadline = entity.Deadline;
            Value = entity.Value;
            MarketCouponLogoURL = entity.Market.LogoCoupon;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is CouponsGetResponse response &&
                   Url == response.Url &&
                   Value == response.Value &&
                   MarketCouponLogoURL == response.MarketCouponLogoURL;
        }
    }
}
