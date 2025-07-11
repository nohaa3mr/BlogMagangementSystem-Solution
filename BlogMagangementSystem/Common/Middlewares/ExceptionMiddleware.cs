using BlogMagangementSystem.Common.ErrorHandling;
using System.Net;
using System.Text.Json;

namespace BlogMagangementSystem.Common.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{Middleware}] {ExceptionType}: {Message}", nameof(ExceptionMiddleware), ex.GetType().Name, ex.Message);
                httpContext.Response.StatusCode = ex switch
                {
                    InvalidOperationException or ArgumentException => (int)HttpStatusCode.BadRequest, // 400
                    KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404
                    _ => (int)HttpStatusCode.InternalServerError // 500
                };

                httpContext.Response.ContentType = "application/json";

                var response = _env.IsDevelopment()
                    ? new ApiExceptionResponse(httpContext.Response.StatusCode, $"{ex.GetType().Name}: {ex.Message}", ex.StackTrace)
                    : new ApiExceptionResponse(httpContext.Response.StatusCode, "An unexpected error occurred. Please try again later.");

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    
    }
}
