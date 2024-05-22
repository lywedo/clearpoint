using System;

namespace TodoList.Api.Exceptions
{
    public class StatusCodeException : Exception
    {
        public int StatusCode { get; }
        public StatusCodeException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public StatusCodeException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public StatusCodeException(string message, Exception inner, int statusCode)
            : base(message, inner)
        {
            StatusCode = statusCode;
        }
    }
}
