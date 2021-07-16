using Domain;
using System;

namespace DataAccessInterface
{
    public interface IUserSessionRepository : IRepository<UserSession>
    {
        Guid GetTokenUserId(Guid token);

    }
}
