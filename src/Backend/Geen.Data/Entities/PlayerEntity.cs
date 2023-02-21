using System;
using Geen.Data.Entities.Attributes;
using Geen.Data.Entities.Namespaces;

namespace Geen.Data.Entities;

[MongoEntity("Players", MongoNamespaces.Global)]
public class PlayerEntity
{
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string UrlName { get; set; }

    public DateTime BirthDate { get; set; }

    public int Position { get; set; }

    public ClubEntity Club { get; set; }

    public long Votes { get; set; }

    public long MentionsCount { get; set; }
}