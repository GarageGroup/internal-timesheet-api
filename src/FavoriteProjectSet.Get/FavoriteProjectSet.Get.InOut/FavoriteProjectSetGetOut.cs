using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class FavoriteProjectSetGetOut
{
    public FavoriteProjectSetGetOut([AllowNull] IReadOnlyCollection<FavoriteProjectItemGetOut> projects)
    {
        Projects = projects ?? Array.Empty<FavoriteProjectItemGetOut>();
    }

    public IReadOnlyCollection<FavoriteProjectItemGetOut> Projects { get; }
}