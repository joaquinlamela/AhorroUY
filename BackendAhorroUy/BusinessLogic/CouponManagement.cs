using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using System;
using System.Collections.Generic;
using WebApiModels.DataTypes.MarketDTs;

namespace BusinessLogic
{
    public class CouponManagement : ICouponManagement
    {
        private IUserRepository userRepository;
        private IRepository<Market> marketRepository;
        private IUserSessionRepository userSessionRepository;

        public CouponManagement(IUserRepository userRepository, IRepository<Market> marketRepository, IUserSessionRepository userSessionRepository)
        {
            this.userRepository = userRepository;
            this.marketRepository = marketRepository;
            this.userSessionRepository = userSessionRepository;
        }

        public List<Coupon> GetAllFromUser(Guid token)
        {
            var userId = userSessionRepository.GetTokenUserId(token);
            var user = userRepository.GetUserByIdWithCoupons(userId);
            if (user == null)
            {
                throw new DomainBusinessLogicException(MessageExceptionBusinessLogic.ErrorObteinedAllCouponsOfUser);
            }
            return user.Coupons;
        }

        public void TryGenerate(string token, int priceOfPurchase)
        {
            if (priceOfPurchase > 1000)
            {
                List<Market> markets = (List<Market>)marketRepository.GetAll();
                var random = new Random();
                int index = random.Next(markets.Count);
                Generate(new Guid(token), markets[index], DateTime.Now.AddDays(30), priceOfPurchase / 20);
            }
        }

        private void Generate(Guid token, Market market, DateTime deadline, int value)
        {
            var userId = userSessionRepository.GetTokenUserId(token);
            var user = userRepository.Get(userId);
            Coupon newCoupon = new Coupon() { Market = market, Deadline = deadline, Value = value };
            user.Coupons.Add(newCoupon);
            userRepository.Update(user);
        }

    }
}
