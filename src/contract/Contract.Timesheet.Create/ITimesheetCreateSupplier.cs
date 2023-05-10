using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

public interface ITimesheetCreateSupplier
{
    ValueTask<Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>> CreateTimesheetAsync(
        TimesheetCreateIn input, CancellationToken cancellationToken);
}