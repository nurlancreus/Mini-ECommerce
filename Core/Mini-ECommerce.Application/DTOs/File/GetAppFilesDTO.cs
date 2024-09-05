using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.File
{
    public class GetAppFilesDTO
    {
        public int PageCount { get; set; }
        public int TotalItems {  get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<GetAppFileDTO> Files { get; set; }
    }
}
