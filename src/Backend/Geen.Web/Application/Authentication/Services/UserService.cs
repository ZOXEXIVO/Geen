using System;
using Geen.Core.Domains.Users;
using Geen.Web.Application.Constants;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace Geen.Web.Application.Authentication.Services;

public class UserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public UserModel GetCurrentUser()
    {
        var model = new UserModel
        {
            Id = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(
                CookieConstants.AuthCookieName, out var anonymousId)
                ? anonymousId
                : UpdateUser()
        };

        var userName = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(
            CookieConstants.UserNameCookieName, out var cookieUserName)
            ? cookieUserName
            : null;

        if (string.IsNullOrWhiteSpace(userName))
        {
            model.IsAnonymous = true;
            model.Name = null;
        }
        else
        {
            model.IsAnonymous = false;
            model.Name = userName;
        }

        return model;
    }

    private string UpdateUser()
    {
        var anonymousId = ObjectId.GenerateNewId().ToString();

        var cookiesContainer = _contextAccessor.HttpContext.Response.Cookies;

        cookiesContainer.Append(CookieConstants.AuthCookieName, anonymousId,
            new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(5),
                Secure = true
            });

        return anonymousId;
    }
}