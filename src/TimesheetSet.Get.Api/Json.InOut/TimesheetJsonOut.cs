#nullable enable

using System;
using System.Text.Json.Serialization;

namespace GGroupp.Internal.Timesheet
{
    internal sealed record TimesheetJsonOut
    {
        [JsonPropertyName(TimesheetJsonFieldName.Id)]
        public Guid Id { get; init; }

        [JsonPropertyName(TimesheetJsonFieldName.UserId)]
        public Guid UserId { get; init; }

        [JsonPropertyName(TimesheetJsonFieldName.Date)]
        public DateTime Date { get; init; }

        [JsonPropertyName(TimesheetJsonFieldName.Description)]
        public string? Description { get; init; }

        [JsonPropertyName(TimesheetJsonFieldName.Duration)]
        public decimal Duration { get; init; }

        [JsonPropertyName(TimesheetJsonFieldName.ProjectName)]
        public string? ProjectName { get; init; }
    }
}