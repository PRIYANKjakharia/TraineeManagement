using System.Net;
using System.Text.Json;
 
namespace TraineeManagement.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
 
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
 
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unhandled exception occurred.");
                await HandleExceptionAsync(context);
            }
        }
 
        private static async Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
 
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
 
            var response = new
            {
                Message = "An unexpected error occurred. Please try again later."
            };
 
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}