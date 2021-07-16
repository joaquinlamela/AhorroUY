using System;

namespace BusinessLogicException
{
    public class DomainBusinessLogicException : Exception
    {
        public DomainBusinessLogicException(String message) : base(message) { }

    }
}
