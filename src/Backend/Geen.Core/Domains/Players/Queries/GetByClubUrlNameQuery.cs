using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries;

public record PlayerGetByClubUrlNameQuery : IQuery<Task<List<PlayerModel>>>
{
    public string ClubUrlName { get; set; }
}

public class PlayerGetByClubUrlNameQueryHandler : IQueryHandler<PlayerGetByClubUrlNameQuery, Task<List<PlayerModel>>>
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerGetByClubUrlNameQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<List<PlayerModel>> Execute(PlayerGetByClubUrlNameQuery query)
    {
        return _playerRepository.GetByClubUrlName(query.ClubUrlName);
    }
}