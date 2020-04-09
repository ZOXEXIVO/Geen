using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Replies.Queries;

namespace Geen.Core.Domains.Replies.Repositories
{
    public interface IReplyRepository
    {
        Task<ReplyModel> GetById(string id);

        Task<List<ReplyModel>> GetList(GetReplyListQuery query);
        Task<List<ReplyModel>> GetLatestList(GetReplyLatestQuery query);
        Task<List<ReplyModel>> GetUnapprovedList(GetReplyUnapprovedListQuery query);

        Task Approve(string id);
        Task Disapprove(string id);

        Task UpdateText(string id, string text);

        Task Save(ReplyModel model);

        Task Delete(string id);
        
        Task<List<ReplyModel>> GetAll(int count);

        Task UpdateUser(string id, string userName);
    }
}
