using FluentValidation;
using Mini_ECommerce.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.Product
{
    public class CreateProductValidator : AbstractValidator<CreateProductVM>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage("Please do not leave the product name empty.")
                .NotNull()
                    .WithMessage("Product name is required.")
                .Length(5, 150)
                    .WithMessage("The product name must be between 5 and 150 characters.")
                .Matches(@"^[a-zA-Z0-9\s]*$")
                    .WithMessage("The product name can only contain letters, numbers, and spaces.");

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
        }
    }
}
