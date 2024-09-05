using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.ViewModels.File
{
    public class GetAppFileVM
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
