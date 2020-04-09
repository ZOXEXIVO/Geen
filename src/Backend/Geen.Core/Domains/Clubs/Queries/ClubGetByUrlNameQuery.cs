using System.Threading.Tasks;
using Geen.Core.Domains.Clubs.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Clubs.Queries
{
    public class ClubGetByUrlNameQuery : IQuery<Task<ClubModel>>
    {
        public string UrlName { get; set; }
    }

    public class ClubGetByUrlNameQueryHandler : IQueryHandler<ClubGetByUrlNameQuery, Task<ClubModel>>
    {
        private readonly IClubRepository _clubRepository;

        public ClubGetByUrlNameQueryHandler(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public Task<ClubModel> Execute(ClubGetByUrlNameQuery query)
        {
            return _clubRepository.GetByUrlName(query.UrlName);
        }
    }
}
