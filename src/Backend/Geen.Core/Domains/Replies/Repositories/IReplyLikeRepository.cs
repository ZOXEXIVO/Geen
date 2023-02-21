using System.Threading.Tasks;
using Geen.Core.Models.Likes;

namespace Geen.Core.Domains.Replies.Repositories;

public interface IReplyLikeRepository
{
    Task<LikeStatus> GetLikeStatus(string id, string userId);

    Task LikeAddUser(string id, string userId);
    Task LikeRemoveUser(string id, string userId);

    Task DislikeAddUser(string id, string userId);
    Task DislikeRemoveUser(string id, string userId);
}