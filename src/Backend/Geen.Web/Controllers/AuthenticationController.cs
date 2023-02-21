using System;
using System.Threading.Tasks;
using Geen.Data.Settings;
using Geen.Web.Application.Authentication.Extensions;
using Geen.Web.Application.Authentication.Model;
using Geen.Web.Application.Authentication.Services;
using Geen.Web.Application.Constants;
using Geen.Web.Application.Filters.Throttling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Geen.Web.Controllers;

public class AuthenticationController : Controller
{
    private readonly AuthenticationService _authenticationService;
    private readonly GeenSettings _geenSettings;

    public AuthenticationController(IOptions<GeenSettings> geenSettings, AuthenticationService authenticationService)
    {
        _geenSettings = geenSettings.Value;
        _authenticationService = authenticationService;
    }

    [HttpPost("/api/authentication/login")]
    [ThrottleFilter(0, 0, 5, 0)]
    public async Task<string> Login([FromBody] LoginModel model)
    {
        await Task.Delay(1000);

        if (model.IsSuccessAdminLogin(_geenSettings))
        {
            var token = _authenticationService.CreateToken(model);

            Response.Cookies.Append(CookieConstants.AdministratorCookieName, token, new CookieOptions
            {
                Expires = DateTime.Now.AddYears(5)
            });
        }

        return null;
    }
}