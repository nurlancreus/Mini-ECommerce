using FluentValidation;
using Mini_ECommerce.Application.Features.Commands.ProductImageFile.UploadProductImage;
using Mini_ECommerce.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.ProductImageFile
{
    public class UploadProductImageCommandRequestValidator : AbstractValidator<UploadProductImageCommandRequest>
    {
        private const int MaxFileSizeMB = 3;
        public UploadProductImageCommandRequestValidator()
        {

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
