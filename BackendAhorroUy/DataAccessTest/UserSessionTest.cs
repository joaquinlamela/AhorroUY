using DataAccess;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DataAccessTest
{
    [TestClass]

    public class UserSessionTest
    {

        UserSession userSession;
        User user;

        [TestInitialize]
        public void SetUp()
        {
            user = new User()
            {
                Id = Guid.NewGuid()
            };
            userSession = new UserSession()
            {
                Token = Guid.NewGuid(),
                UserId = user.Id
            };
        }

        [TestMethod]
        public void TestGetSessionOk()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserSessionRepository userSessionRepository = new UserSessionRepository(context);
            userSessionRepository.Add(userSession);
            Guid userId = userSessionRepository.GetTokenUserId(userSession.Token);
            Assert.AreEqual(user.Id, userId);
        }

        [TestMethod]
        public void TestGetNullSession()
        {
            ContextObl context = ContextFactory.GetMemoryContext(Guid.NewGuid().ToString());
            IUserSessionRepository userSessionRepository = new UserSessionRepository(context);
            userSessionRepository.Add(userSession);
            userSessionRepository.GetTokenUserId(Guid.NewGuid());
        }
    }
}
