using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;
public sealed record class ProjectsItemSearchOut
{
    public ProjectsItemSearchOut(Guid id, [AllowNull] string name, ProjectTypeSearchOut type)
    {
        Id = id;
        Name = name ?? string.Empty;
        Type = type;
    }

    public Guid Id { get; }

    public string Name { get; }

    public ProjectTypeSearchOut Type { get; }
}
