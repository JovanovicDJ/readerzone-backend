using Microsoft.AspNetCore.Mvc;
using readerzone_api.Exceptions;
using System.Net;
using System.Text.Json;

namespace readerzone_api.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException nfe)
            {
                _logger.LogError(nfe, nfe.Message);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Type = "Not found error",
                    Title = "Not found error",
                    Detail = nfe.Message
                };
                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (NotCreatedException nce)
            {
                _logger.LogError(nce, nce.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = "Not created error",
                    Title = "Not created error",
                    Detail = nce.Message
                };
                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (NotUpdatedException nue)
            {
                _logger.LogError(nue, nue.Message);                
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.Conflict,
                    Type = "Not updated error",
                    Title = "Not updated error",
                    Detail = nue.Message
                };
                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (FailedLoginException fle)
            {
                _logger.LogError(fle, fle.Message);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Type = "Failed login error",
                    Title = "Failed login error",
                    Detail = fle.Message
                };
                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (NotValidAccountException nvae)
            {
                _logger.LogError(nvae, nvae.Message);
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.Forbidden,
                    Type = "User account is not valid",
                    Title = "User account is not valid",
                    Detail = nvae.Message
                };
                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = e.Message
                };

                var json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }
    }
}
