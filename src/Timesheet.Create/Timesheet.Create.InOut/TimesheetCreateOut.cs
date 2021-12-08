using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetCreateOut
{
    public TimesheetCreateOut(Guid timesheetId) => TimesheetId = timesheetId;

    public Guid TimesheetId { get; }
}