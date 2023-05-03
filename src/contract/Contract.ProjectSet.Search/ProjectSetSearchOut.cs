using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct ProjectSetSearchOut
{
    public required FlatArray<ProjectSearchItem> Projects { get; init; }
}