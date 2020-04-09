using Geen.Data.Settings;
using Geen.Web.Application.Authentication.Extensions;
using Geen.Web.Application.Authentication.Model;
using Geen.Web.Application.Filters.Throttling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using Geen.Web.Application.Constants;
using Microsoft.AspNetCore.Http;
using AuthenticationService = Geen.Web.Application.Authentication.Services.AuthenticationService;

namespace Geen.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationService _authenticationService;
        private readonly GeenSettings _geenSettings;

        public AuthenticationController(IOptions<GeenSettings> geenSettings, AuthenticationService authenticationService)
        {
            _geenSettings = geenSettings.Value;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [ThrottleFilter(0, 0, 5, 0)]
        [Route("/api/authentication/login")]
        public string Login([FromBody]LoginModel model)
        {
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
}
