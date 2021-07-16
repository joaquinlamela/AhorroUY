using DataAccessInterface;
using Domain;
using Microsoft.EntityFrameworkCore;
using RepositoryException;
using System;
using System.Linq;

namespace DataAccess
{
    public class UserSessionRepository : BaseRepository<UserSession>, IUserSessionRepository
    {
        private readonly DbContext context;

        public UserSessionRepository(DbContext context) : base(context)
        {
            this.context = context;
        }

        public Guid GetTokenUserId(Guid token)
        {
            if (Context.Database.CanConnect())
            {
                UserSession sessionObteined = context.Set<UserSession>().Where(us => us.Token == token).FirstOrDefault();
                if (sessionObteined == null) return new Guid();
                return sessionObteined.UserId;
            }
            else
            {
                throw new ServerException(RepositoryMessagesException.ErrorConnectingIntoDatabase);
            }
        }
    }
}
