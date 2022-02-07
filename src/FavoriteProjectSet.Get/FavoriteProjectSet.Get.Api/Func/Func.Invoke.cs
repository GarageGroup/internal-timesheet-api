using GGroupp.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                filter: $"{ApiNames.OwnerIdField} eq '{@in.UserGuid}' and {ApiNames.DateField} ge {DateTime.Today.AddDays(configuration.CountTimesheetDays * -1):yyyy-MM-dd}",
                top: configuration.CountTimesheetItems)
            {
                IncludeAnnotations = "*"
            })
        .PipeValue(
            dataverseEntitySetGetSupplier.GetEntitySetAsync<TimesheetItemJson>)
        .MapFailure(
            failure => failure.MapFailureCode(MapFailureCode))
        .MapSuccess(
            success => new FavoriteProjectSetGetOut(
                projects: success.Value
                .Where(
                    x => ApiNames.EntityTypes.ContainsKey(x.TimesheetProjectType.OrEmpty()))
                .GroupBy(
                    x => x.TimesheetProjectId)
                .Select(
                    x => x.First())
                .Select(
                    x => new FavoriteProjectItemGetOut(
                        id: x.TimesheetProjectId,
                        name: x.TimesheetProjectName,
                        type: ApiNames.EntityTypes[x.TimesheetProjectType.OrEmpty()]))
                .ToArray())
            );

    private static FavoriteProjectSetGetFailureCode MapFailureCode(DataverseFailureCode code)
        =>
        code switch
        {
            DataverseFailureCode.UserNotEnabled => FavoriteProjectSetGetFailureCode.NotAllowed,
            DataverseFailureCode.PrivilegeDenied => FavoriteProjectSetGetFailureCode.NotAllowed,
            _ => default
        };
}