using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class TimesheetSetItemGetOut
{
    public TimesheetSetItemGetOut(DateOnly date,
        Guid projectId,
        TimesheetProjectType projectType,
        decimal duration,
        string? description)
    {
        Date = date;
        Description = description;
        Duration = duration;
        ProjectId = projectId;
        ProjectType = projectType;
    }

    public DateOnly Date { get; }

    public Guid ProjectId { get; }

    public TimesheetProjectType ProjectType { get; }

    public decimal Duration { get; }

    public string? Description { get; }
}
