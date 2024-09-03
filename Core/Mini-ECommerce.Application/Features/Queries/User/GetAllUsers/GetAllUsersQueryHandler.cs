using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.User.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
    {
        private readonly IUserService _userService;

        public GetAllUsersQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllUsersAsync(request.Page, request.PageSize);

            return new GetAllUsersQueryResponse()
            {
                Success = true,
                Users = result.Users.Select(u => new GetAppUserVM()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserName = u.UserName,

                }).ToList(),
                CurrentPage = result.CurrentPage,
                TotalPagesCount = result.TotalPagesCount,
                TotalUserCount = result.TotalUserCount,
            };
        }
    }
}
