using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetSetGetIn
{
    public TimesheetSetGetIn(
        DateOnly date,
        Guid userId)
    {
        Date = date;
        UserId = userId;
    }

    public DateOnly Date { get; }

    public Guid UserId { get; }
}