namespace Geen.Core.Models.Mentions;

public class MentionCreateModel
{
    public string Text { get; set; }

    public int? PlayerId { get; set; }
    public int? ClubId { get; set; }
}