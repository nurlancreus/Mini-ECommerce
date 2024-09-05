using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.ViewModels.File;
using Mini_ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.File.GetFileById
{
    public class GetFileByIdQueryHandler : IRequestHandler<GetFileByIdQueryRequest, GetFileByIdQueryResponse>
    {
        private readonly IFileService _fileService;
        private readonly IAppFileReadRepository _appFileReadRepository;

        public GetFileByIdQueryHandler(IFileService fileService, IAppFileReadRepository appFileReadRepository)
        {
            _fileService = fileService;
            _appFileReadRepository = appFileReadRepository;
        }

        public async Task<GetFileByIdQueryResponse> Handle(GetFileByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var file = await _fileService.GetAsync(request.Id, _appFileReadRepository);

            return new GetFileByIdQueryResponse()
            {
                File = new GetAppFileVM()
                {
                    Id = file.Id,
                    Path = file.Path,
                    FileName = file.FileName,
                    CreatedAt = file.CreatedAt,
                }
            };
        }
    }
}
