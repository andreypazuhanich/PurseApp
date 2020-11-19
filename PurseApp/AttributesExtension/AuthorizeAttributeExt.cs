using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PurseApp.AttributesExtension
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,AllowMultiple = false, Inherited = true)]
    public class AuthorizeExtAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"];
            if(user == null)
                context.Result = new JsonResult(new {message = "Unathorized"}){ StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}