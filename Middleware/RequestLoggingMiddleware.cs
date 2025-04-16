using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FreeBirds.Services;

namespace FreeBirds.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public RequestLoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var logService = scope.ServiceProvider.GetRequiredService<LogService>();
                    await logService.LogInfoAsync(
                        $"Request {context.Request.Method} {context.Request.Path} completed with status code {context.Response.StatusCode}",
                        "RequestLoggingMiddleware"
                    );
                }
            }
        }
    }
}