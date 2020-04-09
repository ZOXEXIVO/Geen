using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Commands
{
    public class MentionDisapproveCommand : ICommand<Task>
    {
        public long Id { get; set; }
    }

    public class MentionDisapproveCommandDispatcher : ICommandDispatcher<MentionDisapproveCommand, Task>
    {
        private readonly IMentionRepository _mentionRepository;

        public MentionDisapproveCommandDispatcher(IMentionRepository mentionRepository)
        {
            _mentionRepository = mentionRepository;
        }

        public Task Execute(MentionDisapproveCommand command)
        {
            return _mentionRepository.Disapprove(command.Id);
        }
    }
}
