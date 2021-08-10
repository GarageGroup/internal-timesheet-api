#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet
{
    public sealed record TimesheetItemGetOut
    {
        public TimesheetItemGetOut(
            Guid id,
            Guid userId,
            DateTimeOffset date,
            [AllowNull] string description,
            decimal duration,
            [AllowNull] string projectName)
        {
            Id = id;
            UserId = userId;
            Date = date;
            Description = description ?? string.Empty;
            Duration = duration;
            ProjectName = projectName ?? string.Empty;
        }

        public Guid Id { get; }

        public Guid UserId { get; }

        public DateTimeOffset Date { get; }

        public string Description { get; }

        public decimal Duration { get; }

        public string ProjectName { get; }
    }
}