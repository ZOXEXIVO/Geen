using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Replies.Queries;

public record GetReplyListQuery : IQuery<Task<List<ReplyModel>>>
{
    public long MentionId { get; set; }

    public int Page { get; set; }
}

public class GetReplyListQueryHandler : IQueryHandler<GetReplyListQuery, Task<List<ReplyModel>>>
{
    private readonly IReplyRepository _replyRepository;

    public GetReplyListQueryHandler(IReplyRepository replyRepository)
    {
        _replyRepository = replyRepository;
    }

    public Task<List<ReplyModel>> Execute(GetReplyListQuery query)
    {
        return _replyRepository.GetList(query);
    }
}