using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

public interface ITimesheetSetGetSupplier
{
    ValueTask<Result<TimesheetSetGetOut, Failure<TimesheetSetGetFailureCode>>> GetTimesheetSetAsync(
        TimesheetSetGetIn input, CancellationToken cancellationToken);
}