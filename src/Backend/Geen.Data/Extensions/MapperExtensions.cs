using Mapster;

namespace Geen.Data.Extensions
{
    public static class MapperExtensions
    {
        public static T Map<T>(this object obj)
        {
            return obj.Adapt<T>();
        }
    }
}
