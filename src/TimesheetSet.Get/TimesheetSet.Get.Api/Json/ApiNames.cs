using GGroupp.Infra;
using System.Collections.Generic;

namespace GGroupp.Internal.Timesheet;

internal static class ApiNames
{
    public const string TimesheetEntityPluralName = "gg_timesheetactivities";

    public const string DateFieldName = "gg_date";

    public const string DurationFieldName = "gg_duration";

    public const string ProjectFieldName = "gg_hackpro";

    public const string DescriptionFieldName = "gg_description";

    public const string ActivityIdFieldName = "activityid";

    public static readonly IReadOnlyCollection<string> SelectedFields;

    public static readonly IReadOnlyCollection<DataverseOrderParameter> OrderBy;

    static ApiNames()
    {
        SelectedFields = new[]
        {
            DateFieldName, DurationFieldName, ProjectFieldName, DescriptionFieldName
        };

        OrderBy = new[]
        {
            new DataverseOrderParameter("createdon", DataverseOrderDirection.Ascending)
        };
    }
}