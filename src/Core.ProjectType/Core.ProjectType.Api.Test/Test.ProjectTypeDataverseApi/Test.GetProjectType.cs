using System;
using Xunit;
using static GGroupp.Internal.Timesheet.TimesheetProjectTypeDataverseApi;

namespace GGroupp.Internal.Timesheet.Core.ProjectType.Api.Test;

partial class TimesheetProjectTypeDataverseApiTest
{
    [Theory]
    [InlineData("GG_project")]
    [InlineData("Lead")]
    [InlineData("Ppportunity")]
    [InlineData("Incident")]
    [InlineData("test")]
    [InlineData("")]
    [InlineData(null)]
    public void GetProjectTypeOrThrow_EntityNameIsNotActual_ExpectInvalidOperationException(string entityName)
    {
        _ = Assert.Throws<InvalidOperationException>(() => GetProjectTypeOrThrow(entityName));
    }

    [Theory]
    [InlineData("gg_project", TimesheetProjectType.Project)]
    [InlineData("lead", TimesheetProjectType.Lead)]
    [InlineData("opportunity", TimesheetProjectType.Opportunity)]
    [InlineData("incident", TimesheetProjectType.Incident)]
    public void GetProjectTypeOrThrow_EntityNameIsActual_ExpectProjectType(string entityName, TimesheetProjectType expected)
    {
        var actual = GetProjectTypeOrThrow(entityName);
        Assert.Equal(expected, actual);
    }
}