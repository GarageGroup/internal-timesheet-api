using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class TimesheetApi
{
    public ValueTask<Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>> CreateTimesheetAsync(
        TimesheetCreateIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .HandleCancellation()
        .Pipe(
            @in => new DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>>(
                entityPluralName: BaseTimesheetItemJson.EntityPluralName,
                selectFields: TimesheetJsonCreateOut.AllFields,
                entityData: new TimesheetJsonCreateIn
                {
                    ProjectType = @in.ProjectType,
                    ProjectId = @in.ProjectId,
                    Date = @in.Date,
                    Description = @in.Description.OrNullIfEmpty(),
                    Duration = @in.Duration,
                    ChannelCode = option.ChannelCodes.AsEnumerable().GetValueOrAbsent(@in.Channel).OrDefault()
                }
                .BuildEntity()))
        .PipeValue(
            GetDataverseApiClient(input.CallerUserId).CreateEntityAsync<IReadOnlyDictionary<string, object?>, TimesheetJsonCreateOut>)
        .MapFailure(
            static failure => failure.MapFailureCode(ToTimesheetCreateFailureCode))
        .MapSuccess(
            static @out => new TimesheetCreateOut
            {
                TimesheetId = @out.Value.TimesheetId
            });

    private static TimesheetCreateFailureCode ToTimesheetCreateFailureCode(DataverseFailureCode dataverseFailureCode)
        =>
        dataverseFailureCode switch
        {
            DataverseFailureCode.RecordNotFound => TimesheetCreateFailureCode.NotFound,
            DataverseFailureCode.UserNotEnabled => TimesheetCreateFailureCode.NotAllowed,
            DataverseFailureCode.PrivilegeDenied => TimesheetCreateFailureCode.NotAllowed,
            DataverseFailureCode.Throttling => TimesheetCreateFailureCode.TooManyRequests,
            _ => default
        };
}