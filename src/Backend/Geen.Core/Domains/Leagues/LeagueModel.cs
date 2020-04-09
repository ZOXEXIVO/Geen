namespace Geen.Core.Domains.Leagues
{
    public class LeagueModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string UrlName { get; set; }

        public int CountryId { get; set; }
    }
}
