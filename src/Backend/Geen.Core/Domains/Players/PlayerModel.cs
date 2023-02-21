using System;
using Geen.Core.Domains.Clubs;

namespace Geen.Core.Domains.Players;

public class PlayerModel
{
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string UrlName { get; set; }

    public DateTime BirthDate { get; set; }

    public int Position { get; set; }

    public ClubModel Club { get; set; }

    public long Votes { get; set; }

    public long MentionsCount { get; set; }
}