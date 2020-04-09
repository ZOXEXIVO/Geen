using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries
{
    public class PlayerGetListQuery : IQuery<Task<List<PlayerModel>>>
    {
        public string Query{ get;set; }

        public string ClubUrlName { get; set; }

        public int Page { get; set; }
    }

    public class PlayerGetListQueryHandler : IQueryHandler<PlayerGetListQuery, Task<List<PlayerModel>>>
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerGetListQueryHandler(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public Task<List<PlayerModel>> Execute(PlayerGetListQuery query)
        {
            return _playerRepository.GetList(query.Query, query.ClubUrlName, query.Page);
        }
    }
}
