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
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>>(cancellationToken);
        }
        return InnerInvokeAsync(input, cancellationToken);
    }

    private async ValueTask<Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>> InnerInvokeAsync(
        FavoriteProjectSetGetIn input, CancellationToken cancellationToken)
    {
        var @in = new DataverseEntitySetGetIn(
                entityPluralName: ApiNames.TimesheetEntityPluralName,
                selectFields: ApiNames.AllFields,
                orderBy: new[] { new DataverseOrderParameter(ApiNames.DateField, DataverseOrderDirection.Descending) },
                filter: $"{ApiNames.OwnerIdField} eq '{input.UserGuid}' and {ApiNames.DateField} ge {DateTime.Today.AddDays(configuration.CountTimesheetDays.GetValueOrDefault() * -1):yyyy-MM-dd}",
                top: configuration.CountTimesheetItems); // Count Timesheet Items
        var result = await dataverseEntitySetGetSupplier.GetEntitySetAsync<TimesheetItemJson>(@in, cancellationToken);
        if (result.IsSuccess)
        {
            var items = result.MapSuccess(GetProjects).MapSuccess(projects => projects.Top(input.Top)).SuccessOrThrow();
            return new FavoriteProjectSetGetOut(items.ToArray());

        }

        var failure = result.FailureOrThrow();
        return Failure.Create(FavoriteProjectSetGetFailureCode.Unknown, failure.FailureMessage);
    }

    private static IEnumerable<FavoriteProjectItemGetOut> GetProjects(DataverseEntitySetGetOut<TimesheetItemJson> @out)
        =>
        @out.Value.Where(
            x => ApiNames.EntityTypes.ContainsKey(x.TimesheetProjectType.OrEmpty()))
        .GroupBy(
            x => x.TimesheetProjectId)
        .Select(
            x => x.First())
        .Select(
            x => new FavoriteProjectItemGetOut(
                id: x.TimesheetProjectId,
                name: x.TimesheetProjectName,
                type: ApiNames.EntityTypes[x.TimesheetProjectType.OrEmpty()]));
}