using System;
using Geen.Data.Settings;
using Geen.Web.Application.Authentication.Model;

namespace Geen.Web.Application.Authentication.Extensions;

public static class LoginModelExtensions
{
    public static bool IsSuccessAdminLogin(this LoginModel model, GeenSettings settings)
    {
        return string.Compare(settings.Authentication.Login, model.Login, StringComparison.OrdinalIgnoreCase) == 0
               && string.Compare(settings.Authentication.Password, model.Password,
                   StringComparison.OrdinalIgnoreCase) == 0;
    }
}