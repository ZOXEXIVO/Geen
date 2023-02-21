using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Commands;

public record MentionApproveCommand : ICommand<Task>
{
    public long Id { get; set; }
}

public class MentionApproveCommandDispatcher : ICommandDispatcher<MentionApproveCommand, Task>
{
    private readonly IMentionRepository _mentionRepository;

    public MentionApproveCommandDispatcher(IMentionRepository mentionRepository)
    {
        _mentionRepository = mentionRepository;
    }

    public Task Execute(MentionApproveCommand command)
    {
        return _mentionRepository.Approve(command.Id);
    }
}