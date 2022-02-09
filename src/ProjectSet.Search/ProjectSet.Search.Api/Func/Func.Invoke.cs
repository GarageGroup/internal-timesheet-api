using GGroupp.Infra;
using System;
using System.Linq;
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
            static @out => new(@out.Value.Where(IsActualEntityName).Select(MapItemSearch).ToArray()));

    private static bool IsActualEntityName(DataverseSearchItem item)
        =>
        EntityNames.Contains(item.EntityName);

    private static ProjectItemSearchOut MapItemSearch(DataverseSearchItem item)
    {
        var projectType = GetProjectTypeOrThrow(item.EntityName);
        var fieldName = GetEntityData(projectType).FieldName;

        return new(
            id: item.ObjectId,
            name: item.ExtensionData.GetValueOrAbsent(fieldName).OrDefault()?.ToString(),
            type: projectType);
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