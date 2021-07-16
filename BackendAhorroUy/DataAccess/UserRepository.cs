using DataAccessInterface;
using Domain;
using Microsoft.EntityFrameworkCore;
using RepositoryException;
using System;
using System.Linq;

namespace DataAccess
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly DbContext context;

        public UserRepository(DbContext context) : base(context)
        {
            this.context = context;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            if (Context.Database.CanConnect())
            {
                User userObteined = context.Set<User>().Where(user => user.Username.Equals(username) && user.Password.Equals(password)).FirstOrDefault();
                if (userObteined == null)
                {
                    throw new ClientException(RepositoryMessagesException.ErrorUsernameOrPasswordIncorrect);
                }
                return userObteined;
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }

        public User GetUserByUsername(string userName)
        {
            if (Context.Database.CanConnect())
            {
                User userObteined = context.Set<User>().Where(user => user.Username.Equals(userName)).FirstOrDefault();
                return userObteined;
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase); 
            }
        }

        public User GetUserByIdWithCoupons(Guid token)
        {
            if (Context.Database.CanConnect())
            {
                User userObteined = context.Set<User>().Where(u => u.Id == token).Include(u => u.Coupons).FirstOrDefault();
                return userObteined;
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }

        public User GetUserByIdWithFavorites(Guid token)
        {
            if (Context.Database.CanConnect())
            {
                User userObteined = context.Set<User>().Where(u => u.Id == token).Include(u => u.Favorites).FirstOrDefault();
                return userObteined;
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase); 
            }
        }
    }
}
