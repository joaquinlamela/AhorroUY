using Domain;
using System.Threading.Tasks;

namespace BusinessLogicForPushNotification.Interface
{
    public interface IPushNotificationManagement
    {
        public Token Add(Token token);
    }
}
