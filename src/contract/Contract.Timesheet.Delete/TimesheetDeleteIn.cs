using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct TimesheetDeleteIn
{
    public required Guid TimesheetId { get; init; }

    public Guid? CallerUserId { get; init; }
}