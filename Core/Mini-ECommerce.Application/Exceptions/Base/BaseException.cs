using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Exceptions.Base
{
    public abstract class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
        public string Description { get; }

        protected BaseException()
        {
            
        }

        protected BaseException(string message) : base(message)
        {
            
        }

        protected BaseException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            StatusCode = httpStatusCode;
        }

        protected BaseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
