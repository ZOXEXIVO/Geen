using Geen.Core.Domains.Clubs;
using Geen.Core.Domains.Players;

namespace Geen.Core.Models.Content;

public class ContentContext
{
    public ClubModel Club { get; set; }
    public PlayerModel Player { get; set; }
}