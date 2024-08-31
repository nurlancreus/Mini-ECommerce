﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using Mini_ECommerce.Application.Attributes;
using Mini_ECommerce.Application.Enums;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Exceptions.Base;
using Mini_ECommerce.Application.Features.Commands.Product.CreateProduct;
using Mini_ECommerce.Application.Features.Commands.Product.RemoveProduct;
using Mini_ECommerce.Application.Features.Commands.Product.UpdateProduct;
using Mini_ECommerce.Application.Features.Commands.ProductImageFile.UploadProductImage;
using Mini_ECommerce.Application.Features.Queries.Product.GetAllProduct;
using Mini_ECommerce.Application.Features.Queries.Product.GetProductById;
using Mini_ECommerce.Application.RequestParameters;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Enums;
using System.Net;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        // private readonly IFileService _fileService;
        private readonly IStorageService _storageService;

        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;

        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;

        private readonly IMediator _mediator;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment, IStorageService storageService, IProductImageFileWriteRepository productImageFileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IMediator mediator)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            // _fileService = fileService;
            _storageService = storageService;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Products, ActionType = ActionType.Reading, Definition = "Get All Products")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            var response = await _mediator.Send(getAllProductQueryRequest);

            return Ok(response);

        }

        [HttpGet("{Id}")]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Products, ActionType = ActionType.Reading, Definition = "Get Product")]
        public async Task<IActionResult> Get(GetProductByIdQueryRequest getProductByIdQueryRequest)
        {
            var response = await _mediator.Send(getProductByIdQueryRequest);

            return Ok(response);
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Products, ActionType = ActionType.Writing, Definition = "Create Product")]
        public async Task<IActionResult> Create(CreateProductCommandRequest createProductCommandRequest)
        {
            await _mediator.Send(createProductCommandRequest);

            return Created();
        }

        [HttpPut]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Products, ActionType = ActionType.Updating, Definition = "Update Product")]
        public async Task<IActionResult> Update(UpdateProductCommandRequest updateProductCommandRequest)
        {
            await _mediator.Send(updateProductCommandRequest);

            return Created();
        }

        [HttpDelete]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Products, ActionType = ActionType.Deleting, Definition = "Delete Product")]
        public async Task<IActionResult> Delete(RemoveProductCommandRequest removeProductCommandRequest)
        {
            await _mediator.Send(removeProductCommandRequest);

            return NoContent();

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFiles()
        {
            var files = await _storageService.GetFilesAsync("files");

            return Ok(files);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteFile([FromRoute] string id)
        {
            var file = await _productImageFileReadRepository.GetByIdAsync(id);

            if (file != null)
            {
                bool isDeleted = await _productImageFileWriteRepository.RemoveAsync(id);

                if (isDeleted)
                {
                    await _productImageFileWriteRepository.SaveAsync();
                    await _storageService.DeleteAsync("files", file.FileName);
                }
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            var response = await _mediator.Send(uploadProductImageCommandRequest);

            return Ok(response);
        }
    }
}
