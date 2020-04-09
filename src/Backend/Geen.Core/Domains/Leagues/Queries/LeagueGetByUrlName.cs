using System.Threading.Tasks;
using Geen.Core.Domains.Leagues.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Leagues.Queries
{
    public class LeagueModelGetByUrlName : IQuery<Task<LeagueModel>>
    {
        public string UrlName { get; set; }
    }

    public class LeagueModelGetByUrlNameHandler : IQueryHandler<LeagueModelGetByUrlName, Task<LeagueModel>>
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueModelGetByUrlNameHandler(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public Task<LeagueModel> Execute(LeagueModelGetByUrlName query)
        {
            return _leagueRepository.GetByUrlName(query.UrlName);
        }
    }
}
