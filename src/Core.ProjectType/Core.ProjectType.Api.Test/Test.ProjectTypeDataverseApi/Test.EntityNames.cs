using Xunit;

namespace GGroupp.Internal.Timesheet.Core.ProjectType.Api.Test;

partial class TimesheetProjectTypeDataverseApiTest
{
    [Fact]
    public void EntityNames_ExpectAllEntityNames()
    {
        var actual = TimesheetProjectTypeDataverseApi.EntityNames;
        var expected = new[] { "gg_project", "lead", "opportunity", "incident" };

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EntityNamesTwice_ExpectSameEntityNames()
    {
        var first = TimesheetProjectTypeDataverseApi.EntityNames;
        var second = TimesheetProjectTypeDataverseApi.EntityNames;

        Assert.Same(first, second);
    }
}