using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Entities;
using UniAdmissionPlatform.BusinessTier.Responses;
using UniAdmissionPlatform.BusinessTier.Services;

namespace UniAdmissionPlatform.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claims = context.HttpContext.Items["claims"];
            if (claims == null)
            {
                context.Result = new JsonResult(MyResponse<dynamic>.FailWithMessage("Bạn chưa đăng nhập")) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }


    [AttributeUsage(AttributeTargets.Method)]
    public class CasbinAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claims = (CustomClaims)context.HttpContext.Items["claims"];
            if (claims == null)
            {
                context.Result = new JsonResult(MyResponse<dynamic>.FailWithMessage("Bạn chưa dăng nhập")) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var casbinService = context.HttpContext.RequestServices.GetService<ICasbinService>();
            
            var sub = claims.Role;
            var obj = context.HttpContext.Request.Path;
            var act = context.HttpContext.Request.Method;

            if (!casbinService!.Enforce(sub, obj, act))
            {
                context.Result = new JsonResult(MyResponse<dynamic>.FailWithMessage("Bạn không có quyền"))
                    { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
    }
}