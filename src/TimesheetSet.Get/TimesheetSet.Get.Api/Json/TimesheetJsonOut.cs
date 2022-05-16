using System;
using System.Text.Json.Serialization;

namespace GGroupp.Internal.Timesheet;

internal sealed record class TimesheetJsonOut
{
    [JsonPropertyName(ApiNames.ActivityIdFieldName)]
    public Guid ActivityId { get; init; }

    [JsonPropertyName(ApiNames.DateFieldName)]
    public DateTimeOffset Date { get; init; }

    [JsonPropertyName(ApiNames.DurationFieldName)]
    public decimal Duration { get; init; }

    [JsonPropertyName(ApiNames.ProjectFieldName)]
    public string? ProjectName { get; init; }

    [JsonPropertyName(ApiNames.DescriptionFieldName)]
    public string? Description { get; init; }
}
