using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Exceptions.Base
{
    public class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

        public BaseException()
        {
            
        }

        public BaseException(string message) : base(message)
        {
            
        }

        public BaseException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            StatusCode = httpStatusCode;
        }

        public BaseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
