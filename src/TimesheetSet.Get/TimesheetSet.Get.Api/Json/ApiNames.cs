using GGroupp.Infra;
using System.Collections.Generic;

namespace GGroupp.Internal.Timesheet;

internal static class ApiNames
{
    public const string DateFieldName = "gg_date";

    public const string DurationFieldName = "gg_duration";

    public const string HackproFieldName = "gg_hackpro";

    public const string DescriptionFieldName = "gg_description";

    public static readonly IReadOnlyCollection<string> SelectedFields = new[] 
    {
        DateFieldName, DurationFieldName, HackproFieldName, DescriptionFieldName 
    };

    public static readonly IReadOnlyCollection<DataverseOrderParameter> OrderBy = new[]
    {
        new DataverseOrderParameter("createdon", DataverseOrderDirection.Descending)
    };
}