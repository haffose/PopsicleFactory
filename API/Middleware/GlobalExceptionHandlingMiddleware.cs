using API.Models.Responses;
using System.Net;
using System.Text.Json;

namespace API.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate Next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> Logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        Next = next;
        Logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        ErrorResponse errorResponse;

        if (exception is ArgumentException argEx)
        {
            errorResponse = new ErrorResponse
            {
                Message = argEx.Message,
                Details = "Invalid argument provided"
            };
        }
        else if (exception is KeyNotFoundException)
        {
            errorResponse = new ErrorResponse
            {
                Message = "The requested resource was not found",
                Details = exception.Message
            };
        }
        else if (exception is UnauthorizedAccessException)
        {
            errorResponse = new ErrorResponse
            {
                Message = "Unauthorized access",
                Details = "You don't have permission to access this resource"
            };
        }
        else
        {
            errorResponse = new ErrorResponse
            {
                Message = "An internal server error occurred",
                Details = "Please try again later"
            };
        }

        if (exception is ArgumentException)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        else if (exception is KeyNotFoundException)
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        else if (exception is UnauthorizedAccessException)
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(jsonResponse);
    }
}