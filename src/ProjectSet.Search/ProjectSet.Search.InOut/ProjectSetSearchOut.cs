using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class ProjectSetSearchOut
{
    public ProjectSetSearchOut([AllowNull] FlatArray<ProjectItemSearchOut> projects)
        =>
        Projects = projects ?? FlatArray.Empty<ProjectItemSearchOut>();

    public FlatArray<ProjectItemSearchOut> Projects { get; }
}