using System.Threading.Tasks;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Replies.Commands;

public record ReplyDisapproveCommand : ICommand<Task>
{
    public string Id { get; set; }
}

public class ReplyDisapproveCommandDispatcher : ICommandDispatcher<ReplyDisapproveCommand, Task>
{
    private readonly IReplyRepository _replyRepository;

    public ReplyDisapproveCommandDispatcher(IReplyRepository replyRepository)
    {
        _replyRepository = replyRepository;
    }

    public Task Execute(ReplyDisapproveCommand command)
    {
        return _replyRepository.Disapprove(command.Id);
    }
}