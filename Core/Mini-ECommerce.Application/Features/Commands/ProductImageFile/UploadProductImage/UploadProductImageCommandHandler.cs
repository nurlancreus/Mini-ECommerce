using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        private readonly IStorageService _storageService;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public UploadProductImageCommandHandler(IStorageService storageService, IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _storageService = storageService;
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(request.Id);

            if (request.Files == null || !request.Files.Any())
            {
                throw new ArgumentException("No files were provided for upload.");
            }

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), request.Id);
            }

            var uploadResults = await _storageService.UploadAsync("product-images", request.Files);

            var productImages = uploadResults.Select(result => new Domain.Entities.ProductImageFile
            {
                FileName = result.fileName,
                Path = result.pathOrContainerName,
                Storage = Enum.Parse<StorageType>(_storageService.StorageName),
                Products = [product]
            }).ToList();

            var isAdded = await _productImageFileWriteRepository.AddRangeAsync(productImages);

            if (isAdded)
            {
                await _storageService.DeleteAllAsync("product-images");

                throw new Exception("Could not save files in the database, Something happened");
            }

            await _productImageFileWriteRepository.SaveAsync();

            return new UploadProductImageCommandResponse()
            {
                Message = $"{(request.Files.Count > 1 ? "Files" : "File")} added successfully!"
            };

        }
    }
}
