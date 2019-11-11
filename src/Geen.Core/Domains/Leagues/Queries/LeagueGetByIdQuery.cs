using System.Threading.Tasks;
using Geen.Core.Domains.Leagues.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Leagues.Queries
{
    public class LeagueGetByIdQuery : IQuery<Task<LeagueModel>>
    {
        public int Id { get; set; }
    }

    public class LeagueGetByIdQueryHandler : IQueryHandler<LeagueGetByIdQuery, Task<LeagueModel>>
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueGetByIdQueryHandler(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public Task<LeagueModel> Execute(LeagueGetByIdQuery query)
        {
            return _leagueRepository.GetById(query.Id);
        }
    }
}
