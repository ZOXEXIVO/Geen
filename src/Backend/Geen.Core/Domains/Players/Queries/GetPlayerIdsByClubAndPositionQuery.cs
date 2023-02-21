using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Players.Queries;

public record GetPlayerIdsByClubAndPositionQuery : IQuery<Task<List<int>>>
{
    public string ClubUrlName { get; set; }
    public int Position { get; set; }
}

public class
    GetPlayerIdsByClubAndPositionQueryHandler : IQueryHandler<GetPlayerIdsByClubAndPositionQuery, Task<List<int>>>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerIdsByClubAndPositionQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<List<int>> Execute(GetPlayerIdsByClubAndPositionQuery query)
    {
        return _playerRepository.GetIdsByClubAndPosition(query.ClubUrlName, query.Position);
    }
}