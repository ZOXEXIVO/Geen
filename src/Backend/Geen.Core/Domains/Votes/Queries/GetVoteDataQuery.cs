using System.Threading.Tasks;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Domains.Players.Utils;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Votes.Queries;

public record GetVoteDataQuery : IQuery<Task<VoteFullModel>>
{
}

public class GetVoteDataQueryHandler : IQueryHandler<GetVoteDataQuery, Task<VoteFullModel>>
{
    private readonly IPlayerRepository _playerRepository;

    public GetVoteDataQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<VoteFullModel> Execute(GetVoteDataQuery query)
    {
        var position = Randomizer.RandomLocal.Next(0, 5);

        var votesPlayers = await _playerRepository.GetForVotes(position);

        return new VoteFullModel
        {
            Left = votesPlayers.Left,
            Right = votesPlayers.Right
        };
    }
}