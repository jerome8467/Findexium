using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace P7CreateRestApi.Middleware
{
    public class LogEndpointMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<LogEndpointMiddleware> _logger;

        public LogEndpointMiddleware(RequestDelegate requestDelegate, ILogger<LogEndpointMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;
            var method = context.Request.Method;
            var username = context.User.Identity?.Name ?? "Anonyme";

            await _requestDelegate(context);

            int statusCode = context.Response.StatusCode;
            string status = (statusCode >= 200 && statusCode < 300) ? "Success" : "Failure";

            _logger.LogInformation(
                "URL: {path} \nMETHOD: {method} \nUSER: {username} \nSTATUS: {status} \nCODE: {statusCode} ",
                path, method, username, status, statusCode);

        }

    }
}
