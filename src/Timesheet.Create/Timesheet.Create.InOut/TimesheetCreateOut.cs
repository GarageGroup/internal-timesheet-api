using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetCreateOut
{
    public TimesheetCreateOut(Guid id)
    {
        TimesheetId = id;
    }

    public Guid TimesheetId { get; }
}
