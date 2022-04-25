using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UniAdmissionPlatform.BusinessTier.Commons.Enums;
using UniAdmissionPlatform.BusinessTier.Commons.Utils;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.WebApi.Helpers;

namespace UniAdmissionPlatform.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (GlobalException error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status400BadRequest;
                var result = "";

                switch (error.Code)
                {
                    case ExceptionCode.PrintMessageErrorOut:
                        result = JsonSerializer.Serialize(MyResponse<object>.FailWithMessage(error.Message),
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                            });
                        await response.WriteAsync(result);
                        break;
                    case ExceptionCode.PrintErrorObjectOut:
                        result = JsonSerializer.Serialize(MyResponse<object>.FailWithData(error.ErrorObject),
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                            });
                        await response.WriteAsync(result);
                        break;
                    case ExceptionCode.PrintMessageAndErrorObjectOut:
                        result = JsonSerializer.Serialize(
                            MyResponse<object>.FailWithDetail(error.ErrorObject, error.Message),
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                            });
                        await response.WriteAsync(result);
                        break;
                }
            }
        }
    }
}