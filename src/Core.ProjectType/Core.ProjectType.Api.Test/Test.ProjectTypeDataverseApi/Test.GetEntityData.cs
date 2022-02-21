using Xunit;

namespace GGroupp.Internal.Timesheet.Core.ProjectType.Api.Test;

partial class TimesheetProjectTypeDataverseApiTest
{
    [Fact]
    public void GetEntityData_ProjectTypeIsProject()
    {
        var actual = TimesheetProjectTypeDataverseApi.GetEntityData(TimesheetProjectType.Project);

        var expected = new TimesheetProjectTypeEntityData(
            entityName: "gg_project",
            entityPluralName: "gg_projects",
            fieldName: "gg_name");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetEntityData_ProjectTypeIsLead()
    {
        var actual = TimesheetProjectTypeDataverseApi.GetEntityData(TimesheetProjectType.Lead);

        var expected = new TimesheetProjectTypeEntityData(
            entityName: "lead",
            entityPluralName: "leads",
            fieldName: "subject",
            secondFieldName: "companyname");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetEntityData_ProjectTypeIsOpportunity()
    {
        var actual = TimesheetProjectTypeDataverseApi.GetEntityData(TimesheetProjectType.Opportunity);

        var expected = new TimesheetProjectTypeEntityData(
            entityName: "opportunity",
            entityPluralName: "opportunities",
            fieldName: "name");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetEntityData_ProjectTypeIsIncident()
    {
        var actual = TimesheetProjectTypeDataverseApi.GetEntityData(TimesheetProjectType.Incident);

        var expected = new TimesheetProjectTypeEntityData(
            entityName: "incident",
            entityPluralName: "incidents",
            fieldName: "title");

        Assert.Equal(expected, actual);
    }
}