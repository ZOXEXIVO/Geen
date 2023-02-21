namespace Geen.Core.Domains.Countries;

public record CountryModel
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string UrlName { get; set; }
}