using MediatR;
using Mini_ECommerce.Application.DTOs.File;
using Mini_ECommerce.Application.Responses;
using Mini_ECommerce.Application.ViewModels.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.File.GetFiles
{
    public class GetFilesQueryResponse : BaseResponse, IRequest<GetFilesQueryRequest>
    {
        public int PageCount { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public List<GetAppFileVM> Files { get; set; }
    }
}
