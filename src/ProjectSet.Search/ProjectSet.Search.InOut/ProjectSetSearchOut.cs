using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class ProjectSetSearchOut
{
    public ProjectSetSearchOut([AllowNull] IReadOnlyCollection<ProjectItemSearchOut> projects)
    {
        Projects = projects ?? Array.Empty<ProjectItemSearchOut>();
    }

    public IReadOnlyCollection<ProjectItemSearchOut> Projects { get; }
}