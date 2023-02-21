using System;
using System.Text;
using Geen.Web.Application.Authentication.Model;

namespace Geen.Web.Application.Authentication.Services;

public class AuthenticationService
{
    public string CreateToken(LoginModel model)
    {
        var data = (model.Login + model.Password).Trim().ToLower();
        return Convert.ToBase64String(Encoding.ASCII.GetBytes(data));
    }
}