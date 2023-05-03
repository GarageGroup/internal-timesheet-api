using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class TimesheetCreateIn
{
    public TimesheetCreateIn(
        DateOnly date,
        Guid projectId,
        TimesheetProjectType projectType,
        decimal duration,
        [AllowNull] string description,
        TimesheetChannel channel = default)
    {
        Date = date;
        Description = string.IsNullOrEmpty(description) ? null: description;
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

    public Guid? CallerUserId { get; init; }
}