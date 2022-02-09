using System;
using System.Collections.Generic;
using GGroupp.Infra;

namespace GGroupp.Internal.Timesheet.FavoriteProjectSet.Search.Api.Test;

internal static class TestDataSource
{
    public static IEnumerable<object?[]> GetResponseTestData()
    {
        yield return GetEmptyResponseTestData();
        yield return GetResponseTestDataWithNegativeLimit();
        yield return GetResponseTestData(true);
        yield return GetResponseTestData(false);
    }

    private static object?[] GetEmptyResponseTestData()
    {
        var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(default);
        var expected = new FavoriteProjectSetGetOut(default);

        return new object?[] { dataverseOut, 5, expected };
    }

    private static object?[] GetResponseTestDataWithNegativeLimit()
    {
        var dataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = Guid.Parse("3ccee31a-63be-45ad-9697-853a8746d38d"),
            TimesheetProjectName = "Some project name",
            TimesheetProjectType = "gg_project",
            TimesheetDate = new(2021, 07, 21, 15, 25, 01)
        };

        var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(
            value: new[]
            {
                dataverseItem
            });

        var expected = new FavoriteProjectSetGetOut(default);
        return new object?[] { dataverseOut, -1, expected };
    }

    private static object?[] GetResponseTestData(bool hasLimit)
    {
        var firstProjectId = Guid.Parse("9bc7aebd-33f2-4caf-966d-3073b3554ca3");

        var firstDataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = firstProjectId,
            TimesheetProjectName = "First project name",
            TimesheetProjectType = "gg_project",
            TimesheetDate = new(2021, 07, 21, 15, 25, 01)
        };

        var secondDataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = firstProjectId,
            TimesheetProjectName = "Project number 2",
            TimesheetProjectType = "gg_project",
            TimesheetDate = new(2022, 01, 11, 05, 27, 17)
        };

        var thirdDataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = firstProjectId,
            TimesheetProjectName = "Some project name",
            TimesheetProjectType = "gg_project",
            TimesheetDate = new(2021, 12, 01)
        };

        var fourthDataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = Guid.Parse("03a107b9-6a35-4306-b61f-995e93b26e44"),
            TimesheetProjectName = "Some fourth name",
            TimesheetProjectType = "lead",
            TimesheetDate = new(2021, 05, 23),
        };

        var fifthDataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = Guid.Parse("b55d6889-308a-47e9-b3d7-c7e3d3af2f53"),
            TimesheetProjectName = "Fifth project",
            TimesheetProjectType = "opportunity",
            TimesheetDate = new(2020, 11, 03),
        };

        var sixthDataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = Guid.Parse("a70e3103-8fa7-4525-9898-961818c3f81b"),
            TimesheetProjectName = "The sixth project",
            TimesheetProjectType = "Some project type",
            TimesheetDate = new(2022, 01, 15),
        };

        var seventhDataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = firstProjectId,
            TimesheetProjectName = "Project 7",
            TimesheetProjectType = "gg_project",
            TimesheetDate = new(2022, 01, 11, 05, 27, 17)
        };

        var eighthDataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = Guid.Parse("7d54bf8d-add9-4414-a3ab-80e56eea6807"),
            TimesheetProjectName = "Project 7",
            TimesheetProjectType = "gg_project",
            TimesheetDate = new(2021, 01, 31, 11, 25, 17),
        };

        var ninthDataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = Guid.Parse("a29e3a95-8ddd-48df-a605-3a4d6a15567f"),
            TimesheetProjectName = "Some eighth project name",
            TimesheetProjectType = "incident",
            TimesheetDate = new(2022, 02, 03),
        };

        var tenthDataverseItem = new TimesheetItemJson
        {
            TimesheetProjectId = Guid.Parse("35436d82-7a15-425e-a782-15b23141d43a"),
            TimesheetProjectName = "The ninth project",
            TimesheetProjectType = "gg_project",
            TimesheetDate = new(2022, 01, 15),
        };

        var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(
            value: new[]
            {
                firstDataverseItem,
                secondDataverseItem,
                thirdDataverseItem,
                fourthDataverseItem,
                fifthDataverseItem,
                sixthDataverseItem,
                seventhDataverseItem,
                eighthDataverseItem,
                ninthDataverseItem,
                tenthDataverseItem
            });

        var expectedProjects = new List<FavoriteProjectItemGetOut>
        {
            new(firstDataverseItem.TimesheetProjectId, firstDataverseItem.TimesheetProjectName, TimesheetProjectType.Project),
            new(fourthDataverseItem.TimesheetProjectId, fourthDataverseItem.TimesheetProjectName, TimesheetProjectType.Lead),
            new(fifthDataverseItem.TimesheetProjectId, fifthDataverseItem.TimesheetProjectName, TimesheetProjectType.Opportunity),
            new(eighthDataverseItem.TimesheetProjectId, eighthDataverseItem.TimesheetProjectName, TimesheetProjectType.Project),
            new(ninthDataverseItem.TimesheetProjectId, ninthDataverseItem.TimesheetProjectName, TimesheetProjectType.Incident)
        };

        if (hasLimit is false)
        {
            expectedProjects.Add(
                new(tenthDataverseItem.TimesheetProjectId, tenthDataverseItem.TimesheetProjectName, TimesheetProjectType.Project));
        }

        var expected = new FavoriteProjectSetGetOut(expectedProjects);
        int? top = hasLimit ? 5 : null;

        return new object?[] { dataverseOut, top, expected };
    }
}