using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.File.UploadFiles
{
    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommandRequest, UploadFileCommandResponse>
    {
        private readonly IStorageService _storageService;
        private readonly IFileService _fileService;
        private readonly IAppFileWriteRepository _appFileWriteRepository;

        public UploadFileCommandHandler(IStorageService storageService, IFileService fileService, IAppFileWriteRepository appFileWriteRepository)
        {
            _storageService = storageService;
            _fileService = fileService;
            _appFileWriteRepository = appFileWriteRepository;
        }

        public async Task<UploadFileCommandResponse> Handle(UploadFileCommandRequest request, CancellationToken cancellationToken)
        {
            var addFile = delegate (string fileName, string pathName, StorageType storageType)
            {
                var productImageFile = new AppFile()
                {
                   Path = pathName,
                   FileName = fileName,
                   Storage = storageType
                };

              return _appFileWriteRepository.AddAsync(productImageFile).Result;
         
            };

            await _fileService.UploadAsync(request.PathName, request.FormFiles,_appFileWriteRepository, addFile);

            return new UploadFileCommandResponse()
            {
                Success = true,
                Message = "Files uploaded successfully!"
            };
        }
    }
}
