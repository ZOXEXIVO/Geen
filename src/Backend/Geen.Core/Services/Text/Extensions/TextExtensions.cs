using System.Text;
using System.Text.RegularExpressions;

namespace Geen.Core.Services.Text.Extensions
{
    public static class TextExtensions
    {
        private static readonly Regex HtmlCleanRegexp = new Regex("<.*?>", RegexOptions.Compiled);
        
        public static string AsText(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var builder = new StringBuilder(input);

            builder.Replace(" ,", ", ");
            builder.Replace(" .", ". ");

            builder.Replace("  ", " ");

            return HtmlCleanRegexp.Replace(builder.ToString(), string.Empty).Replace("\n", "<br/>");
        }
    }
}
