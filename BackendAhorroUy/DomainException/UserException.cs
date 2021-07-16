using System;


namespace DomainException
{
    public class UserException : Exception
    {
        public UserException(String message) : base(message) { }
    }
}
