using BusinessLogicForPushNotification;
using DataAccessInterface;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryException;
using System;
using System.Threading.Tasks;

namespace BusinessLogic.Test
{
    [TestClass]
    public class PushNotificationTest
    {

        [TestMethod]
        public void CreateValidCategoryTestOk()
        {
            Token token1 = new Token
            {
                TokenValue = Guid.NewGuid().ToString()
            };

            var tokenMock = new Mock<IRepository<Token>>(MockBehavior.Strict);
            tokenMock.Setup(m => m.Add(It.IsAny<Token>()));

            PushNotificationManagement pushNotificationManagement = new PushNotificationManagement(tokenMock.Object);

            Token result = pushNotificationManagement.Add(token1);

            tokenMock.VerifyAll();
            Assert.IsTrue(result.Equals(token1)); 
        }

        [TestMethod]
        [ExpectedException(typeof(ServerException))]
        public void CreateInvalidCategoryInternalError()
        {
            Token token1 = new Token
            {
                TokenValue = Guid.NewGuid().ToString()
            };

            var tokenMock = new Mock<IRepository<Token>>(MockBehavior.Strict);
            tokenMock.Setup(m => m.Add(It.IsAny<Token>())).Throws(new ServerException());

            PushNotificationManagement pushNotificationManagement = new PushNotificationManagement(tokenMock.Object);

            Token result = pushNotificationManagement.Add(token1);
        }
        

        [TestMethod]
        public void SendPushNotificationTest()
        {
            string[] tokensOfDevices = { "fS-bmqfETCyQrVTKpJyBSP:APA91bGJD0LfEbMCkzeF9-P04-HtZUvJ7BRj-2efpmTZVUjXCEvj88__ezvwgazZRWWQrrLhrWZonQiQKzkTTuMYi_fNBShHLOSYSIzHgKjHTSsUNBaEPePVkmaXCMtBpjNFuciCtIxA" }; 
            string body = "Soy una Gargola FT Malaso"; 


            var tokenMock = new Mock<IRepository<Token>>(MockBehavior.Strict);

            PushNotificationManagement pushNotificationManagement = new PushNotificationManagement(tokenMock.Object);

            bool result = pushNotificationManagement.SendPushNotification(tokensOfDevices, body);

            tokenMock.VerifyAll();
            Assert.IsTrue(result);
        }
    }
}
