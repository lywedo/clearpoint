using System;

namespace TodoList.Api.Exceptions
{
    public class DatabaseException : StatusCodeException
    {
        private const int InternalServerErrorStatusCode = 500;
        public DatabaseException() : base(InternalServerErrorStatusCode)
        {
        }

        public DatabaseException(string message)
            : base(message, InternalServerErrorStatusCode)
        {
        }

        public DatabaseException(string message, Exception inner)
            : base(message, inner, InternalServerErrorStatusCode)
        {
        }
    }
}
