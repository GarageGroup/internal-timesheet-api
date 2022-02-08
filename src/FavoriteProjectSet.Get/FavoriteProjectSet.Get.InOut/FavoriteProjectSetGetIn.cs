using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct FavoriteProjectSetGetIn
{
    public FavoriteProjectSetGetIn(Guid userId, int? top = null)
    {
        UserId = userId;
        Top = top;
    }

    public Guid UserId { get; }

    public int? Top { get; }
}