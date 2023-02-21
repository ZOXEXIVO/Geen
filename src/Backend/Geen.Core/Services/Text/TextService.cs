using System.Collections.Generic;
using System.Linq;

namespace Geen.Core.Services.Text;

public interface ITextService
{
    bool CanApproveMention(string text);
}

public class TextService : ITextService
{
    private readonly List<string> _domains;

    public TextService()
    {
        _domains = new List<string>
        {
            ".ru",
            ".com",
            ".net",
            ".online"
        };
    }

    public bool CanApproveMention(string text)
    {
        if (text.Contains("http"))
            return false;

        if (text.Contains("//"))
            return false;

        if (text.Length < 100)
            return false;

        if (_domains.Any(text.Contains))
            return false;

        return true;
    }
}