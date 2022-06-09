using GGroupp.Infra;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GGroupp.Internal.Timesheet.TimesheetProjectTypeDataverseApi;
using static System.FormattableString;

namespace GGroupp.Internal.Timesheet;

partial class TimesheetCreateFunc
{
    public ValueTask<Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>> InvokeAsync(
        TimesheetCreateIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .HandleCancellation()
        .Pipe(
            @in => new DataverseEntityCreateIn<Dictionary<string, object?>>(
                entityPluralName: "gg_timesheetactivities",
                selectFields: selectedFields,
                entityData: CreateEntityData(@in)))
        .PipeValue(
            entityCreateSupplier.CreateEntityAsync<Dictionary<string, object?>, TimesheetJsonOut>)
        .MapFailure(
            failure => failure.MapFailureCode(MapDataverseFailureCode))
        .MapSuccess(
            entityCreateOut => new TimesheetCreateOut(entityCreateOut.Value.TimesheetId));

    private Dictionary<string, object?> CreateEntityData(TimesheetCreateIn input)
    {
        var projectTypeEntityData = GetEntityData(input.ProjectType);

        var name = projectTypeEntityData.EntityName;
        var pluralName = projectTypeEntityData.EntityPluralName;

        var entityData = new Dictionary<string, object?>
        {
            [$"regardingobjectid_{name}@odata.bind"] = Invariant($"/{pluralName}({input.ProjectId:D})"),
            ["gg_date"] = input.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["gg_description"] = input.Description,
            ["gg_duration"] = input.Duration
        };

        var channelCode = option.ChannelCodes.GetValueOrAbsent(input.Channel).OrDefault();
        if (channelCode is not null)
        {
            entityData.Add("gg_timesheetactivity_channel", channelCode);
        }

        return entityData;
    }

    private static TimesheetCreateFailureCode MapDataverseFailureCode(DataverseFailureCode dataverseFailureCode)
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
