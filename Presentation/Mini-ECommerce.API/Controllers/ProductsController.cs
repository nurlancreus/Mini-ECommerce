using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using Mini_ECommerce.Application.Attributes;
using Mini_ECommerce.Domain.Enums;
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
using System.Net;
using Mini_ECommerce.Persistence.Concretes.Services;
using Mini_ECommerce.Application.Features.Commands.Product.UpdateStockQrCodeToProduct;
using Mini_ECommerce.Application.Features.Queries.GetQrCodeToProduct;
using Mini_ECommerce.Application.Features.Commands.ProductImageFile.ChangeMainImage;
using Mini_ECommerce.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using Mini_ECommerce.Application.Features.Queries.ProductImageFile.GetProductImages;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ProductsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Products, ActionType = ActionType.Reading, Definition = "Get All Products")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsQueryRequest getAllProductQueryRequest)
        {
            var response = await _mediator.Send(getAllProductQueryRequest);

            return Ok(response);

        }

        [HttpGet("{Id}")]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Products, ActionType = ActionType.Reading, Definition = "Get Product")]
        public async Task<IActionResult> Get([FromRoute] GetProductByIdQueryRequest getProductByIdQueryRequest)
        {
            var response = await _mediator.Send(getProductByIdQueryRequest);

            return Ok(response);
        }

        [HttpPost]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Products, ActionType = ActionType.Writing, Definition = "Create Product")]
        public async Task<IActionResult> Create([FromForm] CreateProductCommandRequest createProductCommandRequest)
        {
           var response = await _mediator.Send(createProductCommandRequest);

            return Ok(response);
        }

        [HttpPut("{Id}")]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Products, ActionType = ActionType.Updating, Definition = "Update Product")]
        public async Task<IActionResult> Update([FromBody, FromRoute] UpdateProductCommandRequest updateProductCommandRequest)
        {
            var response = await _mediator.Send(updateProductCommandRequest);

            return Ok(response);
        }

        [HttpDelete("{Id}")]
        [AuthorizeDefinition(Menu = AuthorizedMenu.Products, ActionType = ActionType.Deleting, Definition = "Delete Product")]
        public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {
            var response = await _mediator.Send(removeProductCommandRequest);

            return Ok(response);
        }

        [HttpGet("qrcode/{ProductId}")]
        public async Task<IActionResult> GetQrCodeToProduct([FromRoute] GetQrCodeToProductQueryRequest getQrCodeToProductQueryRequest)
        {
            var response = await _mediator.Send(getQrCodeToProductQueryRequest);

            return File(response.ImageData, "image/png");
        }

        [HttpPut("qrcode")]
        public async Task<IActionResult> UpdateStockQrCodeToProduct(UpdateStockQrCodeToProductCommandRequest updateStockQrCodeToProductCommandRequest)
        {
            var response = await _mediator.Send(updateStockQrCodeToProductCommandRequest);
            return Ok(response);
        }

        [HttpPost("[action]/{Id}")]
        public async Task<IActionResult> UploadFile([FromRoute] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            var response = await _mediator.Send(uploadProductImageCommandRequest);

            return Ok(response);
        }

        [HttpPut("{ProductId}/[action]/{ProductImageId}")]
        public async Task<IActionResult> ChangeMainImage([FromRoute] ChangeMainImageCommandRequest changeMainImageCommandRequest)
        {
            var response = await _mediator.Send(changeMainImageCommandRequest);

            return Ok(response);
        }

        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
            var response = await _mediator.Send(getProductImagesQueryRequest);

            return Ok(response);
        }

        [HttpDelete("{ProductId}/[action]/{ProductImageId}")]
        public async Task<IActionResult> DeleteFile([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest)
        {
            var response = await _mediator.Send(removeProductImageCommandRequest);

            return Ok(response);
        }
    }
}
