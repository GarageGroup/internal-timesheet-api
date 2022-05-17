using GGroupp.Infra;
using System;

namespace GGroupp.Internal.Timesheet;

using ITimesheetDeleteFunc = IAsyncValueFunc<TimesheetDeleteIn, Result<Unit, Failure<TimesheetDeleteFailureCode>>>;

internal sealed partial class TimesheetDeleteFunc : ITimesheetDeleteFunc
{
    public static TimesheetDeleteFunc Create(IDataverseEntityDeleteSupplier entityDeleteSupplier)
        =>
        new(entityDeleteSupplier ?? throw new ArgumentNullException(nameof(entityDeleteSupplier)));

    private readonly IDataverseEntityDeleteSupplier entityDeleteSupplier;

    private const string TimesheetActivityEntityPluralName = "gg_timesheetactivities";

    private TimesheetDeleteFunc(IDataverseEntityDeleteSupplier entityDeleteSupplier)
        =>
        this.entityDeleteSupplier = entityDeleteSupplier;
}