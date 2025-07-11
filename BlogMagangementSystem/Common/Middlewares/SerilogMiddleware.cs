
using Serilog;

namespace BlogMagangementSystem.Common.Middlewares
{
    public class SerilogMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Log.Information("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);

            await next(context);

            Log.Information("Finished handling request. Status Code: {StatusCode}", context.Response.StatusCode);
        }
    }
}
