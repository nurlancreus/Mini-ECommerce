using MediatR;
using Mini_ECommerce.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.User.GetAllUsers
{
    public class GetAllUsersQueryResponse : IRequest<GetAllUsersQueryRequest>
    {
        public bool Success { get; set; }
        public List<GetAppUserVM> Users { get; set; } = [];
        public int TotalUserCount { get; set; }
        public int TotalPagesCount { get; set; }
        public int CurrentPage { get; set; }
        public string? Message { get; set; }
    }
}
