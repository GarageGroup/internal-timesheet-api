using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct FavoriteProjectSetGetIn
{
        
    public FavoriteProjectSetGetIn(
        Guid userGuid,
        int? top)
    {
        UserGuid = userGuid;
        Top = top;
    }

    public Guid UserGuid { get; }

    public int? Top { get; }
}