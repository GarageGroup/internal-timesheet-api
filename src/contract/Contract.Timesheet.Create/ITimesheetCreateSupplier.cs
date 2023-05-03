using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet;

public interface ITimesheetCreateSupplier
{
    ValueTask<Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>> CreateTimesheetAsync(
        TimesheetCreateIn input, CancellationToken cancellationToken);
}