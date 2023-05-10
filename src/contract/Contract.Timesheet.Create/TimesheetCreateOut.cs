using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct TimesheetCreateOut
{
    public required Guid TimesheetId { get; init; }
}