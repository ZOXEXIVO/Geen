using System;
using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Core.Domains.Users;
using Geen.Core.Interfaces.Common;
using Geen.Core.Models.Content.Extensions;
using Geen.Core.Models.Replies;
using Geen.Core.Services.Text;

namespace Geen.Core.Domains.Replies.Commands
{
    public class ReplyCreateCommand : ICommand<Task<ReplyModel>>
    {
        public ReplyCreateModel Model { get; set; }
        public UserModel User { get; set; }
    }

    public class ReplyCreateCommandDispatcher : ICommandDispatcher<ReplyCreateCommand, Task<ReplyModel>>
    {
        private readonly IReplyRepository _replyRepository;
        private readonly IMentionRepository _mentionRepository;

        private readonly ITextService _textService;

        private readonly IContentService _contentService;

        public ReplyCreateCommandDispatcher(IContentService contentService, 
            IMentionRepository mentionRepository, 
            IReplyRepository replyRepository, ITextService textService)
        {
            _contentService = contentService;
            _replyRepository = replyRepository;
            _textService = textService;
            _mentionRepository = mentionRepository;
            _replyRepository = replyRepository;
        }

        public async Task<ReplyModel> Execute(ReplyCreateCommand command)
        {
            var mention = await _mentionRepository.GetById(command.Model.MentionId);
            
            if (mention == null)
                throw new ArgumentException(nameof(ReplyCreateModel));

            var contentProcessingResult = _contentService.Process(command.Model.Text, mention.ToContentContext());

            var reply = new ReplyModel
            {
                MentionId = command.Model.MentionId,
                Text = contentProcessingResult.Text,
                User = command.User,
                Date = DateTime.UtcNow,
                IsApproved =  _textService.CanApproveMention(command.Model.Text)
            };

            await _replyRepository.Save(reply);

            if (reply.IsApproved)
                await _mentionRepository.IncremenRepliesCount(reply.MentionId);
               
            return reply;
        }
    }
}
