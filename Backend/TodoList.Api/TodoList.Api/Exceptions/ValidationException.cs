using System;

namespace TodoList.Api.Exceptions
{    public class ValidationException : StatusCodeException
    {
        private const int BadRequestStatusCode = 400;
        public ValidationException() : base(BadRequestStatusCode)
        {
        }

        public ValidationException(string message)
            : base(message, BadRequestStatusCode)
        {
        }

        public ValidationException(string message, Exception inner)
            : base(message, inner, BadRequestStatusCode)
        {
        }
    }
}
