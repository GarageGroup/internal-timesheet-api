using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class ProjectSetSearchOut
{
    public ProjectSetSearchOut([AllowNull] IReadOnlyCollection<ProjectsItemSearchOut> projects)
    {
        Projects = projects ?? Array.Empty<ProjectsItemSearchOut>();
    }

    public IReadOnlyCollection<ProjectsItemSearchOut> Projects { get; }
}
