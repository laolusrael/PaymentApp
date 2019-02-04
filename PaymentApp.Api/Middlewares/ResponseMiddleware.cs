using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PaymentApp.Core.Shared.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static PaymentApp.Api.Helpers.RequestHelpers;

namespace PaymentApp.Api.Middlewares
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsSwagger(context))
            {
                await _next(context);
                return;
            }
            
            var actualResponseStream = context.Response.Body;
            try
            {
                using (var responseStream = new MemoryStream())
                {
                    context.Response.Body = responseStream;

                    await _next.Invoke(context);


                    string bodyString = "";

                    switch (context.Response.StatusCode)
                    {

                        case (int)HttpStatusCode.OK:

                            bodyString = await _GetOkResponse(context);
                            // need to write back

                            break;
                        case (int)HttpStatusCode.Unauthorized:
                            bodyString = _GetUnauthorizedResponse(context);
                            break;
                        default:
                            bodyString = _GetDefaultResponse(context);
                            break;
                    }


                    // replace stream,
                    context.Response.Body = new MemoryStream();
                    // write final response to stream
                    await context.Response.WriteAsync(bodyString);

                    // reset stream, so client can read  stream
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    // copy back to the actual stream
                    await context.Response.Body.CopyToAsync(actualResponseStream);

                }
            }
            catch(Exception ex)
            {
                // Replace with original stream if an error occurs
                context.Response.Body = actualResponseStream;

                // rethrow the exception
                throw ex;
            }
        }


        private string _GetDefaultResponse(HttpContext context)
        {
            var resp = new ApiResponse<object>
            {
                Code = context.Response.StatusCode,
                Result = context.Response.StatusCode.ToString(),
                Version = "v1"
            };

            return JsonConvert.SerializeObject(resp);

        }

        private string _GetUnauthorizedResponse(HttpContext context)
        {
            var resp = new ApiResponse<object>
            {
                Code = context.Response.StatusCode,
                Result = context.Response.StatusCode.ToString(),
                Message = "The service you tried to access requires authorization",
                Version = "v1"
            };

            return JsonConvert.SerializeObject(resp);
        }

        private async Task<string> _GetBadResponse(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var bodyString = await reader.ReadToEndAsync();

            var resp = new ApiResponse<object>
            {
                Code = context.Response.StatusCode,
                Result = !string.IsNullOrEmpty(bodyString) ? JsonConvert.DeserializeObject(bodyString) : null,
                Version = "v1"
            };

            bodyString = JsonConvert.SerializeObject(resp);


            return bodyString;
        }
        private async Task<string> _GetOkResponse(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var bodyString = await reader.ReadToEndAsync();

            var resp = new ApiResponse<object>
            {
                Code = context.Response.StatusCode,
                Result = !string.IsNullOrEmpty(bodyString) ? JsonConvert.DeserializeObject(bodyString) : null,
                Version = "v1"
            };

            bodyString = JsonConvert.SerializeObject(resp);


            return bodyString;

        }

    }

    public static class ResponseMiddlewareExtension
    {

        public static IApplicationBuilder UseResponseWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseMiddleware>();
        }
    }
}
