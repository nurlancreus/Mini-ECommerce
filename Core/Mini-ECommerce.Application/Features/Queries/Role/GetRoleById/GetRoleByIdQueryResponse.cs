using MediatR;
using Mini_ECommerce.Application.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Role.GetRoleById
{
    public class GetRoleByIdQueryResponse : IRequest<GetRoleByIdQueryRequest>
    {
        public GetRoleVM Role {  get; set; }
        public string? Message { get; set; }
    }
}
