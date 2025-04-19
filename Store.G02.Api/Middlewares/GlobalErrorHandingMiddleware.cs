using Domain.Exceptions;
using Shared.ErrorsModels;

namespace Store.G02.Api.Middlewares
{
    public class GlobalErrorHandingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandingMiddleware> _logger;

        public GlobalErrorHandingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandingMiddleware> logger) 
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync (HttpContext context) 
        {
            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandingNotFoundEndPointAsync(context);

                }

            }
            catch (Exception ex)
            {
                // log Exception
                _logger.LogError(ex, ex.Message);
                await HandingErrorAsync(context, ex);

            }
        }

        private static async Task HandingErrorAsync(HttpContext context, Exception ex)
        {

            // 1. Set Status Code For Response
            // 2. Set Content Type Code For Response
            // 3. Response Object (Body) 
            // 4. Return Response

            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";


            var response = new ErrorDetails()
            {

                ErrorMessage = ex.Message
            };

            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = response.StatusCode;


            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"End Point {context.Request.Path} is Not Found"

            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
