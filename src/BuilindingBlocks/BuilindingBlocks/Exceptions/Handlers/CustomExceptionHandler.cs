using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

namespace BuildingBlocks.Exceptions.Handlers;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {exceptionMessage}, Time of occurence {time}",
            exception.Message, DateTime.UtcNow);

        (string Details, string Title, int StatusCode) details = exception switch
        {
            NotFoundException => (exception.Message, exception.GetType().Name, StatusCodes.Status404NotFound),
            InternalServerException => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError),
            BadRequestException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
            ValidationException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
            _ => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError)
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Details,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);
        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        context.Response.StatusCode = details.StatusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
