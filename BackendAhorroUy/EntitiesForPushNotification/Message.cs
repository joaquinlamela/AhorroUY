using System;

namespace EntitiesForPushNotification
{
    public class Message
    {
        public string[] registration_ids { get; set; }
        public Notification notification { get; set; }
        public string priority { get; set; }

    }
}
