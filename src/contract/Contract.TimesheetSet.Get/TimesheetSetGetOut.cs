using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct TimesheetSetGetOut
{
    public required FlatArray<TimesheetSetItem> Timesheets { get; init; }
}