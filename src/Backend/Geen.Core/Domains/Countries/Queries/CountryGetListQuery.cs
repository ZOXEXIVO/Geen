using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Countries.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Countries.Queries;

public record CountryGetListQuery : IQuery<Task<List<CountryModel>>>
{
}

public class CountryGetListQueryHandler : IQueryHandler<CountryGetListQuery, Task<List<CountryModel>>>
{
    private readonly ICountryRepository _countryRepository;

    public CountryGetListQueryHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public Task<List<CountryModel>> Execute(CountryGetListQuery query)
    {
        return _countryRepository.GetAll();
    }
}