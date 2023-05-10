using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class TimesheetApi
{
    public ValueTask<Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>> SearchProjectSetAsync(
        ProjectSetSearchIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .HandleCancellation()
        .Pipe<DataverseSearchIn>(
            static @in => new($"*{@in.SearchText}*")
            {
                Entities = ProjectTypeEntityNames,
                Top = @in.Top
            })
        .PipeValue(
            GetDataverseApiClient(input.CallerUserId).SearchAsync)
        .MapFailure(
            static failure => failure.MapFailureCode(ToProjectSetSearchFailureCode))
        .MapSuccess(
            static @out => new ProjectSetSearchOut
            {
                Projects = @out.Value.AsEnumerable().Select(MapProjectType).NotNull().Select(MapItemSearch).ToFlatArray()
            });

    private static ITimesheetProjectType? MapProjectType(DataverseSearchItem item)
    {
        if (IncidentJson.FromSearchItem(item) is IncidentJson incident)
        {
            return incident;
        }

        if (LeadJson.FromSearchItem(item) is LeadJson lead)
        {
            return lead;
        }

        if (OpportunityJson.FromSearchItem(item) is OpportunityJson opportunity)
        {
            return opportunity;
        }

        if (ProjectJson.FromSearchItem(item) is ProjectJson project)
        {
            return project;
        }

        return null;
    }

    private static ProjectSearchItem MapItemSearch(ITimesheetProjectType item)
        =>
        new(
            id: item.Id,
            name: item.Name,
            type: item.Type);

    private static ProjectSetSearchFailureCode ToProjectSetSearchFailureCode(DataverseFailureCode failureCode)
        =>
        failureCode switch
        {
            DataverseFailureCode.UserNotEnabled => ProjectSetSearchFailureCode.NotAllowed,
            DataverseFailureCode.SearchableEntityNotFound => ProjectSetSearchFailureCode.NotAllowed,
            DataverseFailureCode.Throttling => ProjectSetSearchFailureCode.TooManyRequests,
            _ => default
        };
}