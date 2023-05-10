using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;

namespace GarageGroup.Internal.Timesheet;

partial class TimesheetApi
{
    public ValueTask<Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>> GetFavoriteProjectSetAsync(
        FavoriteProjectSetGetIn input, CancellationToken cancellationToken)
        =>
        AsyncPipeline.Pipe(
            input, cancellationToken)
        .HandleCancellation()
        .Pipe(
            @in => new DataverseEntitySetGetIn(
                entityPluralName: BaseTimesheetItemJson.EntityPluralName,
                selectFields: LastTimesheetItemJson.SelectedFields,
                expandFields: BaseTimesheetItemJson.ExpandedFields,
                orderBy: LastTimesheetItemJson.OrderFiels,
                filter: BuildFilter(@in),
                top: option.FavoriteProjectItemsCount))
        .PipeValue(
            GetDataverseApiClient(input.CallerUserId).GetEntitySetAsync<LastTimesheetItemJson>)
        .MapFailure(
            static failure => failure.MapFailureCode(ToFavoriteProjectSetGetFailureCode))
        .MapSuccess(
            success => new FavoriteProjectSetGetOut()
            {
                Projects = GetProjects(success.Value, input.Top)
            });

    private string BuildFilter(FavoriteProjectSetGetIn input)
    {
        var today = todayProvider.GetToday();
        var maxDate = today.AddDays(1);

        DateOnly? minDate = option.FavoriteProjectDaysCount switch
        {
            not null => today.AddDays(option.FavoriteProjectDaysCount.Value * -1),
            _ => null
        };

        return LastTimesheetItemJson.BuildFilter(input.UserId, maxDate, minDate);
    }

    private static FlatArray<FavoriteProjectItem> GetProjects(
        FlatArray<LastTimesheetItemJson> itemsJson, int? top)
        =>
        itemsJson.AsEnumerable()
        .Select(
            static item => item.GetProjectType())
        .NotNull()
        .GroupBy(
            static type => type.Id)
        .Select(
            static item => item.First())
        .Select(
            static item => new FavoriteProjectItem(
                id: item.Id,
                name: item.Name,
                type: item.Type))
        .TakeTop(
            top)
        .ToFlatArray();

    private static FavoriteProjectSetGetFailureCode ToFavoriteProjectSetGetFailureCode(DataverseFailureCode code)
        =>
        code switch
        {
            DataverseFailureCode.UserNotEnabled => FavoriteProjectSetGetFailureCode.NotAllowed,
            DataverseFailureCode.PrivilegeDenied => FavoriteProjectSetGetFailureCode.NotAllowed,
            _ => default
        };
}