using Mini_ECommerce.Application.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Exceptions
{
    public class RoleException : BaseException
    {
        public RoleException() : base(HttpStatusCode.BadRequest, "Cannot create the role.")
        {

        }

        public RoleException(string message) : base(HttpStatusCode.BadRequest, message)
        {

        }

        public RoleException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
