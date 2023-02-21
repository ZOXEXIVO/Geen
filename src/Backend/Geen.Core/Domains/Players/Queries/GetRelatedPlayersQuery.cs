using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries;

public record GetRelatedPlayerQuery : IQuery<Task<List<PlayerModel>>>
{
    public string UrlName { get; set; }
}

public class GetRelatedPlayerQueryHandler : IQueryHandler<GetRelatedPlayerQuery, Task<List<PlayerModel>>>
{
    private readonly IPlayerRepository _playerRepository;

    public GetRelatedPlayerQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<List<PlayerModel>> Execute(GetRelatedPlayerQuery query)
    {
        return _playerRepository.GetRelatedPlayers(query.UrlName);
    }
}