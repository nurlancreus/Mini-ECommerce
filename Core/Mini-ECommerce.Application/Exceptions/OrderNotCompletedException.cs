using Mini_ECommerce.Application.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Exceptions
{
    public class OrderNotCompletedException : BaseException
    {
        public OrderNotCompletedException() : base(HttpStatusCode.BadRequest, "Cannot complete the order.")
        {

        }

        public OrderNotCompletedException(string message) : base(HttpStatusCode.BadRequest, message)
        {

        }

        public OrderNotCompletedException(HttpStatusCode statusCode) : base(statusCode, "Cannot complete the order.")
        {

        }

        public OrderNotCompletedException(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {

        }

        public OrderNotCompletedException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
