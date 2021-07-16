using Domain;
using System;

namespace DataAccessInterface
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByUsernameAndPassword(string username, string password);
        User GetUserByUsername(string username);
        public User GetUserByIdWithCoupons(Guid token);
        public User GetUserByIdWithFavorites(Guid token);

    }
}
