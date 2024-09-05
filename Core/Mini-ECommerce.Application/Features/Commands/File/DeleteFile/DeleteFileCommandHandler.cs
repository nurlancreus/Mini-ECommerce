using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.File.DeleteFile
{
    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommandRequest, DeleteFileCommandResponse>
    {
        private readonly IFileService _fileService;
        private readonly IAppFileWriteRepository _appFileWriteRepository;
        private readonly IAppFileReadRepository _appFileReadRepository;
        public DeleteFileCommandHandler(IFileService fileService, IAppFileWriteRepository appFileWriteRepository, IAppFileReadRepository appFileReadRepository)
        {
            _fileService = fileService;
            _appFileWriteRepository = appFileWriteRepository;
            _appFileReadRepository = appFileReadRepository;
        }

        public async Task<DeleteFileCommandResponse> Handle(DeleteFileCommandRequest request, CancellationToken cancellationToken)
        {
            await _fileService.DeleteAsync(request.Id, _appFileWriteRepository, _appFileReadRepository);

            return new DeleteFileCommandResponse()
            {
                Success = true,
                Message = "File deleted successfully!"
            };
        }
    }
}
