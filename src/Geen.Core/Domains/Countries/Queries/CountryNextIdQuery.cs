using System.Threading.Tasks;
using Geen.Core.Domains.Countries.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Countries.Queries
{
    public class CountryNextIdQuery : IQuery<Task<long>>
    {
    }

    public class CountryNextIdQueryHandler : IQueryHandler<CountryNextIdQuery, Task<long>>
    {
        private readonly ICountryRepository _countryRepository;

        public CountryNextIdQueryHandler(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public Task<long> Execute(CountryNextIdQuery query)
        {
            return _countryRepository.GetNextId();
        }
    }
}
