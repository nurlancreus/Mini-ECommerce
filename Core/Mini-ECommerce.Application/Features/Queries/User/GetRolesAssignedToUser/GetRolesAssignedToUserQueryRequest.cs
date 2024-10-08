﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.User.GetRolesAssignedToUser
{
    public class GetRolesAssignedToUserQueryRequest : IRequest<GetRolesAssignedToUserQueryResponse>
    {
        public string Id { get; set; }
    }
}
