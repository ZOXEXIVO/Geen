using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Replies.Queries
{
    public class GetReplyUnapprovedListQuery : IQuery<Task<List<ReplyModel>>>
    {
        public long? MentionId { get; set; }

        public bool? IsApproved { get; set; }

        public int Page { get; set; }
    }

    public class GetReplyUnapprovedListQueryHandler : IQueryHandler<GetReplyUnapprovedListQuery, Task<List<ReplyModel>>>
    {
        private readonly IReplyRepository _replyRepository;

        public GetReplyUnapprovedListQueryHandler(IReplyRepository replyRepository)
        {
            _replyRepository = replyRepository;
        }

        public Task<List<ReplyModel>> Execute(GetReplyUnapprovedListQuery query)
        {
            return _replyRepository.GetUnapprovedList(query);
        }
    }
}
    