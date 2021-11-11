using System;
using System.Text.Json.Serialization;

namespace GGroupp.Internal.Timesheet;

internal readonly record struct TimesheetJsonOut
{
    [JsonPropertyName(TimesheetJsonFieldName.Id)]
    public Guid TimesheetId { get; init; }
}