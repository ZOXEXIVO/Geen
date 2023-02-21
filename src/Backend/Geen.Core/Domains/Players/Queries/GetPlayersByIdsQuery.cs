using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries;

public record GetPlayerModelsByIdsQuery : IQuery<Task<List<PlayerModel>>>
{
    public List<int> PlayerIds { get; set; }
}

public class GetPlayerModelsByIdsQueryHandler : IQueryHandler<GetPlayerModelsByIdsQuery, Task<List<PlayerModel>>>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerModelsByIdsQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<List<PlayerModel>> Execute(GetPlayerModelsByIdsQuery query)
    {
        return _playerRepository.GetByIds(query.PlayerIds);
    }
}