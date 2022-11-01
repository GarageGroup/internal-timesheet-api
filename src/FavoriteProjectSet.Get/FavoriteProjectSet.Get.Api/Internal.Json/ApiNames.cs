using GGroupp.Infra;
using System.Collections.Generic;

namespace GGroupp.Internal.Timesheet;

internal static class ApiNames
{
    public const string TimesheetEntityPluralName = "gg_timesheetactivities";

    public const string OwnerIdField = "_ownerid_value";

    public const string ProjectIdField = "_regardingobjectid_value";

    public const string NameField = "_regardingobjectid_value@OData.Community.Display.V1.FormattedValue";

    public const string TypeField = "_regardingobjectid_value@Microsoft.Dynamics.CRM.lookuplogicalname";

    public const string DateField = "gg_date";

    private const string CreatedOnField = "createdon";

    public static readonly FlatArray<string> AllFields;

    public static readonly FlatArray<DataverseOrderParameter> OrderFiels;

    static ApiNames()
    {
        AllFields = new[] { ProjectIdField, DateField };
        OrderFiels = new[]
        {
            new DataverseOrderParameter(DateField, DataverseOrderDirection.Descending),
            new DataverseOrderParameter(CreatedOnField, DataverseOrderDirection.Descending)
        };
    }
}