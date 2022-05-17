using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class TimesheetSetItemGetOut
{
    public TimesheetSetItemGetOut(
        Guid timesheetId,
        DateOnly date,
        decimal duration,
        [AllowNull] string projectName,
        [AllowNull] string description)
    {
        TimesheetId = timesheetId;
        Date = date;
        Duration = duration;
        ProjectName = projectName ?? string.Empty;
        Description = description ?? string.Empty;
    }

    public Guid TimesheetId { get; }

    public DateOnly Date { get; }

    public decimal Duration { get; }

    public string ProjectName { get; }

    public string Description { get; }
}
