using Geen.Core.Models.Replies;

namespace Geen.Web.Application.Models
{
    public class ReplyCreateDto
    {
        public long MentionId { get; set; }
        public string Text { get; set; }

        public ReplyCreateModel ToModel()
        {
            return new ReplyCreateModel
            {
                MentionId = MentionId,
                Text = Text
            };
        }
    }
}
