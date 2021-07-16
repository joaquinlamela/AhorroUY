using System;

namespace Domain
{
    public class UserSession
    {
        public Guid Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }

        public UserSession()
        {
            Date = DateTime.Now; 
        }

    }
}
