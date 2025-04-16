using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FreeBirds.Services;

namespace FreeBirds.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public ErrorHandlingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
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
            catch (Exception ex)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var logService = scope.ServiceProvider.GetRequiredService<LogService>();
                    await logService.LogErrorAsync(ex, context.Request.Path);
                }
                
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                
                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An error occurred while processing your request."
                });
            }
        }
    }
} 