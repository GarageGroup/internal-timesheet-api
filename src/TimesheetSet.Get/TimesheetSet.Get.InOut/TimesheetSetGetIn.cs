using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetSetGetIn
{
    public TimesheetSetGetIn(Guid userId, DateOnly dateFrom, DateOnly dateTo)
    {
        UserId = userId;
        DateFrom = dateFrom;
        DateTo = dateTo;
    }

    public Guid UserId { get; }

    public DateOnly DateFrom { get; }

    public DateOnly DateTo { get; }
}