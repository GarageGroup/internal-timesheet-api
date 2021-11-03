using System;
using System.Text.Json.Serialization;

namespace GGroupp.Internal.Timesheet;

internal sealed record class TimesheetJsonOut
{
    [JsonPropertyName(TimesheetJsonFieldName.Id)]
    public Guid TimesheetId { get; init; }
}
