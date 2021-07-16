using Domain;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Interface
{
    public interface IUserManagement
    {
        User Create(User user);
        User Get(Guid userId);
        IEnumerable<User> GetAll();
    }
}
