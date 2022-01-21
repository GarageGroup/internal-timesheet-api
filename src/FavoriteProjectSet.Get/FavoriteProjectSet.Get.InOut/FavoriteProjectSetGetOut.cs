using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct FavoriteProjectSetGetOut
{
    public FavoriteProjectSetGetOut(Guid timesheetId) => TimesheetId = timesheetId;

    public Guid TimesheetId { get; }
}