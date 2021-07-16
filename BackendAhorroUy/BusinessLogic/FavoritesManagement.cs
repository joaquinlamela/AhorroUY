using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class FavoritesManagement : IFavoritesManagement
    {
        private IUserSessionRepository userSessionRepository;
        private IUserRepository userRepository;
        private IRepository<Product> productRepository;

        public FavoritesManagement(IUserSessionRepository userSessionRepository, IUserRepository userRepository, IRepository<Product> productRepository)
        {
            this.userSessionRepository = userSessionRepository;
            this.userRepository = userRepository;
            this.productRepository = productRepository;
        }

        public Product AddProduct(Guid token, Guid productId)
        {
            var userId = userSessionRepository.GetTokenUserId(token);
            var user = userRepository.Get(userId);
            ValidateExists(user);
            var product = productRepository.Get(productId);
            user.Favorites.Add(product);
            userRepository.Update(user);
            return product;
        }

        public List<Product> GetAllFromUser(Guid token)
        {
            var userId = userSessionRepository.GetTokenUserId(token);
            var user = userRepository.GetUserByIdWithFavorites(userId);
            ValidateExists(user);
            return user.Favorites;
        }

        public void DeleteProduct(Guid token, Guid productId)
        {
            var userId = userSessionRepository.GetTokenUserId(token);
            var user = userRepository.GetUserByIdWithFavorites(userId);
            ValidateExists(user);
            user.Favorites.Remove(user.Favorites.Find(p => p.Id == productId));
            userRepository.Update(user);
        }

        private static void ValidateExists(User user)
        {
            if (user == null)
            {
                throw new DomainBusinessLogicException(MessageExceptionBusinessLogic.ErrorExistFavoritesOfUser);
            }
        }

    }
}
