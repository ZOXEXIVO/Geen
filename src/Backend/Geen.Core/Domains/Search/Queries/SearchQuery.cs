using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Search.Queries;

public record SearchQuery : IQuery<Task<List<PlayerModel>>>
{
    public string Query { get; set; }
    public int Count { get; set; }
}

public class SearchQueryHandler : IQueryHandler<SearchQuery, Task<List<PlayerModel>>>
{
    private readonly IPlayerRepository _playerRepository;

    public SearchQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<List<PlayerModel>> Execute(SearchQuery query)
    {
        return _playerRepository.Search(query.Query, query.Count);
    }
}