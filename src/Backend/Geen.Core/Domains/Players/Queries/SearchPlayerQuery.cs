using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries;

public record SearchPlayerQuery : IQuery<Task<List<PlayerModel>>>
{
    public string Query { get; set; }
}

public class SearchPlayerQueryHandler : IQueryHandler<SearchPlayerQuery, Task<List<PlayerModel>>>
{
    private readonly IPlayerRepository _playerRepository;

    public SearchPlayerQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<List<PlayerModel>> Execute(SearchPlayerQuery query)
    {
        return _playerRepository.Search(query.Query, 5);
    }
}