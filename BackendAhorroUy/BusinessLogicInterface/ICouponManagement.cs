using Domain;
using System;
using System.Collections.Generic;
using WebApiModels.DataTypes.MarketDTs;

namespace BusinessLogic.Interface
{
    public interface ICouponManagement
    {
        void TryGenerate(string token, int priceOfPurchase);
        List<Coupon> GetAllFromUser(Guid token);

    }
}
