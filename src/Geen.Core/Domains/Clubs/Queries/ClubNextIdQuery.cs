using System.Threading.Tasks;
using Geen.Core.Domains.Clubs.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Clubs.Queries
{
    public class ClubNextIdQuery : IQuery<Task<long>>
    {
    }

    public class ClubNextIdQueryHandler : IQueryHandler<ClubNextIdQuery, Task<long>>
    {
        private readonly IClubRepository _clubRepository;

        public ClubNextIdQueryHandler(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public Task<long> Execute(ClubNextIdQuery query)
        {
            return _clubRepository.GetNextId();
        }
    }
}
