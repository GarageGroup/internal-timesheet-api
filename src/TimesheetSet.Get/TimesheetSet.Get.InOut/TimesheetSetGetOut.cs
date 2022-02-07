using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class TimesheetSetGetOut
{
    public TimesheetSetGetOut([AllowNull] IReadOnlyCollection<TimesheetSetItemGetOut> timesheets)
    {
        Timesheets = timesheets ?? Array.Empty<TimesheetSetItemGetOut>();
    }

    public IReadOnlyCollection<TimesheetSetItemGetOut> Timesheets { get; }
}