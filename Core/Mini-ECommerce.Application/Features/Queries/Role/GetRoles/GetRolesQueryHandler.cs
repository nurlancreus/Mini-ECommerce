using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRolesQueryResponse>
    {
        private readonly IRoleService _roleService;

        public GetRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRolesQueryResponse> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleService.GetAllRolesAsync(request.Page, request.PageSize);

            return new GetRolesQueryResponse()
            {
                Roles = result.Roles.Select(role => new GetRoleVM()
                {
                    Id = role.Id,
                    Name = role.Name,
                }).ToList(),
                TotalCount = result.Count,
            };
        }
    }
}
