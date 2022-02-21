using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GGroupp.Internal.Timesheet;

public static partial class TimesheetProjectTypeDataverseApi
{
    private static readonly ReadOnlyCollection<string> entityNames;

    private static readonly ReadOnlyDictionary<string, TimesheetProjectType> entityTypes;

    private static readonly ReadOnlyDictionary<TimesheetProjectType, TimesheetProjectTypeEntityData> typeEntities;

    static TimesheetProjectTypeDataverseApi()
    {
        var types = new Dictionary<TimesheetProjectType, TimesheetProjectTypeEntityData>
        {
            [TimesheetProjectType.Project] = new("gg_project", "gg_projects", "gg_name"),
            [TimesheetProjectType.Lead] = new("lead", "leads", "subject", "companyname"),
            [TimesheetProjectType.Opportunity] = new("opportunity", "opportunities", "name"),
            [TimesheetProjectType.Incident] = new("incident", "incidents", "title")
        };

        entityTypes = new(types.ToDictionary(kv => kv.Value.EntityName, kv => kv.Key));
        entityNames = new(entityTypes.Keys.ToArray());
        typeEntities = new(types);
    }

    private static InvalidOperationException CreateUnexpectedEntityNameException(string? entityName)
        =>
        new($"Unexpected entity name: {entityName}");
}