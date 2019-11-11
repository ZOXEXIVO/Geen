using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Clubs.Queries
{
    public class ClubGetListQuery : IQuery<Task<List<ClubModel>>>
    {
    }

    public class ClubGetListQueryHandler : IQueryHandler<ClubGetListQuery, Task<List<ClubModel>>>
    {
        private readonly IClubRepository _clubRepository;

        public ClubGetListQueryHandler(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public Task<List<ClubModel>> Execute(ClubGetListQuery query)
        {
            return _clubRepository.GetAll();
        }
    }
}
