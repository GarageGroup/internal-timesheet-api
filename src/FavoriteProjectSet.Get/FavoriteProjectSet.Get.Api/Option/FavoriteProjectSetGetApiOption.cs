namespace GGroupp.Internal.Timesheet;

public readonly record struct FavoriteProjectSetGetApiOption
{
    public FavoriteProjectSetGetApiOption(int? countTimesheetItems, int? countTimesheetDays)
        =>
        (CountTimesheetItems, CountTimesheetDays) = (countTimesheetItems, countTimesheetDays);

    public int? CountTimesheetItems { get; }

    public int? CountTimesheetDays { get; }
}