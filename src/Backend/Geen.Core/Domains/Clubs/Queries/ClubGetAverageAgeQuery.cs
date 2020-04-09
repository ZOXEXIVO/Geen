using System;
using System.Linq;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Clubs.Queries
{
    public class ClubGetAverageAgeQuery : IQuery<Task<double?>>
    {
        public string UrlName { get; set; }
    }

    public class ClubGetAverageAgeQueryHandler : IQueryHandler<ClubGetAverageAgeQuery, Task<double?>>
    {
        private readonly IClubRepository _clubRepository;

        public ClubGetAverageAgeQueryHandler(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<double?> Execute(ClubGetAverageAgeQuery query)
        {
            var birthdays = await _clubRepository.GetBirthdays(query.UrlName);

            if (!birthdays.Any())
                return null;

            return Math.Round(birthdays.Select(GetAge).Average(), 1);
        }

        private static int GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
