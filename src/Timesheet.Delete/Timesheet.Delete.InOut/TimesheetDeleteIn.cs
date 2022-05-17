using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetDeleteIn
{
    public TimesheetDeleteIn(Guid activityId)
        =>
        ActivityId = activityId;

    public Guid ActivityId { get; }

}