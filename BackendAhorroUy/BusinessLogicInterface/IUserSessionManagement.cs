using Domain;
using System;

namespace BusinessLogic.Interface
{
    public interface IUserSessionManagement
    {
        Guid Login(User user);
        void Logout(Guid token);
        UserSession IsLogged(Guid token);
    }
}
