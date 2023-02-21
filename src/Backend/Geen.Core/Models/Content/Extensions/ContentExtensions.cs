using Geen.Core.Domains.Mentions;

namespace Geen.Core.Models.Content.Extensions;

public static class ContentExtensions
{
    public static ContentContext ToContentContext(this MentionModel obj)
    {
        return new ContentContext
        {
            Club = obj.Club,
            Player = obj.Player
        };
    }
}