namespace GGroupp.Internal.Timesheet;

public interface ITimesheetApi :
    IFavoriteProjectSetGetSupplier,
    IProjectSetSearchSupplier,
    ITimesheetSetGetSupplier,
    ITimesheetCreateSupplier,
    ITimesheetDeleteSupplier
{
}