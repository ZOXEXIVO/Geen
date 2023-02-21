using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Commands;

public record MentionSetDefaultAvatarCommand : ICommand<Task>
{
    public long Id { get; set; }
}

public class MentionSetDefaultAvatarCommandDispatcher : ICommandDispatcher<MentionSetDefaultAvatarCommand, Task>
{
    private readonly IMentionRepository _mentionRepository;

    public MentionSetDefaultAvatarCommandDispatcher(IMentionRepository mentionRepository)
    {
        _mentionRepository = mentionRepository;
    }

    public Task Execute(MentionSetDefaultAvatarCommand command)
    {
        return _mentionRepository.SetDefaultAvatar(command.Id);
    }
}