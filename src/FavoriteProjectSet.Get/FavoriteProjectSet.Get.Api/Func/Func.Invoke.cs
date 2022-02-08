using GGroupp.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GGroupp.Internal.Timesheet.TimesheetProjectTypeDataverseApi;

namespace GGroupp.Internal.Timesheet;

partial class FavoriteProjectSetGetFunc
{
    public ValueTask<Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>> InvokeAsync(
        FavoriteProjectSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            @in => new DataverseEntitySetGetIn(
                entityPluralName: ApiNames.TimesheetEntityPluralName,
                selectFields: ApiNames.AllFields,
                orderBy: ApiNames.OrderFiels,
                filter: BuildFilter(@in.UserId),
                top: configuration.CountTimesheetItems)
            {
                IncludeAnnotations = "*"
            })
        .PipeValue(
            dataverseEntitySetGetSupplier.GetEntitySetAsync<TimesheetItemJson>)
        .MapFailure(
            static failure => failure.MapFailureCode(MapFailureCode))
        .MapSuccess(
            success => new FavoriteProjectSetGetOut(
                projects: MapProjects(success.Value, input.Top)));

    private string BuildFilter(Guid userId)
    {
        var filterBuilder = new StringBuilder($"{ApiNames.OwnerIdField} eq '{userId}'");

        if (configuration.CountTimesheetDays is not null)
        {
            var minDate = todayProvider.GetToday().AddDays(configuration.CountTimesheetDays.Value * -1);
            filterBuilder = filterBuilder.Append($" and {ApiNames.DateField} gt {minDate:yyyy-MM-dd}");
        }

        return filterBuilder.ToString();
    }

    private static IReadOnlyCollection<FavoriteProjectItemGetOut> MapProjects(
        IReadOnlyCollection<TimesheetItemJson> itemsJson, int? top)
        =>
        itemsJson.Where(
            static x => EntityNames.Contains(x.TimesheetProjectType.OrEmpty()))
        .GroupBy(
            static x => x.TimesheetProjectId)
        .Select(
            static x => x.First())
        .Select(
            static x => new FavoriteProjectItemGetOut(
                id: x.TimesheetProjectId,
                name: x.TimesheetProjectName,
                type: GetProjectTypeOrThrow(x.TimesheetProjectType.OrEmpty())))
        .Top(
            top)
        .ToArray();

    private static FavoriteProjectSetGetFailureCode MapFailureCode(DataverseFailureCode code)
        =>
        code switch
        {
            DataverseFailureCode.UserNotEnabled => FavoriteProjectSetGetFailureCode.NotAllowed,
            DataverseFailureCode.PrivilegeDenied => FavoriteProjectSetGetFailureCode.NotAllowed,
            _ => default
        };
}