using FluentValidation;
using GlassLewisChallange.Application.Exceptions;
using GlassLewisChallange.Infrastructure.Models;
using System.Net;
using System.Text.Json;

namespace GlassLewisChallange.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });

                var response = ApiResponse<object>.FailResponse(errors.Select(s => $"{s.field} : {s.message}").ToList(), "Validation Error.");

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.FailResponse(new List<string>() { "Record not found." }, "Record Error.");

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<object>.FailResponse(new List<string>() { ex.Message }, "System Error.");

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
