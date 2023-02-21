using System;
using System.Threading.Tasks;
using Geen.Data.Settings;
using Geen.Web.Application.Authentication.Model;
using Geen.Web.Application.Authentication.Services;
using Geen.Web.Application.Constants;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Geen.Web.Application.Authentication.Filters;

public class AuthenticationFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var token = context.HttpContext.Request.Cookies[CookieConstants.AdministratorCookieName];

        if (string.IsNullOrWhiteSpace(token))
        {
            context.Result = null;
            context.HttpContext.Response.StatusCode = 401;
            return;
        }

        var geenSettings = context.HttpContext.RequestServices.GetService<IOptions<GeenSettings>>();
        var authenticationService = context.HttpContext.RequestServices.GetService<AuthenticationService>();

        var currentToken = authenticationService.CreateToken(new LoginModel
        {
            Login = geenSettings.Value.Authentication.Login,
            Password = geenSettings.Value.Authentication.Password
        });

        if (string.Compare(token, currentToken, StringComparison.Ordinal) != 0)
        {
            context.Result = null;
            context.HttpContext.Response.StatusCode = 401;
            return;
        }

        await next();
    }
}