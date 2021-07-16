using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using DomainException;
using RepositoryException;
using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class UserManagement : IUserManagement
    {
        private readonly IUserRepository userRepository;

        public UserManagement(IUserRepository aUserRepository)
        {
            userRepository = aUserRepository;
        }

        public User Create(User newUserData)
        {
            try
            {
                newUserData.Id = Guid.NewGuid();
                newUserData.Validate();
                VerifyIfUserExist(newUserData);
                userRepository.Add(newUserData);
                return newUserData;
            }
            catch (UserException e)
            {
                throw new DomainBusinessLogicException(e.Message);
            }
            catch (DomainBusinessLogicException e)
            {
                throw new DomainBusinessLogicException(e.Message);
            }
        }
        private void VerifyIfUserExist(User user)
        {
                User userObteined = userRepository.GetUserByUsername(user.Username);
                if (userObteined != null)
                {
                    throw new DomainBusinessLogicException(MessageExceptionBusinessLogic.ErrorUserAlredyExist);
                }
        }

        public User Get(Guid userId)
        {
            try
            {
                User userObteined = userRepository.Get(userId);
                return userObteined;
            }
            catch (ClientException e)
            {
                throw new ClientBusinessLogicException(MessageExceptionBusinessLogic.ErrorNotFindUser, e);
            }
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return userRepository.GetAll();
            }
            catch (ClientException e)
            {
                throw new ClientBusinessLogicException(MessageExceptionBusinessLogic.ErrorNotExistUsers, e);
            }
        }
    }
}
