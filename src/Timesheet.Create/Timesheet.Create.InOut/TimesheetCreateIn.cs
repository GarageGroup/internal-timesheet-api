using System;

namespace GGroupp.Internal.Timesheet;

public sealed record class TimeSheetCreateIn
{
    public TimeSheetCreateIn(
        Guid ownerId,
        DateOnly date,
        string? description,
        decimal duration,
        Guid projectId,
        TimesheetProjectType projectType)
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

    public string? Description { get; }

    public decimal Duration { get; }

    public Guid ProjectId { get; }

    public TimesheetProjectType ProjectType { get; }
}
