using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.User
{
    public class GetAllUsersDTO
    {
        public int TotalUserCount { get; set; }
        public int TotalPagesCount { get; set; }
        public int CurrentPage { get; set; }
        public List<GetAppUserDTO> Users { get; set; } = [];
    }
}
