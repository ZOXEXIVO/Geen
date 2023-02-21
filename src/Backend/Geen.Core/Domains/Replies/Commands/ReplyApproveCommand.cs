using System.Threading.Tasks;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Replies.Commands;

public record ReplyApproveCommand : ICommand<Task>
{
    public string Id { get; set; }
}

public class ReplyApproveCommandDispatcher : ICommandDispatcher<ReplyApproveCommand, Task>
{
    private readonly IReplyRepository _replyRepository;

    public ReplyApproveCommandDispatcher(IReplyRepository replyRepository)
    {
        _replyRepository = replyRepository;
    }

    public Task Execute(ReplyApproveCommand command)
    {
        return _replyRepository.Approve(command.Id);
    }
}