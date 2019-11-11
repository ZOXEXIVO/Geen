using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries
{
    public class GetTopPlayerQuery : IQuery<Task<List<PlayerModel>>>
    {
        public string ClubUrlName { get; set; }
    }

    public class GetTopPlayerQueryHandler : IQueryHandler<GetTopPlayerQuery, Task<List<PlayerModel>>>
    {
        private readonly IPlayerRepository _playerRepository;

        public GetTopPlayerQueryHandler(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public Task<List<PlayerModel>> Execute(GetTopPlayerQuery query)
        {
            return _playerRepository.GetTopPlayers(query.ClubUrlName);
        }
    }
}
