using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Options.Auth.External
{
    public class ExternalLoginSettingsOptions
    {
        public GoogleOptions Google { get; set; }
        public FacebookOptions Facebook { get; set; }
    }
}
