using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetDeleteIn
{
    public TimesheetDeleteIn(Guid timesheetId)
        =>
        TimesheetId = timesheetId;

    public Guid TimesheetId { get; }

}