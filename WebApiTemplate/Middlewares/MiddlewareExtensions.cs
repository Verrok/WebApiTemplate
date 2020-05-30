using Microsoft.AspNetCore.Builder;

namespace WebApiTemplate.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}