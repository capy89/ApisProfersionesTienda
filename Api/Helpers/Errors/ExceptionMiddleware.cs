using System.Net;
using System.Text.Json;

namespace Api.Helpers.Errors
{
    public class ExceptionMiddleware
    {

        public RequestDelegate _next { get; }
        public ILogger<ExceptionMiddleware> _logger { get; }
        public IHostEnvironment _env { get; }

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch(Exception ex) 
            {
                var statusCode = (int)HttpStatusCode.InternalServerError;

                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = _env.IsDevelopment()
                    ? new ApiException(statusCode, ex.Message, ex.StackTrace.ToString())
                    : new ApiException(statusCode);


                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }


    }
}
