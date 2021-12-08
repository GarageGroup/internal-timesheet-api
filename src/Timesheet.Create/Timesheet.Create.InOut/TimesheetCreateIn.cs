using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetCreateIn
{
    public TimesheetCreateIn(
        Guid ownerId,
        DateOnly date,
        Guid projectId,
        TimesheetProjectType projectType,
        decimal duration,
        string? description)
    {
        OwnerId = ownerId;
        Date = date;
        Description = description;
        Duration = duration;
        ProjectId = projectId;
        ProjectType = projectType;
    }

    public Guid OwnerId { get; }

    public DateOnly Date { get; }

    public Guid ProjectId { get; }

    public TimesheetProjectType ProjectType { get; }

    public decimal Duration { get; }

    public string? Description { get; }
}