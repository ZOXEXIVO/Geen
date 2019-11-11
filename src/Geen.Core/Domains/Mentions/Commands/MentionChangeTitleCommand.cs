using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Commands
{
    public class MentionChangeTitleCommand : ICommand<Task>
    {
        public long Id { get; set; }
        public string Title { get; set; }
    }

    public class MentionChangeTitleCommandDispatcher : ICommandDispatcher<MentionChangeTitleCommand, Task>
    {
        private readonly IMentionRepository _mentionRepository;

        public MentionChangeTitleCommandDispatcher(IMentionRepository mentionRepository)
        {
            _mentionRepository = mentionRepository;
        }

        public Task Execute(MentionChangeTitleCommand command)
        {
            return _mentionRepository.UpdateTitle(command.Id, command.Title);
        }
    }
}
