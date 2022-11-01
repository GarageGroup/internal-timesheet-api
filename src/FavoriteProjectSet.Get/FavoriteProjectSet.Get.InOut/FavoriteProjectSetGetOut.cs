using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class FavoriteProjectSetGetOut
{
    public FavoriteProjectSetGetOut([AllowNull] FlatArray<FavoriteProjectItemGetOut> projects)
        =>
        Projects = projects ?? FlatArray.Empty<FavoriteProjectItemGetOut>();

    public FlatArray<FavoriteProjectItemGetOut> Projects { get; }
}