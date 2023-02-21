using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries;

public record PlayerNextIdQuery : IQuery<Task<long>>
{
}

public class PlayerModelNextIdQueryHandler : IQueryHandler<PlayerNextIdQuery, Task<long>>
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerModelNextIdQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<long> Execute(PlayerNextIdQuery query)
    {
        return _playerRepository.GetNextId();
    }
}