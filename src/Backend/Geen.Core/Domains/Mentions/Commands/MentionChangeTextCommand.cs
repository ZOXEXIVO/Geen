using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Commands
{
    public class MentionChangeTextCommand : ICommand<Task>
    {
        public long Id { get; set; }
        public string Text { get; set; }
    }

    public class MentionChangeTextCommandDispatcher : ICommandDispatcher<MentionChangeTextCommand, Task>
    {
        private readonly IMentionRepository _mentionRepository;

        public MentionChangeTextCommandDispatcher(IMentionRepository mentionRepository)
        {
            _mentionRepository = mentionRepository;
        }

        public Task Execute(MentionChangeTextCommand command)
        {
            return _mentionRepository.UpdateText(command.Id, command.Text);
        }
    }
}
