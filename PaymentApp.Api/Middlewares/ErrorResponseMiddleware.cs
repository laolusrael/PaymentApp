using PaymentApp.Core.ComplexTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PaymentApp.Core.Shared.Types;

namespace PaymentApp.Api.Middlewares
{
    public class ErrorResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        public ErrorResponseMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            _next = next;
            _loggerFactory = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(FrameworkException ex)
            {
                var _logger = _GetLogger(context);
                _logger.LogError("An Application Level Error Has Occured", ex);
                await _WriteFrameworkExceptionOutput(context, ex);
            }
            catch (Exception ex)
            {
                var _logger = _GetLogger(context);
                _logger.LogError(ex.Message, ex);
                await _WriteExceptionOutput(context, ex);
            }
        }


        private ILogger _GetLogger(HttpContext context) {
            return _loggerFactory.CreateLogger(context.Request.Path);
        }

        private Task _WriteFrameworkExceptionOutput(HttpContext context, FrameworkException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;


            var _response = JsonConvert.SerializeObject(new ApiResponse<object>
            {
                Code = (int)context.Response.StatusCode,
                Message = ex.Message,
                Result = ex.ResultValue
            });

            return context.Response.WriteAsync(_response);

        }

        private Task _WriteExceptionOutput(HttpContext context, Exception ex)
        {

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


            var _response = JsonConvert.SerializeObject(new ApiResponse<object>
            {
                Code = (int)context.Response.StatusCode,
                Message = $"An internal Server Error has occured // {ex.Message}",
            });

            return context.Response.WriteAsync(_response);
        }
    }

    public static class ErrorResponseMiddlewareExtension
    {
        public static void UseErrorResponse(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorResponseMiddleware>();
        }
    }
}
