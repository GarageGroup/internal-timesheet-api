using GGroupp.Infra;
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

    public static readonly ReadOnlyCollection<string> AllFields;

    public static readonly ReadOnlyCollection<DataverseOrderParameter> OrderFiels;

    static ApiNames()
    {
        AllFields = new(new[] { ProjectIdField, DateField });
        OrderFiels = new(new[] { new DataverseOrderParameter(DateField, DataverseOrderDirection.Descending) });
    }
}