using GGroupp.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet;

partial class ProjectSetSearchFunc
{
    public partial ValueTask<Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>> InvokeAsync(
        ProjectSetSearchIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .Pipe(
            @in => new DataverseSearchIn($"*{@in.SearchText}*")
            {
                Entities = entityNames,
                Top = @in.Top
            })
        .PipeValue(
            dataverseSearchSupplier.SearchAsync)
        .MapFailure(
            failure => failure.MapFailureCode( _ => ProjectSetSearchFailureCode.Unknown))
        .MapSuccess(
            @out => new ProjectSetSearchOut(
                @out.Value.Where(IsActualEntityName).Select(MapItemSearch).ToArray()));

    private static bool IsActualEntityName(DataverseSearchItem item)
        =>
        entityTypes.ContainsKey(item.EntityName);

    private static ProjectItemSearchOut MapItemSearch(DataverseSearchItem item)
        =>
        new(
            id: item.ObjectId,
            name: item.ExtensionData.GetValueOrAbsent("name").OrDefault()?.ToString(),
            type: entityTypes[item.EntityName]);

}
