using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries;

public record GetRandomPlayerQuery : IQuery<Task<PlayerModel>>
{
}

public class GetRandomPlayerQueryHandler : IQueryHandler<GetRandomPlayerQuery, Task<PlayerModel>>
{
    private readonly IPlayerRepository _playerRepository;

    public GetRandomPlayerQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<PlayerModel> Execute(GetRandomPlayerQuery query)
    {
        return _playerRepository.GetRandom();
    }
}