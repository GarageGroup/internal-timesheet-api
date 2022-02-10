using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class TimesheetProjectTypeEntityData
{
    public TimesheetProjectTypeEntityData(string entityName, string entityPluralName, string fieldName, [AllowNull]string secondFieldName = null)
    {
        EntityName = entityName ?? string.Empty;
        EntityPluralName = entityPluralName ?? string.Empty;
        FieldName = fieldName ?? string.Empty;
        SecondFieldName = secondFieldName;
    }

    public string EntityName { get; }

    public string EntityPluralName { get; }

    public string FieldName { get; }

    public string? SecondFieldName { get; }
}