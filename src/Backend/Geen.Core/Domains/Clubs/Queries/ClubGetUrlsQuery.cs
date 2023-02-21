using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Clubs.Queries;

public class ClubGetUrlsQuery : IQuery<Task<List<ClubModel>>>
{
}

public class ClubGetUrlsQueryHandler : IQueryHandler<ClubGetUrlsQuery, Task<List<ClubModel>>>
{
    private readonly IClubRepository _clubRepository;

    public ClubGetUrlsQueryHandler(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }

    public Task<List<ClubModel>> Execute(ClubGetUrlsQuery query)
    {
        return _clubRepository.GetAllUrls();
    }
}