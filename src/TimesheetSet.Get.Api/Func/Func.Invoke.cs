#nullable enable

using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;
using static System.FormattableString;

namespace GGroupp.Internal.Timesheet
{
    partial class TimesheetSetGetFunc
    {
        public ValueTask<Result<TimesheetSetGetOut, Failure<Unit>>> InvokeAsync(
            TimesheetSetGetIn input, CancellationToken cancellationToken = default)
            =>
            AsyncPipeline.Start(
                input ?? throw new ArgumentNullException(nameof(input)),
                cancellationToken)
            .Pipe(
                @in => new DataverseEntitySetGetIn(
                    entitySetName: "gg_timesheetactivities",
                    selectFields: new[]
                    {
                        TimesheetJsonFieldName.Id,
                        TimesheetJsonFieldName.UserId,
                        TimesheetJsonFieldName.Date,
                        TimesheetJsonFieldName.Description,
                        TimesheetJsonFieldName.Duration,
                        TimesheetJsonFieldName.ProjectName
                    },
                    filter: BuildFilter(input)))
            .PipeValue(
                dataverseEntitySetGetSupplier.GetEntitySetAsync<TimesheetJsonOut>)
            .MapFailure(
                failure => failure.MapFailureCode(_ => Unit.Value))
            .MapSuccess(
                dataverseOut => new TimesheetSetGetOut(
                    timesheets: dataverseOut.Value.Select(MapTimesheetJson).ToArray()));

        private static string BuildFilter(TimesheetSetGetIn input)
            =>
            Pipeline.Pipe(
                new StringBuilder())
            .Append(
                Invariant($"{TimesheetJsonFieldName.UserId}%20eq%20{input.UserId}"))
            .Append(
                Invariant($"%20and%20{TimesheetJsonFieldName.Date}%20ge%20{input.DateFrom:yyyy-MM-dd}"))
            .Append(
                Invariant($"%20and%20{TimesheetJsonFieldName.Date}%20le%20{input.DateTo:yyyy-MM-dd}"))
            .ToString();

        private static TimesheetItemGetOut MapTimesheetJson(TimesheetJsonOut jsonOut)
            =>
            new(
                id: jsonOut.Id,
                userId: jsonOut.UserId,
                date: new(jsonOut.Date, default),
                description: jsonOut.Description,
                duration: jsonOut.Duration,
                projectName: jsonOut.ProjectName);
    }
}