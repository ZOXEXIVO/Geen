using Geen.Core.Models.Content;
using Geen.Core.Models.Mentions;

namespace Geen.Core.Services.Interfaces
{
    public interface IPlayerCacheRepository
    {
        EntityInfo GetPlayerUrl(string word, ContentContext context);
    }
}
