using System;

namespace Geen.Core.Utils
{
    public static class PagingExtensions
    {
        private const int PageSize = 30;

        public static int GetTotalPages(long items)
        {
            return (int)Math.Ceiling((double)items / PageSize);
        }
    }
}
