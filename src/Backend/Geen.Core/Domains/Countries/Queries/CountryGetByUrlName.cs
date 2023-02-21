using System.Threading.Tasks;
using Geen.Core.Domains.Countries.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Countries.Queries;

public record CountryGetByUrlName : IQuery<Task<CountryModel>>
{
    public string UrlName { get; set; }
}

public class CountryGetByUrlNameHandler : IQueryHandler<CountryGetByUrlName, Task<CountryModel>>
{
    private readonly ICountryRepository _countryRepository;

    public CountryGetByUrlNameHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public Task<CountryModel> Execute(CountryGetByUrlName query)
    {
        return _countryRepository.GetByUrlName(query.UrlName);
    }
}