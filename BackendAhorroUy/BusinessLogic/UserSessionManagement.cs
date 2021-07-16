using BusinessLogic.Interface;
using BusinessLogicException;
using DataAccessInterface;
using Domain;
using RepositoryException;
using System;

namespace BusinessLogic
{
    public class UserSessionManagement : IUserSessionManagement
    {
        private IUserSessionRepository sessionRepository;
        private IUserRepository userRepository;

        public UserSessionManagement(IUserSessionRepository sessionRepository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.sessionRepository = sessionRepository;
        }

        public Guid Login(User user)
        {
            try
            {
                user.ValidateLogin();
                User userObteined = userRepository.GetUserByUsernameAndPassword(user.Username, user.Password);
                UserSession userSession = new UserSession()
                {
                    Token = Guid.NewGuid(),
                    UserId = userObteined.Id
                };
                sessionRepository.Add(userSession);
                return userSession.Token;
            }
            catch (ClientException e)
            {
                throw new ClientBusinessLogicException(e.Message);
            }
        }


        public void Logout(Guid token)
        {
            try
            {
                UserSession session = sessionRepository.Get(token);
                sessionRepository.Remove(session);
            }
            catch (ClientException)
            {
                throw new ClientBusinessLogicException(MessageExceptionBusinessLogic.ErrorLogoutToken);
            }
        }

        public UserSession IsLogged(Guid token)
        {
            return sessionRepository.Get(token);
        }


    }
}
