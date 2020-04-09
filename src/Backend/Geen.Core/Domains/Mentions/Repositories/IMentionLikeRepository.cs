using System.Threading.Tasks;
using Geen.Core.Models.Likes;

namespace Geen.Core.Domains.Mentions.Repositories
{
    public interface IMentionLikeRepository
    {
        Task<LikeStatus> GetLikeStatus(long id, string userId);

        Task LikeAddUser(long id, string userId);
        Task LikeRemoveUser(long id, string userId);

        Task DislikeAddUser(long id, string userId);
        Task DislikeRemoveUser(long id, string userId);
    }
}
