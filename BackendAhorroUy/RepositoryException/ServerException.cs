using System;

namespace RepositoryException
{
    public class ServerException : Exception
    {
        public ServerException() : base() { }
        public ServerException(String message) : base(message) { }
    }
}
