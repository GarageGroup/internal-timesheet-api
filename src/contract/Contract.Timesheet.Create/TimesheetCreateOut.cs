using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct TimesheetCreateOut
{
    public required Guid TimesheetId { get; init; }
}