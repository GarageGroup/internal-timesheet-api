using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class TimesheetSetGetOut
{
    public TimesheetSetGetOut([AllowNull] FlatArray<TimesheetSetItemGetOut> timesheets)
        =>
        Timesheets = timesheets ?? FlatArray.Empty<TimesheetSetItemGetOut>();

    public FlatArray<TimesheetSetItemGetOut> Timesheets { get; }
}