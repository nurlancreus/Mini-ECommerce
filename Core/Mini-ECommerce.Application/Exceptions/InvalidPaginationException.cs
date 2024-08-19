using Mini_ECommerce.Application.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Exceptions
{
    public enum PaginationErrorType
    {
        InvalidPageSize,
        InvalidPageNumber
    }

    public class InvalidPaginationException : BaseException
    {
        public PaginationErrorType ErrorType { get; }
        public int InvalidValue { get; }

        public InvalidPaginationException(PaginationErrorType errorType, int invalidValue)
            : base(GenerateMessage(errorType, invalidValue))
        {
            ErrorType = errorType;
            InvalidValue = invalidValue;
        }

        private static string GenerateMessage(PaginationErrorType errorType, int invalidValue)
        {
            return errorType switch
            {
                PaginationErrorType.InvalidPageSize => $"Invalid page size: {invalidValue}. Page size must be a positive number and cannot exceed 100.",
                PaginationErrorType.InvalidPageNumber => $"Invalid page number: {invalidValue}.",
                _ => "Invalid pagination parameter."
            };
        }
    }
}
