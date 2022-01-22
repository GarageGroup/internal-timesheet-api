using System;
using System.Text.Json.Serialization;

namespace GGroupp.Internal.Timesheet;

internal sealed record class TimesheetJsonOut
{
    [JsonPropertyName("")] // TODO: Вставить названия согласно контракту Dataverse
    public DateTimeOffset Date { get; init; }

    [JsonPropertyName("")]
    public decimal Duration { get; init; }

    [JsonPropertyName("")]
    public string? ProjectName { get; init; }

    [JsonPropertyName("")]
    public string? Description { get; init; }
}
