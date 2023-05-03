using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct FavoriteProjectSetGetIn
{
    public required Guid UserId { get; init; }

    public int? Top { get; init; }

    public Guid? CallerUserId { get; init; }
}