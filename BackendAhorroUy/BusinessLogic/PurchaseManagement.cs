using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using System;


namespace BusinessLogic
{
    public class PurchaseManagement : IPurchaseManagement
    {
        private IUserSessionRepository userSessionRepository;
        private IUserRepository userRepository;
        private IRepository<Purchase> purchaseRepository;
        private ICouponManagement couponsLogic;


        public PurchaseManagement(IRepository<Purchase> aPurchaseRepository, IUserSessionRepository aUserSessionRepository, IUserRepository aUserRepository, ICouponManagement couponManagement)
        {
            purchaseRepository = aPurchaseRepository; 
            userSessionRepository = aUserSessionRepository;
            userRepository = aUserRepository;
            couponsLogic = couponManagement; 
        }

        public Purchase SavePurchase(Guid authToken, Purchase purchaseToSave)
        {
            couponsLogic.TryGenerate(authToken.ToString(), purchaseToSave.Amount); 
            var userId = userSessionRepository.GetTokenUserId(authToken);
            var user = userRepository.Get(userId);
            ValidateExists(user);
            Purchase purchase = new Purchase()
            {
                Id = Guid.NewGuid(),
                Amount = purchaseToSave.Amount,
                MarketName = purchaseToSave.MarketName,
                MarketAddress = purchaseToSave.MarketAddress
            };
            user.Purchases.Add(purchase);
            purchaseRepository.Add(purchase);
            userRepository.Update(user);
            return purchase; 
        }

        private static void ValidateExists(User user)
        {
            if (user == null)
            {
                throw new DomainBusinessLogicException(MessageExceptionBusinessLogic.ErrorSavingPurchaseUser);
            }
        }
    }
}
