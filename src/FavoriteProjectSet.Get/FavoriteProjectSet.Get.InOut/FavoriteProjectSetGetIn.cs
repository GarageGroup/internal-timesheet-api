using System;

namespace GGroupp.Internal.Timesheet;

public readonly record struct FavoriteProjectSetGetIn
{
    public FavoriteProjectSetGetIn(
        TimesheetProjectType projectType)
    {
        ProjectType = projectType;
    }

    public TimesheetProjectType ProjectType { get; }

}