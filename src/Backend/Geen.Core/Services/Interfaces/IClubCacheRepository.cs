using Geen.Core.Models.Content;
using Geen.Core.Models.Mentions;

namespace Geen.Core.Services.Interfaces
{
    public interface IClubCacheRepository
    {
        EntityInfo GetClubUrl(string word, ContentContext context);
    }
}
