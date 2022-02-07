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
        new(
            entityPluralName: "gg_timesheetactivities",
            selectFields: ApiNames.SelectedFields,
            filter: $"_ownerid_value eq '{input.UserId}' and createdon gt '{input.Date:yyyy-MM-dd}' and createdon lt '{input.Date.AddDays(1):yyyy-MM-dd}'",
            orderBy: ApiNames.OrderBy);

    private static TimesheetSetItemGetOut MapItemSuccess(TimesheetJsonOut itemJson)
        =>
        new(
            date: new(itemJson.Date.Year, itemJson.Date.Month, itemJson.Date.Day),
            duration: itemJson.Duration,
            projectName: itemJson.ProjectName, 
            description: itemJson.Description);

    private static TimesheetSetGetFailureCode MapFailureCode(DataverseFailureCode code)
        =>
        code switch
        {
            DataverseFailureCode.UserNotEnabled => TimesheetSetGetFailureCode.NotAllowed,
            DataverseFailureCode.PrivilegeDenied => TimesheetSetGetFailureCode.NotAllowed,
            _ => default
        };
}