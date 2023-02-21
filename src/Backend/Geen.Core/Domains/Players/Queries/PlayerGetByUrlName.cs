using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries;

public record PlayerGetByUrlName : IQuery<Task<PlayerModel>>
{
    public string UrlName { get; set; }
}

public class PlayerGetByUrlNameQueryHandler : IQueryHandler<PlayerGetByUrlName, Task<PlayerModel>>
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerGetByUrlNameQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<PlayerModel> Execute(PlayerGetByUrlName query)
    {
        return _playerRepository.GetByUrlName(query.UrlName);
    }
}