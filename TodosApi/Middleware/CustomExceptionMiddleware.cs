using Models;
using Newtonsoft.Json;
using System.Net;

namespace TodosApi.Middleware
{
   public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            ApiResponse response;

            switch (exception)
            {
                case KeyNotFoundException _:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = new ApiResponse
                    {
                        Code = context.Response.StatusCode,
                        Message = "NotFound",
                        Data = exception.Message
                    };
                    break;
                case UnauthorizedAccessException _:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response = new ApiResponse
                    {
                        Code = context.Response.StatusCode,
                        Message = "Unauthorized",
                        Data = exception.Message
                    };
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                    response = new ApiResponse
                    {
                        Code = context.Response.StatusCode,
                        Message = "BadGateway",
                        Data = exception.Message
                    };
                    break;
            }

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
