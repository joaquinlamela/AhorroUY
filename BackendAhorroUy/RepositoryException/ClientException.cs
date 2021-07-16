using System;

namespace RepositoryException
{
    public class ClientException : Exception
    {
        public ClientException() : base() { }

        public ClientException(String message) : base(message) { }
    }
}
