using Geen.Data.Entities.Attributes;
using Geen.Data.Entities.Namespaces;

namespace Geen.Data.Entities;

[MongoEntity("Countries", MongoNamespaces.Global)]
public class CountryEntity
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string UrlName { get; set; }
}