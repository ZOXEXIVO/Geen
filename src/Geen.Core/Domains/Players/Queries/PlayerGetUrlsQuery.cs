using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries
{
    public class PlayerGetUrlsQuery : IQuery<Task<List<string>>>
    {
    }

    public class PlayerGetUrlsQueryHandler : IQueryHandler<PlayerGetUrlsQuery, Task<List<string>>>
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerGetUrlsQueryHandler(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public Task<List<string>> Execute(PlayerGetUrlsQuery query)
        {
            return _playerRepository.GetUrls();
        }
    }
}
