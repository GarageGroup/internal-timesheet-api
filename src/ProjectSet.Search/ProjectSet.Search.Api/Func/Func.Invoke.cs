﻿using GGroupp.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GGroupp.Internal.Timesheet.TimesheetProjectTypeDataverseApi;

namespace GGroupp.Internal.Timesheet;

partial class ProjectSetSearchFunc
{
    public ValueTask<Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>> InvokeAsync(
        ProjectSetSearchIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .HandleCancellation()
        .Pipe<DataverseSearchIn>(
            static @in => new($"*{@in.SearchText}*")
            {
                Entities = EntityNames,
                Top = @in.Top
            })
        .PipeValue(
            dataverseSearchSupplier.SearchAsync)
        .MapFailure(
            static failure => failure.MapFailureCode(MapFailureCode))
        .MapSuccess<ProjectSetSearchOut>(
            static @out => new(@out.Value.Where(IsActualEntityName).Select(MapItemSearch).ToFlatArray()));

    private static bool IsActualEntityName(DataverseSearchItem item)
        =>
        EntityNames.Contains(item.EntityName);

    private static ProjectItemSearchOut MapItemSearch(DataverseSearchItem item)
    {
        var projectType = GetProjectTypeOrThrow(item.EntityName);
        var fieldName = GetEntityData(projectType).FieldName;

        return new(
            id: item.ObjectId,
            name: ExtractNameByProjectType(item.ExtensionData, projectType),
            type: projectType);
    }

    private static string? ExtractNameByProjectType(
        IReadOnlyCollection<KeyValuePair<string, DataverseSearchJsonValue>> extensionData,
        TimesheetProjectType projectType)
    {
        var entityData = GetEntityData(projectType);
        var projectName = extensionData.GetValueOrAbsent(entityData.FieldName).OrDefault()?.ToString();

        if (entityData.SecondFieldName is null)
        {
            return projectName;
        }

        var secondField = extensionData.GetValueOrAbsent(entityData.SecondFieldName).OrDefault()?.ToString();
        if (string.IsNullOrEmpty(secondField))
        {
            return projectName;
        }

        var stringBuilder = new StringBuilder(projectName);
        if(string.IsNullOrEmpty(projectName) is false)
        {
            stringBuilder.Append(' ');
        }
    
        return stringBuilder.Append('(').Append(secondField).Append(')').ToString();
    }

    private static ProjectSetSearchFailureCode MapFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.UserNotEnabled => ProjectSetSearchFailureCode.NotAllowed,
            DataverseFailureCode.SearchableEntityNotFound => ProjectSetSearchFailureCode.NotAllowed,
            DataverseFailureCode.Throttling => ProjectSetSearchFailureCode.TooManyRequests,
            _ => default
        };
}