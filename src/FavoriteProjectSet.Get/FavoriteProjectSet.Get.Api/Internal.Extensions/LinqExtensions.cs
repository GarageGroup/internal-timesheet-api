using System.Collections.Generic;
using System.Linq;

namespace GGroupp.Internal.Timesheet;

internal static class LinqExtensions
{
    internal static IEnumerable<T> Top<T>(this IEnumerable<T> source, int? count)
        =>
        count is not null ? source.Take(count.Value) : source;
}