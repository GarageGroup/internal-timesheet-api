using System.Collections.Generic;
using System.Linq;

namespace GarageGroup.Internal.Timesheet;

internal static class LinqExtensions
{
    internal static IEnumerable<T> TakeTop<T>(this IEnumerable<T> source, int? count)
        =>
        count is not null ? source.Take(count.Value) : source;

    internal static IEnumerable<T> NotNull<T>(this IEnumerable<T?> source)
        where T : class
    {
        foreach (var item in source)
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }
}