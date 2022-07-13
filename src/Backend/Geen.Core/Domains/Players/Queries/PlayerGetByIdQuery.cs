using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries;

public class PlayerGetByIdQuery : IQuery<Task<PlayerModel>>
{
    public int Id { get; set; }
}

public class PlayerGetByIdQueryHandler : IQueryHandler<PlayerGetByIdQuery, Task<PlayerModel>>
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerGetByIdQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<PlayerModel> Execute(PlayerGetByIdQuery query)
    {
        return _playerRepository.GetById(query.Id);
    }
}