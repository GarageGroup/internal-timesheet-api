namespace GarageGroup.Internal.Timesheet;

public interface ITimesheetApi :
    IFavoriteProjectSetGetSupplier,
    IProjectSetSearchSupplier,
    ITimesheetSetGetSupplier,
    ITimesheetCreateSupplier,
    ITimesheetDeleteSupplier
{
}