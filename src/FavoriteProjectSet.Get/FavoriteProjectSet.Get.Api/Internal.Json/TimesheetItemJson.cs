using System;
using System.Text.Json.Serialization;

namespace GGroupp.Internal.Timesheet;
internal sealed record class TimesheetItemJson
{
    [JsonPropertyName(ApiNames.ProjectIdField)]
    public Guid TimesheetProjectId { get; init; }

    [JsonPropertyName(ApiNames.NameField)]
    public string? TimesheetProjectName { get; init; }

    [JsonPropertyName(ApiNames.TypeField)]
    public string? TimesheetProjectType { get; init; }

    [JsonPropertyName(ApiNames.DateField)]
    public DateTime TimesheetDate { get; init; }
}