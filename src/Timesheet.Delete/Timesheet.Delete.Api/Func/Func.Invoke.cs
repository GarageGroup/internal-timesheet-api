using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;

namespace GGroupp.Internal.Timesheet;

partial class TimesheetDeleteFunc
{
    public ValueTask<Result<Unit, Failure<TimesheetDeleteFailureCode>>> InvokeAsync(
        TimesheetDeleteIn input, CancellationToken cancellationToken = default)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .HandleCancellation()
        .Pipe(
            static @in => new DataverseEntityDeleteIn(
                entityPluralName: TimesheetActivityEntityPluralName,
                entityKey: new DataversePrimaryKey(@in.TimesheetId)))
        .PipeValue(
            entityDeleteSupplier.DeleteEntityAsync)
        .MapFailure(
            static failure => failure.MapFailureCode(MapFailureCode));

    private static TimesheetDeleteFailureCode MapFailureCode(DataverseFailureCode code)
        =>
        code switch
        {
            DataverseFailureCode.UserNotEnabled => TimesheetDeleteFailureCode.NotAllowed,
            DataverseFailureCode.PrivilegeDenied => TimesheetDeleteFailureCode.NotAllowed,
            DataverseFailureCode.RecordNotFound => TimesheetDeleteFailureCode.NotFound,
            _ => default
        };
}