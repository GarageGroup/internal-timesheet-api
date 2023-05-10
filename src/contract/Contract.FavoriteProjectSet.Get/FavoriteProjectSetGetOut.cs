using System;

namespace GarageGroup.Internal.Timesheet;

public readonly record struct FavoriteProjectSetGetOut
{
    public required FlatArray<FavoriteProjectItem> Projects { get; init; }
}