using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Geen.Data.Utils
{
    public static class PagingExtensions
    {
        private const int PageSize = 30;

        public static async Task<List<T>> ToPagedAsync<T>(this IFindFluent<T, T> items, int page)
        {
            if (page < 1)
                page = 1;

            var skip = (page - 1) * PageSize;

            return await items.Skip(skip).Limit(PageSize).ToListAsync();
        }
    }
}
