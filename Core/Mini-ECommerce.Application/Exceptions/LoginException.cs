using Mini_ECommerce.Application.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Exceptions
{
    public class LoginException : BaseException
    {
        public LoginException() : base(HttpStatusCode.BadRequest, "Cannot log in user.")
        {

        }

        public LoginException(string message) : base(HttpStatusCode.BadRequest, message)
        {

        }
    }
}
