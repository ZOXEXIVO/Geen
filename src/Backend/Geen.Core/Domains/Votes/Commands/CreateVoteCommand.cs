using System;
using System.Threading.Tasks;
using Geen.Core.Domains.Votes.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Votes.Commands;

public record CreateVoteCommand : ICommand<Task>
{
    public int? LeftPlayerId { get; set; }
    public int? RightPlayerId { get; set; }

    public int? WinnerId { get; set; }
}

public class CreateVoteCommandDispatcher : ICommandDispatcher<CreateVoteCommand, Task>
{
    private readonly IVoteRepository _voteRepository;

    public CreateVoteCommandDispatcher(IVoteRepository voteRepository)
    {
        _voteRepository = voteRepository;
    }

    public Task Execute(CreateVoteCommand command)
    {
        var model = new VoteModel
        {
            LeftPlayerId = command.LeftPlayerId,
            RightPlayerId = command.RightPlayerId,
            WinnerId = command.WinnerId,
            Date = DateTime.UtcNow
        };

        return _voteRepository.Create(model);
    }
}