using FluentValidation;
using Mini_ECommerce.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.Product
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductVM>
    {
        public UpdateProductValidator()
        {
            RuleFor(p => p.Name)
               .NotEmpty()
                   .WithMessage("Please do not leave the product name empty.")
               .NotNull()
                   .WithMessage("Product name is required.")
               .Length(5, 150)
                   .WithMessage("The product name must be between 5 and 150 characters.");

            RuleFor(p => p.Stock)
                .NotNull()
                    .WithMessage("Stock information is required.")
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Stock cannot be negative.");

            RuleFor(p => p.Price)
                .NotNull()
                    .WithMessage("Price information is required.")
                .GreaterThanOrEqualTo(0.0f)
                    .WithMessage("Price cannot be negative.")
                    .WithMessage("Price must have up to 2 decimal places.");
        }
    }
}
