using System.Threading.Tasks;
using Geen.Core.Domains.Players;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Clubs.Queries
{
    public class ClubGetCoachQuery : IQuery<Task<PlayerModel>>
    {
        public string ClubUrlName { get; set; }
    }

    public class ClubGetCoachQueryHandler : IQueryHandler<ClubGetCoachQuery, Task<PlayerModel>>
    {
        private readonly IPlayerRepository _playerRepository;

        public ClubGetCoachQueryHandler(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public Task<PlayerModel> Execute(ClubGetCoachQuery query)
        {
            return _playerRepository.GetClubCoach(query.ClubUrlName);
        }
    }
}
