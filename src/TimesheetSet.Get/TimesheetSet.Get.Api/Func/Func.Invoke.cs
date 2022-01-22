using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;

namespace GGroupp.Internal.Timesheet;

partial class TimesheetSetGetFunc
{
    public ValueTask<Result<TimesheetSetGetOut, Failure<TimesheetSetGetFailureCode>>> InvokeAsync(TimesheetSetGetIn input, CancellationToken cancellationToken = default)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            MapInput)
        .PipeValue(
            entitySetGetSupplier.GetEntitySetAsync<TimesheetJsonOut>)
        .MapFailure(
            failure => failure.MapFailureCode(MapFailureCode))
        .MapSuccess(
            succsess => new TimesheetSetGetOut(
                timesheets: succsess.Value.Select(MapItemSuccess).ToArray()));

    private static DataverseEntitySetGetIn MapInput(TimesheetSetGetIn input)
        =>
        throw new NotImplementedException();

    private static TimesheetSetItemGetOut MapItemSuccess(TimesheetJsonOut itemJson)
        =>
        throw new NotImplementedException();

    private static TimesheetSetGetFailureCode MapFailureCode(int code)
        =>
        TimesheetSetGetFailureCode.Unknown;
}