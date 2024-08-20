using Mini_ECommerce.Application.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Exceptions
{
    public class RegistrationException : BaseException
    {
        public RegistrationException() : base(HttpStatusCode.BadRequest, "Cannot register user")
        {
            
        }
        
        public RegistrationException(string message) : base(HttpStatusCode.BadRequest, message)
        {
            
        }
    }
}
