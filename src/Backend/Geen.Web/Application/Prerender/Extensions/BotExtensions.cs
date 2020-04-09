namespace Geen.Web.Application.Prerender.Extensions
{
    public static class BotExtensions
    {
        public static bool IsGoogleBot(this string userAgent)
        {
            userAgent = userAgent?.ToLower() ?? "";

            return userAgent.Contains("google");
        }

        public static bool IsYandexBot(this string userAgent)
        {
            userAgent = userAgent?.ToLower() ?? "";

            return userAgent.Contains("yandex");
        }
    }
}
