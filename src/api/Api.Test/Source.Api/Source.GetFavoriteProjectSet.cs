using System;
using System.Collections.Generic;
using GGroupp.Infra;

namespace GGroupp.Internal.Timesheet.Api.Test;

partial class TimesheetApiTestSource
{
    public static IEnumerable<object?[]> FavoriteProjectSetGetOutputTestData()
    {
        yield return GetEmptyResponseTestData();
        yield return GetResponseTestDataWithNegativeLimit();
        yield return GetResponseTestData(true);
        yield return GetResponseTestData(false);
    }

    private static object?[] GetEmptyResponseTestData()
    {
        var dataverseOut = new DataverseEntitySetGetOut<LastTimesheetItemJson>(default);
        var expected = default(FavoriteProjectSetGetOut);

        return new object?[] { dataverseOut, 5, expected };
    }

    private static object?[] GetResponseTestDataWithNegativeLimit()
    {
        var dataverseItem = new LastTimesheetItemJson
        {
            Project = new()
            {
                Id = Guid.Parse("3ccee31a-63be-45ad-9697-853a8746d38d"),
                Name = "Some project name"
            },
            TimesheetDate = new(2021, 07, 21, 15, 25, 01)
        };

        var dataverseOut = new DataverseEntitySetGetOut<LastTimesheetItemJson>(
            value: new(dataverseItem));

        var expected = default(FavoriteProjectSetGetOut);
        return new object?[] { dataverseOut, -1, expected };
    }

    private static object?[] GetResponseTestData(bool hasLimit)
    {
        var firstProjectId = Guid.Parse("9bc7aebd-33f2-4caf-966d-3073b3554ca3");

        var firstDataverseItem = new LastTimesheetItemJson
        {
            Project = new()
            {
                Id = firstProjectId,
                Name = "First project name"
            },
            TimesheetDate = new(2021, 07, 21, 15, 25, 01)
        };

        var secondDataverseItem = new LastTimesheetItemJson
        {
            Project = new()
            {
                Id = firstProjectId,
                Name = "Project number 2"
            },
            TimesheetDate = new(2022, 01, 11, 05, 27, 17)
        };

        var thirdDataverseItem = new LastTimesheetItemJson
        {
            Project = new()
            {
                Id = firstProjectId,
                Name = "Some project name"
            },
            TimesheetDate = new(2021, 12, 01)
        };

        var fourthDataverseItem = new LastTimesheetItemJson
        {
            Lead = new()
            {
                Id = Guid.Parse("03a107b9-6a35-4306-b61f-995e93b26e44"),
                Subject = "Some fourth name"
            },
            TimesheetDate = new(2021, 05, 23),
        };

        var fifthDataverseItem = new LastTimesheetItemJson
        {
            Opportunity = new()
            {
                Id = Guid.Parse("b55d6889-308a-47e9-b3d7-c7e3d3af2f53"),
                Name = "Fifth project"
            },
            TimesheetDate = new(2020, 11, 03),
        };

        var sixthDataverseItem = new LastTimesheetItemJson
        {
            TimesheetDate = new(2022, 01, 15),
        };

        var seventhDataverseItem = new LastTimesheetItemJson
        {
            Project = new()
            {
                Id = firstProjectId,
                Name = "Project 7"
            },
            TimesheetDate = new(2022, 01, 11, 05, 27, 17)
        };

        var eighthDataverseItem = new LastTimesheetItemJson
        {
            Project = new()
            {
                Id = Guid.Parse("7d54bf8d-add9-4414-a3ab-80e56eea6807"),
                Name = "Project 7"
            },
            TimesheetDate = new(2021, 01, 31, 11, 25, 17),
        };

        var ninthDataverseItem = new LastTimesheetItemJson
        {
            Incident = new()
            {
                Id = Guid.Parse("a29e3a95-8ddd-48df-a605-3a4d6a15567f"),
                Title = "Some eighth project name"
            },
            TimesheetDate = new(2022, 02, 03),
        };

        var tenthDataverseItem = new LastTimesheetItemJson
        {
            Project = new()
            {
                Id = Guid.Parse("35436d82-7a15-425e-a782-15b23141d43a"),
                Name = "The ninth project"
            },
            TimesheetDate = new(2022, 01, 15),
        };

        var dataverseOut = new DataverseEntitySetGetOut<LastTimesheetItemJson>(
            value: new(
                firstDataverseItem,
                secondDataverseItem,
                thirdDataverseItem,
                fourthDataverseItem,
                fifthDataverseItem,
                sixthDataverseItem,
                seventhDataverseItem,
                eighthDataverseItem,
                ninthDataverseItem,
                tenthDataverseItem));

        var expectedProjects = new List<FavoriteProjectItem>
        {
            new(firstProjectId, "First project name", TimesheetProjectType.Project),
            new(Guid.Parse("03a107b9-6a35-4306-b61f-995e93b26e44"), "Some fourth name", TimesheetProjectType.Lead),
            new(Guid.Parse("b55d6889-308a-47e9-b3d7-c7e3d3af2f53"), "Fifth project", TimesheetProjectType.Opportunity),
            new(Guid.Parse("7d54bf8d-add9-4414-a3ab-80e56eea6807"), "Project 7", TimesheetProjectType.Project),
            new(Guid.Parse("a29e3a95-8ddd-48df-a605-3a4d6a15567f"), "Some eighth project name", TimesheetProjectType.Incident)
        };

        if (hasLimit is false)
        {
            expectedProjects.Add(
                new(Guid.Parse("35436d82-7a15-425e-a782-15b23141d43a"), "The ninth project", TimesheetProjectType.Project));
        }

        var expected = new FavoriteProjectSetGetOut
        {
            Projects = expectedProjects
        };

        int? top = hasLimit ? 5 : null;
        return new object?[] { dataverseOut, top, expected };
    }
}