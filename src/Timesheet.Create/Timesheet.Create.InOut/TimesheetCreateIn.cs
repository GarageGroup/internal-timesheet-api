using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetCreateIn
{
    public TimesheetCreateIn(
        DateOnly date,
        Guid projectId,
        TimesheetProjectType projectType,
        decimal duration,
        string? description,
        TimesheetChannel channel = default)
    {
        Date = date;
        Description = description;
        Duration = duration;
        ProjectId = projectId;
        ProjectType = projectType;
        Channel = channel;
    }

    public DateOnly Date { get; }

    public Guid ProjectId { get; }

    public TimesheetProjectType ProjectType { get; }

    public decimal Duration { get; }

    public string? Description { get; }

    public TimesheetChannel Channel { get; }
}