using System.Net;
using System.Text.Json;

namespace ProductWebApi.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _request;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate request, ILogger<GlobalExceptionMiddleware> logger)
    {
        _request = request;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _request(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception. Method: {Method}, Path: {Path}",
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode = (int)HttpStatusCode.InternalServerError;

        string message = "Đã xảy ra lỗi hệ thống.";

        if (exception is KeyNotFoundException)
        {
            statusCode = (int)HttpStatusCode.NotFound;
            message = exception.Message;
        }
        else if (exception is ArgumentException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            message = exception.Message;
        }

        context.Response.StatusCode = statusCode;

        object response = new
        {
            status = statusCode,
            message = message
        };

        string json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}