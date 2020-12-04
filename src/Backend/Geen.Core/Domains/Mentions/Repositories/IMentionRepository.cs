using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Commands;
using Geen.Core.Domains.Mentions.Models;
using Geen.Core.Domains.Mentions.Queries;
using Geen.Core.Models.Likes;

namespace Geen.Core.Domains.Mentions.Repositories
{
    public interface IMentionRepository
    {
        Task<MentionModel> GetById(long id);
        Task<List<MentionModel>> GetMentionIdentities();
        Task<List<MentionModel>> GetMentionTitleList(long? id, int page);
        Task<List<MentionModel>> GetList(GetMentionListQuery query);

        Task<List<MentionModel>> GetFreshMentions(DateTime? dateStart);
        Task<List<MentionModel>> GetFreshTitledMentions(DateTime? dateStart);
        Task<List<MentionModel>> GetFreshRepliedMentionIds(DateTime? dateStart);
        long GetClubItemsCount(string clubUrlName);
        long GetPlayerItemsCount(string playerUrlName);

        Task<LikeModel> GetLikeStatus(long id);

        Task Approve(long id);
        Task Disapprove(long id);

        Task Like(long id, IncrementStatus status);
        Task Dislike(long id, IncrementStatus status);

        Task UpdateTitle(long id, string title);
        Task UpdateText(long id, string text);
        Task UpdateUser(long id, string userName);

        Task SetDefaultAvatar(long id);
        
        Task Save(MentionModel model);

        Task Delete(long id);

        Task IncrementRepliesCount(long id);
        Task DecrementRepliesCount(long id);

        #region Temp

        Task RandomLikes(MentionRandomLikerCommand command);
        
        Task<List<MentionModel>> GetAll(int count);

        #endregion
    }
}
