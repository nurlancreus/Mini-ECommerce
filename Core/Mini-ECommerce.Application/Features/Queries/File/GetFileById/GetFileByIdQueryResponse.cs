using MediatR;
using Mini_ECommerce.Application.Responses;
using Mini_ECommerce.Application.ViewModels.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.File.GetFileById
{
    public class GetFileByIdQueryResponse : BaseResponse, IRequest<GetFileByIdQueryRequest>
    {
        public GetAppFileVM File {  get; set; }
    }
}
