namespace GGroupp.Internal.Timesheet;

public sealed record class TimesheetProjectTypeEntityData
{
    public TimesheetProjectTypeEntityData(string entityName, string entityPluralName, string fieldName)
    {
        EntityName = entityName ?? string.Empty;
        EntityPluralName = entityPluralName ?? string.Empty;
        FieldName = fieldName ?? string.Empty;
    }

    public string EntityName { get; }

    public string EntityPluralName { get; }

    public string FieldName { get; }
}