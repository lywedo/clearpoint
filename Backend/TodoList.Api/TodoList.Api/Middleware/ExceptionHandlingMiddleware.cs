using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TodoList.Api.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace TodoList.Api.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error processing invocation");

                if (context.Request.Body.CanRead)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("JSON input parameter error");
                }
            }
            catch (StatusCodeException ex)
            {
                _logger.LogError(ex, "Error processing invocation");

                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing invocation");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Unknown error occurred");
            }
        }
    }
}
