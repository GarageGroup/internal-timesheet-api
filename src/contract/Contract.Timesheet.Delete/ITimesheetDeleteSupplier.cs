using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet;

public interface ITimesheetDeleteSupplier
{
    ValueTask<Result<Unit, Failure<TimesheetDeleteFailureCode>>> DeleteTimesheetAsync(
        TimesheetDeleteIn input, CancellationToken cancellationToken);
}