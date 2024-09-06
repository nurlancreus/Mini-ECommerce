﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Features.Commands.Product.CreateProduct;
using Mini_ECommerce.Application.ViewModels.Product;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mini_ECommerce.Application.Helpers; // For IFormFile

namespace Mini_ECommerce.Application.Validators.Product
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommandRequest>
    {
        private readonly IProductReadRepository _productReadRepository;
        private const int MaxFileSizeMB = 3;

        public CreateProductCommandValidator(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;

            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage("Please do not leave the product name empty.")
                .NotNull()
                    .WithMessage("Product name is required.")
                .Length(5, 150)
                    .WithMessage("The product name must be between 5 and 150 characters.")
                .Matches(@"^[a-zA-Z0-9\s]*$")
                    .WithMessage("The product name can only contain letters, numbers, and spaces.")
                .MustAsync(async (name, cancellation) =>
                {
                    bool isExist = await _productReadRepository.Table.AnyAsync(product => product.Name == name, cancellationToken: cancellation);
                    return !isExist;
                })
                .WithMessage("Name must be unique");

            RuleFor(p => p.Stock)
                .NotNull()
                    .WithMessage("Stock information is required.")
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Stock cannot be negative.")
                .LessThanOrEqualTo(10000)
                    .WithMessage("Stock cannot exceed 10,000 units.");

            RuleFor(p => p.Price)
                .NotNull()
                    .WithMessage("Price information is required.")
                .GreaterThanOrEqualTo(0.0f)
                    .WithMessage("Price cannot be negative.");

            // Validation for product images
            RuleForEach(p => p.ProductImages)
                .NotNull()
                    .WithMessage("Each product image is required.")
                .Must(file => file.IsSizeOk(MaxFileSizeMB))
                    .WithMessage($"File size should not exceed {MaxFileSizeMB} MB.")
                .Must(file => file.RestrictExtension([".jpg", ".png", ".gif"]))
                    .WithMessage("Only .jpg, .png, and .gif files are allowed.")
                .Must(file => file.RestrictMimeTypes(["image/jpeg", "image/png", "image/gif"]))
                    .WithMessage("Only image files (JPEG, PNG, GIF) are allowed.");
        }
    }
}
