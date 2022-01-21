﻿using GGroupp.Infra;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using static System.FormattableString;

namespace GGroupp.Internal.Timesheet;

partial class FavoriteProjectSetGetGetFunc
{
    public ValueTask<Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>> InvokeAsync(
        FavoriteProjectSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            @in => new DataverseEntityCreateIn<Dictionary<string, object?>>(
                entityPluralName: "gg_timesheetactivities",
                selectFields: selectedFields,
                entityData: MapJsonIn(@in)))
        .PipeValue(
            entityCreateSupplier.CreateEntityAsync<Dictionary<string, object?>, FavoriteProjectSetJsonOut>)
        .MapFailure(
            failure => failure.MapFailureCode(MapDataverseFailureCode))
        .MapSuccess(
            entityCreateOut => new FavoriteProjectSetGetOut(entityCreateOut.Value.TimesheetId));

    private static Dictionary<string, object?> MapJsonIn(FavoriteProjectSetGetIn input)
    {
        var entityData = GetProjectEntityData(input.ProjectType);
        return new()
        {
            [$"regardingobjectid_{entityData.Name}@odata.bind"] = Invariant($"/{entityData.PluralName}({input.ProjectId:D})"),
            ["gg_date"] = input.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["gg_description"] = input.Description,
            ["gg_duration"] = input.Duration
        };
    }

    private static (string Name, string PluralName) GetProjectEntityData(TimesheetProjectType projectType)
        =>
        projectType switch
        {
            TimesheetProjectType.Lead => ("lead", "leads"),
            TimesheetProjectType.Opportunity => ("opportunity", "opportunities"),
            _ => ("gg_project", "gg_projects")
        };

    private static FavoriteProjectSetGetFailureCode MapDataverseFailureCode(int dataverseFailureCode)
        =>
        dataverseFailureCode switch
        {
            NotFoundFailureCode => FavoriteProjectSetGetFailureCode.NotFound,
            _ => FavoriteProjectSetGetFailureCode.Unknown
        };
}