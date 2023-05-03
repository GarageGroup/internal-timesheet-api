using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct FavoriteProjectSetGetOut
{
    public required FlatArray<FavoriteProjectItem> Projects { get; init; }
}