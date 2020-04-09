using System.Collections.Concurrent;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Clubs.Queries
{
    public class ClubsForCacheQuery : IQuery<Task<ConcurrentDictionary<string, ClubModel>>>
    {
        public string ClubUrlName { get; set; }
    }

    public class ClubsForCacheQueryHandler : IQueryHandler<ClubsForCacheQuery, Task<ConcurrentDictionary<string, ClubModel>>>
    {
        private readonly IClubRepository _clubRepository;

        public ClubsForCacheQueryHandler(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<ConcurrentDictionary<string, ClubModel>> Execute(ClubsForCacheQuery query)
        {
            var items = await _clubRepository.GetCached();

            var result = new ConcurrentDictionary<string, ClubModel>();

            foreach (var club in items)
                result.TryAdd(GetCleanName(club.Name), club);

            return result;
        }

        private static string GetCleanName(string clubName)
        {
            var ind = clubName.IndexOf(' ');

            if (ind == -1)
                return clubName.ToLower();

            return clubName.Substring(0, ind).ToLower();
        }
    }
}
