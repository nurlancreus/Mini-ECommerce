using Mini_ECommerce.Application.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Exceptions
{
    public class PasswordChangeFailedException : BaseException
    {
        public PasswordChangeFailedException() : base("Şifre güncellenirken bir sorun oluştu.")
        {
        }

        public PasswordChangeFailedException(string message) : base(HttpStatusCode.BadRequest, message)
        {
        }

        public PasswordChangeFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
