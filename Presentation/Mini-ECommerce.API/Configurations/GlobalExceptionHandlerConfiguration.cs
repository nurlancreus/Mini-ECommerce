using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;
using System.Net;
using System.Text.Json;
using FluentValidation;  // Add this import to handle FluentValidation exceptions
using Mini_ECommerce.Application.Exceptions.Base;

namespace Mini_ECommerce.API.Configurations
{
    public static class GlobalExceptionHandlerConfiguration
    {
        public static void ConfigureExceptionHandler<T>(this WebApplication application, ILogger<T> logger)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;
                        HttpStatusCode statusCode;
                        string message;
                        string track = string.Empty;

                        if (exception is BaseException baseException)
                        {
                            statusCode = baseException.StatusCode;  // Use the status code from the custom exception
                            message = baseException.Message;        // Use the message from the custom exception
                        }
                        else if (exception is ValidationException validationException)
                        {
                            statusCode = HttpStatusCode.BadRequest; // Set status code to 400 for validation errors
                            message = "Validation failed: " + string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage));
                        }
                        else
                        {
                            statusCode = HttpStatusCode.InternalServerError;
                            message = exception.Message; // Generic error message for other exceptions
                            track = exception.StackTrace ?? "";
                        }

                        // Log the error
                        logger.LogError(exception, message);

                        context.Response.StatusCode = (int)statusCode; // Set the response status code
                        var response = new
                        {
                            context.Response.StatusCode,
                            Message = message,
                            Title = "Exception caught!",
                            track,
                        };


                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });
        }
    }
}
