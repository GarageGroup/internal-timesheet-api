using System;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;

namespace GGroupp.Internal.Timesheet;

partial class TimesheetApi
{
    public ValueTask<Result<Unit, Failure<TimesheetDeleteFailureCode>>> DeleteTimesheetAsync(
        TimesheetDeleteIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .HandleCancellation()
        .Pipe(
            static @in => new DataverseEntityDeleteIn(
                entityPluralName: BaseTimesheetItemJson.EntityPluralName,
                entityKey: new DataversePrimaryKey(@in.TimesheetId)))
        .PipeValue(
            GetDataverseApiClient(input.CallerUserId).DeleteEntityAsync)
        .MapFailure(
            static failure => failure.MapFailureCode(ToTimesheetDeleteFailureCode));

    private static TimesheetDeleteFailureCode ToTimesheetDeleteFailureCode(DataverseFailureCode code)
        =>
        code switch
        {
            DataverseFailureCode.UserNotEnabled => TimesheetDeleteFailureCode.NotAllowed,
            DataverseFailureCode.PrivilegeDenied => TimesheetDeleteFailureCode.NotAllowed,
            DataverseFailureCode.RecordNotFound => TimesheetDeleteFailureCode.NotFound,
            _ => default
        };
}