using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicException
{
    public class ClientBusinessLogicException : Exception
    {
        public ClientBusinessLogicException() : base() { }

        public ClientBusinessLogicException(String message) : base(message) { }

        public ClientBusinessLogicException(String message, Exception exception) : base(message, exception) { }
    }
}
