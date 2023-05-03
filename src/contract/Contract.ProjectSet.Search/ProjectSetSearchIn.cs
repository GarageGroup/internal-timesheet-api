using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public readonly record struct ProjectSetSearchIn
{
    private readonly string? searchText;
        
    public ProjectSetSearchIn([AllowNull] string searchText)
        =>
        this.searchText = string.IsNullOrEmpty(searchText) ? null : searchText;

    public string SearchText => searchText ?? string.Empty;

    public int? Top { get; init; }

    public Guid? CallerUserId { get; init; }
}