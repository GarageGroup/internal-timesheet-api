using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Internal.Timesheet;

public sealed record class FavoriteProjectItem
{
    public FavoriteProjectItem(Guid id, [AllowNull] string name, TimesheetProjectType type)
    {
        Id = id;
        Name = name ?? string.Empty;
        Type = type;
    }

    public Guid Id { get; }

    public string Name { get; }

    public TimesheetProjectType Type { get; }
}