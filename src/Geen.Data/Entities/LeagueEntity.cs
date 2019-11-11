using Geen.Data.Entities.Attributes;
using Geen.Data.Entities.Namespaces;

namespace Geen.Data.Entities
{
    [MongoEntity("Leagues", MongoNamespaces.Global)]
    public class LeagueEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string UrlName { get; set; }

        public int CountryId { get; set; }
    }
}
