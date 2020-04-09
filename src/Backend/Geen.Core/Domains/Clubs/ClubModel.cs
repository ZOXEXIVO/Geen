namespace Geen.Core.Domains.Clubs
{
    public class ClubModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string UrlName { get; set; }

        public string OfficialProfile { get; set; }

        public int LeagueId { get; set; }

        public bool IsNational { get; set; }

        public long Votes { get; set; }
    }
}
