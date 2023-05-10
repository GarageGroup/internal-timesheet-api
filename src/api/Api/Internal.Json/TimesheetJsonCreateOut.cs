using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Internal.Timesheet;

internal readonly record struct TimesheetJsonCreateOut
{
    public static readonly FlatArray<string> AllFields = new(IdFieldName);

    private const string IdFieldName = "activityid";

    [JsonPropertyName(IdFieldName)]
    public Guid TimesheetId { get; init; }
}