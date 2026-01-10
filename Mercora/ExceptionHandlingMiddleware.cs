using System.Net;
using System.Text.Json;
using Mercora.Application.Common.Exceptions;

namespace Mercora.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
    }
}


