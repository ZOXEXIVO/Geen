using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Models;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Commands
{
    public class MentionLikeCommand : ICommand<Task>
    {
        public long Id { get; set; }
        public string UserId { get; set; }
    }

    public class MentionLikeCommandDispatcher : ICommandDispatcher<MentionLikeCommand, Task>
    {
        private readonly IMentionLikeRepository _mentionLikeRepository;
        private readonly IMentionRepository _mentionRepository;

        public MentionLikeCommandDispatcher(IMentionLikeRepository mentionLikeRepository, 
            IMentionRepository mentionRepository)
        {
            _mentionLikeRepository = mentionLikeRepository;
            _mentionRepository = mentionRepository;
        }

        public async Task Execute(MentionLikeCommand command)
        {
            var redisResult = await GetRedisResult(command);

            await _mentionRepository.Like(command.Id, redisResult);
        }

        private async Task<IncrementStatus> GetRedisResult(MentionLikeCommand command)
        {
            var likesStatus = await _mentionLikeRepository.GetLikeStatus(command.Id, command.UserId);

            var likesIncrement = 0;
            var dislikesIncrement = 0;

            if (!likesStatus.Likes && !likesStatus.Dislikes)
            {
                await _mentionLikeRepository.LikeAddUser(command.Id, command.UserId);
                likesIncrement = 1;
            }
            else if (likesStatus.Likes && !likesStatus.Dislikes)
            {
                await _mentionLikeRepository.LikeRemoveUser(command.Id, command.UserId);
                likesIncrement = -1;
            }
            else if (!likesStatus.Likes && likesStatus.Dislikes)
            {
                await Task.WhenAll(
                    _mentionLikeRepository.LikeAddUser(command.Id, command.UserId),
                    _mentionLikeRepository.DislikeRemoveUser(command.Id, command.UserId)
                );

                likesIncrement = 1;
                dislikesIncrement = -1;
            }

            return new IncrementStatus
            {
                Likes = likesIncrement,
                Dislikes = dislikesIncrement
            };
        }
    }
}
