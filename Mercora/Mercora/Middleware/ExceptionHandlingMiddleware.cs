using Mercora.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Mercora.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessRuleException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/problem+json";

                var problem = new
                {
                    type = "https://mercora.dev/errors/business-rule",
                    title = "Business rule violated",
                    status = 400,
                    detail = ex.Message,
                    errorCode = ex.ErrorCode,
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/problem+json";

                var problem = new
                {
                    type = "https://mercora.dev/errors/invalid-request",
                    title = "Invalid request",
                    status = 400,
                    detail = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/problem+json";

                var problem = new
                {
                    type = "https://mercora.dev/errors/unhandled",
                    title = "Unexpected error",
                    status = 500,
                    detail = "An unexpected error occurred."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}
