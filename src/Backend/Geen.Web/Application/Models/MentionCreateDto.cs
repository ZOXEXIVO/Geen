using Geen.Core.Models.Mentions;

namespace Geen.Web.Application.Models;

public class MentionCreateDto
{
    public string Text { get; set; }

    public int? ClubId { get; set; }
    public int? PlayerId { get; set; }

    public MentionCreateModel ToModel()
    {
        return new MentionCreateModel
        {
            Text = Text,
            ClubId = ClubId,
            PlayerId = PlayerId
        };
    }
}