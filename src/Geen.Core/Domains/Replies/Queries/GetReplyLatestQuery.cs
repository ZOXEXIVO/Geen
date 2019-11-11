using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Replies.Queries
{
    public class GetReplyLatestQuery : IQuery<Task<List<ReplyModel>>>
    {
        public long? ClubId { get; set; }
        public int? PlayerId { get; set; }
        
        public int Count { get; set; }
    }

    public class GetReplyLatestQueryHandler : IQueryHandler<GetReplyLatestQuery, Task<List<ReplyModel>>>
    {
        private readonly IReplyRepository _replyRepository;

        public GetReplyLatestQueryHandler(IReplyRepository replyRepository)
        {
            _replyRepository = replyRepository;
        }

        public Task<List<ReplyModel>> Execute(GetReplyLatestQuery query)
        {
            return _replyRepository.GetLatestList(query);
        }
    }
}
    