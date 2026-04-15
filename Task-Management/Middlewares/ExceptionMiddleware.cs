using System.Text.Json;
using TaskManagement.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Task_Management.Middlewares
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var endpoint = context.GetEndpoint();
            var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            string controllerName = actionDescriptor?.ControllerName ?? "System";
            string methodName = actionDescriptor?.ActionName ?? context.Request.Path;

            LogException(controllerName, methodName, ex.Message, ex.ToString(), ex.StackTrace ?? string.Empty);

            context.Response.ContentType = "application/json";

            if (ex is AppException appEx)
            {
                context.Response.StatusCode = appEx.StatusCode;
                var response = new
                {
                    message = appEx.Message,
                    statusCode = appEx.StatusCode
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            else
            {
                context.Response.StatusCode = 500;
                var response = new
                {
                    message = "Something went wrong",
                    details = ex.Message
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

        private void LogException(string controller, string method, string message, string exception, string stackTrace)
        {
            _logger.LogError("Exception in {Controller}.{Method}: {Message}\nException: {Exception}\nStackTrace: {StackTrace}",
                controller, method, message, exception, stackTrace);
        }
    }
}