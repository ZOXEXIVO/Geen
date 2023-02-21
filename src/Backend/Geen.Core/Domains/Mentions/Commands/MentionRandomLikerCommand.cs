using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Commands;

public record MentionRandomLikerCommand : ICommand<Task>
{
    public int Count { get; set; }

    public int MaxLikes { get; set; }
    public int MaxDislikes { get; set; }

    public int DaysInteval { get; set; }
}

public class MentionRandomLikerCommandDispatcher : ICommandDispatcher<MentionRandomLikerCommand, Task>
{
    private readonly IMentionRepository _mentionRepository;

    public MentionRandomLikerCommandDispatcher(IMentionRepository mentionRepository)
    {
        _mentionRepository = mentionRepository;
    }

    public Task Execute(MentionRandomLikerCommand command)
    {
        return _mentionRepository.RandomLikes(command);
    }
}