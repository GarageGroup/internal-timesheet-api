#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet
{
    public sealed record TimesheetSetGetOut
    {
        public TimesheetSetGetOut([AllowNull] IReadOnlyCollection<TimesheetItemGetOut> timesheets)
            =>
            Timesheets = timesheets ?? Array.Empty<TimesheetItemGetOut>();

        public IReadOnlyCollection<TimesheetItemGetOut> Timesheets { get; }
    }
}