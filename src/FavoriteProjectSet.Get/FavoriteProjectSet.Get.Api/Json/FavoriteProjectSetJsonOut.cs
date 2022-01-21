using System;
using System.Text.Json.Serialization;

namespace GGroupp.Internal.Timesheet;

internal readonly record struct FavoriteProjectSetJsonOut
{
    [JsonPropertyName(FavoriteProjectSetJsonFieldName.Id)]
    public Guid TimesheetId { get; init; }
}