using BusinessLogicForPushNotification.Interface;
using DataAccessInterface;
using Domain;
using EntitiesForPushNotification;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicForPushNotification
{
    public class PushNotificationManagement : IPushNotificationManagement
    {
        private IRepository<Token> tokenRepository;

        public PushNotificationManagement(IRepository<Token> aTokenRepository)
        {
            tokenRepository = aTokenRepository;
        }

        private Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");
        private string ServerKey = "AAAAJ6RCfZ0:APA91bHPRcfKv90OPoD8Tos9o9eB6DmUzF1iufkVV6Fgm9rITq_NgTSosOydj48CuS3OycxxChkYGJniHLQxhPvFIBFmgVGJAp0YgGUqrs9-uN8GVvCIos9tjRxfuPIi2Rkp-zPl73iJ";

        public bool SendPushNotification(string[] deviceTokens, string body)
        {
                bool sent = false;
                
                var messageInformation = new Message()
                {
                    notification = new Notification()
                    {
                        title = "¿Viste las ofertas bomba que tenemos?",
                        body = body
                    },
                    registration_ids = deviceTokens,
                    priority = "high"
                };

                string jsonMessage = JsonConvert.SerializeObject(messageInformation);

                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);

                request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                HttpResponseMessage result;
                using (var client = new HttpClient())
                {
                    result = client.Send(request);
                    sent = sent || result.IsSuccessStatusCode;
                }

                return sent;
        }

        public Token Add(Token tokenValue)
        {
                tokenRepository.Add(tokenValue);
                return tokenValue;
        }
    }
}
