using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GGroupp.Internal.Timesheet;

internal static class ApiNames
{
    public const string TimesheetEntityPluralName = "gg_timesheetactivities";

    public const string OwnerIdField = "_ownerid_value";

    public const string ProjectIdField = "_regardingobjectid_value";

    public const string NameField = "_regardingobjectid_value@OData.Community.Display.V1.FormattedValue";

    public const string TypeField = "_regardingobjectid_value@Microsoft.Dynamics.CRM.lookuplogicalname";

    public const string DateField = "gg_date";

    public static readonly ReadOnlyDictionary<string, TimesheetProjectType> EntityTypes;

    public static readonly ReadOnlyCollection<string> AllFields;

    static ApiNames()
    {
        EntityTypes = new(new Dictionary<string, TimesheetProjectType>
        {
            ["gg_project"] = TimesheetProjectType.Project,
            ["lead"] = TimesheetProjectType.Lead,
            ["opportunity"] = TimesheetProjectType.Opportunity
        });

        AllFields = new(new[] { ProjectIdField, DateField });
    }
}