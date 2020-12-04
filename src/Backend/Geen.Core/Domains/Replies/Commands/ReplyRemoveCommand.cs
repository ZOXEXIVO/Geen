using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Replies.Commands
{
    public class ReplyRemoveCommand : ICommand<Task>
    {
        public string ReplyId { get; set; }
    }

    public class ReplyRemoveCommandDispatcher : ICommandDispatcher<ReplyRemoveCommand, Task>
    {
        private readonly IReplyRepository _replyRepository;
        private readonly IMentionRepository _mentionRepository;
        
        public ReplyRemoveCommandDispatcher(IReplyRepository replyRepository, IMentionRepository mentionRepository)
        {
            _replyRepository = replyRepository;
            _mentionRepository = mentionRepository;
        }

        public async Task Execute(ReplyRemoveCommand command)
        {         
            //TODO Transactioned sessions
            
            var replyModel = await _replyRepository.GetById(command.ReplyId);
 
            await _replyRepository.Delete(command.ReplyId);

            await _mentionRepository.DecrementRepliesCount(replyModel.MentionId);
        }
    }
}
