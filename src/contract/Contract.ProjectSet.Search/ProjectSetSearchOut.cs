using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct ProjectSetSearchOut
{
    public required FlatArray<ProjectSearchItem> Projects { get; init; }
}