using Glowee.Api.Models;
using Glowee.Application.Exceptions;
using System.Net;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            CustomProblemDetails problem = new CustomProblemDetails();

            switch (ex)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    problem = new CustomProblemDetails
                    {
                        Title = badRequestException.Message,
                        Status = (int)statusCode,
                        Type = nameof(BadRequestException),
                        Detail = badRequestException.InnerException?.Message,
                        Errors = badRequestException.ValidationErrors
                    };
                    break;
                case NotFoundException notFound:
                    statusCode = HttpStatusCode.NotFound;
                    problem = new CustomProblemDetails
                    {
                        Title = notFound.Message,
                        Status = (int)statusCode,
                        Type = nameof(BadRequestException),
                        Detail = notFound.InnerException?.Message
                    };
                    break;
                case InternalServerException internalServer:
                    statusCode = HttpStatusCode.InternalServerError;
                    problem = new CustomProblemDetails
                    {
                        Title = internalServer.Message,
                        Status = (int)statusCode,
                        Type = nameof(InternalServerException),
                        Detail = internalServer.InnerException?.Message
                    };
                    break;
                case ForbiddenException forbidden:
                    statusCode = HttpStatusCode.Forbidden;
                    problem = new CustomProblemDetails
                    {
                        Title = forbidden.Message,
                        Status = (int)statusCode,
                        Type = nameof(ForbiddenException),
                        Detail = forbidden.InnerException?.Message
                    };
                    break;
                case UnauthorizedAccessException unauthorized:
                    statusCode = HttpStatusCode.Unauthorized;
                    problem = new CustomProblemDetails
                    {
                        Title = unauthorized.Message,
                        Status = (int)statusCode,
                        Type = nameof(UnauthorizedAccessException),
                        Detail = unauthorized.InnerException?.Message
                    };
                    break;
                default:
                    problem = new CustomProblemDetails
                    {
                        Title = ex.Message,
                        Status = (int)statusCode,
                        Type = nameof(HttpStatusCode.InternalServerError),
                        Detail = ex.StackTrace
                    };
                    break;
            }

            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(problem);
        }
    }
}
