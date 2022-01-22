using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class TimesheetSetGetOut
{
    public TimesheetSetGetOut([AllowNull] IReadOnlyCollection<TimesheetItemGetOut> projects)
    {
        Projects = projects ?? Array.Empty<TimesheetItemGetOut>();
    }

    public IReadOnlyCollection<TimesheetItemGetOut> Projects { get; }
}