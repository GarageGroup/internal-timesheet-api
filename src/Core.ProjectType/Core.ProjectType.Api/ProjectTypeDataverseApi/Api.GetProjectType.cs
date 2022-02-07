namespace GGroupp.Internal.Timesheet;

partial class TimesheetProjectTypeDataverseApi
{
    public static TimesheetProjectType GetProjectTypeOrThrow(string entityName)
    {
        if (entityTypes.TryGetValue(entityName ?? string.Empty, out var value))
        {
            return value;
        }

        throw CreateUnexpectedEntityNameException(entityName);
    }
}