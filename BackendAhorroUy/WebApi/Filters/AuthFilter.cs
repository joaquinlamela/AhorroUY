using BusinessLogic.Interface;
using DomainException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics.CodeAnalysis;
using WebApi.DataTypes;

namespace WebApi.Filters
{
    [ExcludeFromCodeCoverage]
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        private IUserSessionManagement userSessions;

        public AuthFilter() { }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Auth"];
            if (token == null)
            {
                GenerateError("ERR_AUTH_MISSING", ref context);
                return;
            }
            userSessions = GetSessionsInstance(context);
            Guid tokenGuid;
            try
            {
                tokenGuid = Guid.Parse(token);
            }
            catch
            {
                GenerateError("ERR_AUTH_INVALID", ref context);
                return;
            }
            if (userSessions.IsLogged(tokenGuid) == null)
            {
                Console.WriteLine("filtro");
                GenerateError("ERR_AUTH_INCORRECT", ref context);
                return;
            }
        }

        private void GenerateError(string errorCode, ref AuthorizationFilterContext context)
        {
            var error = new ErrorDT(new UserException(errorCode));

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = error.Status;
            response.ContentType = "application/json";
            context.Result = new ObjectResult(error);
        }

        private static IUserSessionManagement GetSessionsInstance(AuthorizationFilterContext context)
        {
            return (IUserSessionManagement)context.HttpContext.RequestServices.GetService(typeof(IUserSessionManagement));
        }

    }
}