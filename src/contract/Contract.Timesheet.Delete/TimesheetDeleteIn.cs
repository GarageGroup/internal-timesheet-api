using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetDeleteIn
{
    public required Guid TimesheetId { get; init; }

    public Guid? CallerUserId { get; init; }
}