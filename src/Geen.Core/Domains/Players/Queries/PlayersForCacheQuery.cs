using System.Collections.Concurrent;
using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries
{
    public class PlayerForCacheQuery : IQuery<Task<ConcurrentBag<PlayerModel>>>
    {
        public string ClubUrlName { get; set; }
    }

    public class PlayerForCacheQueryHandler : IQueryHandler<PlayerForCacheQuery, Task<ConcurrentBag<PlayerModel>>>
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerForCacheQueryHandler(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<ConcurrentBag<PlayerModel>> Execute(PlayerForCacheQuery query)
        {
            return new ConcurrentBag<PlayerModel>(
                await _playerRepository.GetCached());
        }
    }
}
