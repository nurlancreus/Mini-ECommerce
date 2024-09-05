using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.File.GetFiles
{
    public class GetFilesQueryRequest : PaginationParams, IRequest<GetFilesQueryResponse>
    {
        [FromQuery]
        public string? PathName { get; set; }
    }
}
