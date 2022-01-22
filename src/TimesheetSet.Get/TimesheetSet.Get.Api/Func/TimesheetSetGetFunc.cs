using GGroupp.Infra;
using System;

namespace GGroupp.Internal.Timesheet;

using ITimesheetSetGetFunc = IAsyncValueFunc<TimesheetSetGetIn, Result<TimesheetSetGetOut, Failure<TimesheetSetGetFailureCode>>>;

internal sealed partial class TimesheetSetGetFunc : ITimesheetSetGetFunc
{
    public static TimesheetSetGetFunc Create(IDataverseEntitySetGetSupplier entitySetGetSupplier)
        =>
        new(entitySetGetSupplier ?? throw new ArgumentNullException(nameof(entitySetGetSupplier)));

    private readonly IDataverseEntitySetGetSupplier entitySetGetSupplier;

    private TimesheetSetGetFunc(IDataverseEntitySetGetSupplier entitySetGetSupplier)
    {
        this.entitySetGetSupplier = entitySetGetSupplier;
    }
}