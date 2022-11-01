﻿using GGroupp.Infra;
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
        .HandleCancellation()
        .Pipe(
            @in => new DataverseEntitySetGetIn(
                entityPluralName: ApiNames.TimesheetEntityPluralName,
                selectFields: ApiNames.AllFields,
                orderBy: ApiNames.OrderFiels,
                filter: BuildFilter(@in.UserId),
                top: option.CountTimesheetItems)
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
        var today = todayProvider.GetToday();
        var maxDate = today.AddDays(1);

        var filterBuilder = new StringBuilder()
            .Append($"{ApiNames.OwnerIdField} eq '{userId}'")
            .Append($" and {ApiNames.ProjectIdField} ne null")
            .Append($" and {ApiNames.DateField} lt {maxDate:yyyy-MM-dd}");

        if (option.CountTimesheetDays is not null)
        {
            var minDate = today.AddDays(option.CountTimesheetDays.Value * -1);
            filterBuilder = filterBuilder.Append($" and {ApiNames.DateField} gt {minDate:yyyy-MM-dd}");
        }

        return filterBuilder.ToString();
    }

    private static FlatArray<FavoriteProjectItemGetOut> MapProjects(
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
        .ToFlatArray();

    private static FavoriteProjectSetGetFailureCode MapFailureCode(DataverseFailureCode code)
        =>
        code switch
        {
            DataverseFailureCode.UserNotEnabled => FavoriteProjectSetGetFailureCode.NotAllowed,
            DataverseFailureCode.PrivilegeDenied => FavoriteProjectSetGetFailureCode.NotAllowed,
            _ => default
        };
}