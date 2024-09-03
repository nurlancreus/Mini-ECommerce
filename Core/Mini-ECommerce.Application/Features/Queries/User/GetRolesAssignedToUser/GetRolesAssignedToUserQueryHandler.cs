using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.User.GetRolesAssignedToUser
{
    public class GetRolesAssignedToUserQueryHandler : IRequestHandler<GetRolesAssignedToUserQueryRequest, GetRolesAssignedToUserQueryResponse>
    {
        private readonly IUserService _userService;

        public GetRolesAssignedToUserQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetRolesAssignedToUserQueryResponse> Handle(GetRolesAssignedToUserQueryRequest request, CancellationToken cancellationToken)
        {
            var roles = await _userService.GetRolesAssignedToUserAsync(request.Id);

            return new GetRolesAssignedToUserQueryResponse()
            {
                Success = true,
                Roles = roles.Select(r => new ViewModels.Role.GetRoleVM()
                {
                    Id = r.Id,
                    Name = r.Name,

                }).ToList()
            };
        }
    }
}
