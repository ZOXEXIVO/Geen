using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Commands;

public record MentionRemoveCommand : ICommand<Task>
{
    public long Id { get; set; }
}

public class MentionRemoveCommandDispatcher : ICommandDispatcher<MentionRemoveCommand, Task>
{
    private readonly IMentionRepository _mentionRepository;

    public MentionRemoveCommandDispatcher(IMentionRepository mentionRepository)
    {
        _mentionRepository = mentionRepository;
    }

    public Task Execute(MentionRemoveCommand command)
    {
        return _mentionRepository.Delete(command.Id);
    }
}