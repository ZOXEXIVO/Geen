using System.Threading.Tasks;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Replies.Commands;

public record ReplyChangeTextCommand : ICommand<Task>
{
    public string Id { get; set; }
    public string Text { get; set; }
}

public class ReplyChangeTextCommandDispatcher : ICommandDispatcher<ReplyChangeTextCommand, Task>
{
    private readonly IReplyRepository _replyRepository;

    public ReplyChangeTextCommandDispatcher(IReplyRepository replyRepository)
    {
        _replyRepository = replyRepository;
    }

    public Task Execute(ReplyChangeTextCommand command)
    {
        return _replyRepository.UpdateText(command.Id, command.Text);
    }
}