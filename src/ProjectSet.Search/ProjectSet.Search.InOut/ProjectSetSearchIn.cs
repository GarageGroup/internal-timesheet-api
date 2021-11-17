using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public readonly record struct ProjectSetSearchIn
{
    private readonly string? searchText;
        
    public ProjectSetSearchIn(
        [AllowNull] string searchText,
        int? top)
    {
        this.searchText = string.IsNullOrEmpty(searchText) ? null : searchText;
        Top = top;
    }

    public string SearchText => searchText ?? string.Empty;

    public int? Top { get; }
}
