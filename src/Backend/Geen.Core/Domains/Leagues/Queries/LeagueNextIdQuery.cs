using System.Threading.Tasks;
using Geen.Core.Domains.Leagues.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Leagues.Queries
{
    public class LeagueNextIdQuery : IQuery<Task<long>>
    {
    }

    public class LeagueNextIdQueryHandler : IQueryHandler<LeagueNextIdQuery, Task<long>>
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueNextIdQueryHandler(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public Task<long> Execute(LeagueNextIdQuery query)
        {
            return _leagueRepository.GetNextId();
        }
    }
}
