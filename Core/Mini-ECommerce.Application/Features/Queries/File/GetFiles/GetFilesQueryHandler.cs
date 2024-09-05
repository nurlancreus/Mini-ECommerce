using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.File;
using Mini_ECommerce.Application.ViewModels.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.File.GetFiles
{
    public class GetFilesQueryHandler : IRequestHandler<GetFilesQueryRequest, GetFilesQueryResponse>
    {
        private readonly IFileService _fileService;
        private readonly IAppFileReadRepository _appFileReadRepository;

        public GetFilesQueryHandler(IFileService fileService, IAppFileReadRepository appFileReadRepository)
        {
            _fileService = fileService;
            _appFileReadRepository = appFileReadRepository;
        }

        public async Task<GetFilesQueryResponse> Handle(GetFilesQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _fileService.GetFilesAsync(request.Page, request.PageSize, request.PathName, _appFileReadRepository);

            return new GetFilesQueryResponse()
            {
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                PageCount = result.PageCount,
                TotalItems = result.TotalItems,
                Files = result.Files.Select(f => new GetAppFileVM()
                {
                    Id = f.Id.ToString(),
                    FileName = f.FileName,
                    Path = f.Path,
                    CreatedAt = f.CreatedAt,
                }).ToList(),
            };
        }
    }
}
