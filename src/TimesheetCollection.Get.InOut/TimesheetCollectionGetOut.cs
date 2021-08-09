#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet
{
    public sealed record TimesheetCollectionGetOut
    {
        public TimesheetCollectionGetOut([AllowNull] IReadOnlyCollection<TimesheetItemGetOut> timesheets)
            =>
            Timesheets = timesheets ?? Array.Empty<TimesheetItemGetOut>();

        public IReadOnlyCollection<TimesheetItemGetOut> Timesheets { get; }
    }
}