using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetSetGetIn
{
    private readonly string? searchText;
        
    public TimesheetSetGetIn(
        DateOnly date,
        Guid id)
    {
        this.date = Date;
        Id = id;
    }

    public DateOnly Date { get; }

    public Guid Id { get; }
}