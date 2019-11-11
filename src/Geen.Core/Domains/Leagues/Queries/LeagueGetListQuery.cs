using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Leagues.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Leagues.Queries
{
    public class LeagueGetListQuery : IQuery<Task<List<LeagueModel>>>
    {
        public int? CountryId { get; set; }
    }

    public class LeagueGetListQueryHandler : IQueryHandler<LeagueGetListQuery, Task<List<LeagueModel>>>
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueGetListQueryHandler(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public Task<List<LeagueModel>> Execute(LeagueGetListQuery query)
        {
            return _leagueRepository.GetAll();
        }
    }
}
