using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Options.Storage.AWS
{
    public class AWSOptions
    {
        public string AccessKey { get; set; }
        public string SecretAccessKey { get; set; }
        public string Region { get; set; }
        public S3Options AWSS3 { get; set; }
    }
}
