using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.File.GetFileById
{
    public class GetFileByIdQueryRequest : IRequest<GetFileByIdQueryResponse>
    {
        public string Id { get; set; }
    }
}
