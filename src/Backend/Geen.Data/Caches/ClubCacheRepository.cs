using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs;
using Geen.Core.Domains.Clubs.Queries;
using Geen.Core.Interfaces.Common;
using Geen.Core.Models.Content;
using Geen.Core.Models.Mentions;
using Geen.Core.Services.Interfaces;

namespace Geen.Data.Caches;

public class ClubCacheRepository : IClubCacheRepository
{
    private readonly Lazy<Task<ConcurrentDictionary<string, ClubModel>>> _cache;

    public ClubCacheRepository(IQueryDispatcher queryDispatcher)
    {
        _cache = new Lazy<Task<ConcurrentDictionary<string, ClubModel>>>(async ()
            => await queryDispatcher.Execute(new ClubsForCacheQuery()));
    }

    public EntityInfo GetClubUrl(string word, ContentContext context)
    {
        var clubKeys = _cache.Value.Result.Keys.Where(word.StartsWith).ToList();

        if (clubKeys.Count != 1)
            return EntityInfo.Empty;

        var club = _cache.Value.Result[clubKeys.FirstOrDefault()];

        if (club.Id == context.Club?.Id)
            return EntityInfo.Empty;

        return new EntityInfo
        {
            Id = club.Id,
            Url = club.UrlName
        };
    }
}