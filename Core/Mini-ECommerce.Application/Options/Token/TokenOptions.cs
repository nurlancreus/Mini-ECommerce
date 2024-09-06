using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Options.Token
{
    public class TokenOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecurityKey { get; set; }
        public int AccessTokenLifeTimeInMinutes { get; set; }
        public int RefreshTokenLifeTimeInMinutes { get; set; }
    }
}
