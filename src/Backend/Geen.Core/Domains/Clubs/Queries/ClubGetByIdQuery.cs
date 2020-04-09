using System.Threading.Tasks;
using Geen.Core.Domains.Clubs.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Clubs.Queries
{
    public class ClubGetByIdQuery : IQuery<Task<ClubModel>>
    {
        public int Id { get; set; }
    }

    public class ClubGetByIdQueryHandler : IQueryHandler<ClubGetByIdQuery, Task<ClubModel>>
    {
        private readonly IClubRepository _clubRepository;

        public ClubGetByIdQueryHandler(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public Task<ClubModel> Execute(ClubGetByIdQuery query)
        {
            return _clubRepository.GetById(query.Id);
        }
    }
}
