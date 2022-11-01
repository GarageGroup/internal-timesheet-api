using GGroupp.Infra;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GGroupp.Internal.Timesheet.ProjectSet.Search.Api.Test;

partial class ProjectSetSearchFuncTest
{
    [Fact]
    public void InvokeAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var dataverseOut = new DataverseSearchOut(1, new[] { SomeDataverseSearchItem });
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = SomeInput;
        var token = new CancellationToken(canceled: true);

        var valueTask = func.InvokeAsync(input, token);
        Assert.True(valueTask.IsCanceled);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", 15)]
    [InlineData("Some text", -10)]
    public async Task InvokeAsync_CancellationTokenIsNotCanceled_ExpectCallDataVerseApiClientOnce(string? searchText, int? top)
    {
        var dataverseOut = new DataverseSearchOut(1, new[] { SomeDataverseSearchItem });
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = new ProjectSetSearchIn(searchText, top);
        var token = new CancellationToken(false);

        _ = await func.InvokeAsync(input, token);

        var expectedText = string.IsNullOrEmpty(searchText) ? "**" : "*" + searchText + "*";

        var expected = new DataverseSearchIn("*" + searchText + "*")
        {
            Entities = TimesheetProjectTypeDataverseApi.EntityNames,
            Top = top
        };

        mockDataverseApiClient.Verify(c => c.SearchAsync(expected, token), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_DataverseResultIsSuccessNotEmpty_ExpectSuccessNotEmpty()
    {
        var firstProjectId = Guid.Parse("cc1efd36-ceca-eb11-bacc-000d3a47050c");

        var firstDataverseSearchItem = new DataverseSearchItem(
            searchScore: 18.698789596557617,
            objectId: firstProjectId,
            entityName: "opportunity",
            extensionData: default);

        var secondProjectId = Guid.Parse("93877469-68ca-eb11-bacc-000d3a47050c");
        var secondProjectName = "some second project name";

        var secondDataverseSearchItem = new DataverseSearchItem(
            searchScore: 16.482242584228516,
            objectId: secondProjectId,
            entityName: "gg_project",
            extensionData: new Dictionary<string, DataverseSearchJsonValue>
            {
                ["gg_name"] = new(JsonSerializer.SerializeToElement(secondProjectName))
            }.ToFlatArray());

        var thirdProjectId = Guid.Parse("5660cb5b-e3de-465a-9c2a-5a445c1faa1a");
        var thirdProjectName = "Third Project";

        var thirdDataverseSearchItem = new DataverseSearchItem(
            searchScore: 1000,
            objectId: thirdProjectId,
            entityName: "incident",
            extensionData: new Dictionary<string, DataverseSearchJsonValue>
            {
                ["title"] = new(JsonSerializer.SerializeToElement(thirdProjectName))
            }.ToFlatArray());

        var fourthProjectId = Guid.Parse("308891d9-cdca-eb11-bacc-000d3a47050c");

        var fourthDataverseSearchItem = new DataverseSearchItem(
            searchScore: 1001,
            objectId: fourthProjectId,
            entityName: "Some",
            extensionData: default);

        var fifthProjectId = Guid.Parse("07dedef2-951c-4405-8e17-4338e7408690");
        var fifthProjectName = "Some test";
        var fifthProjectNameCompanyName = "Some company name";

        var fifthDataverseSearchItem = new DataverseSearchItem(
            searchScore: 2000,
            objectId: fifthProjectId,
            entityName: "lead",
            extensionData: new Dictionary<string, DataverseSearchJsonValue>
            {
                ["subject"] = new(JsonSerializer.SerializeToElement(fifthProjectName)),
                ["companyname"] = new(JsonSerializer.SerializeToElement(fifthProjectNameCompanyName))
            }.ToFlatArray());

        var sixthProjectId = Guid.Parse("07dedef2-951c-4405-8e17-4338e7408287");
        var sixthProjectName = "Some test with empty companyname";
        var sixthDataverseSearchItem = new DataverseSearchItem(
            searchScore: 238,
            objectId: sixthProjectId,
            entityName: "lead",
            extensionData: new Dictionary<string, DataverseSearchJsonValue>
            {
                ["subject"] = new(JsonSerializer.SerializeToElement(sixthProjectName))
            }.ToFlatArray());

        var seventhProjectId = Guid.Parse("07dedef2-951c-4405-8e17-4338e7408237");
        var seventhProjectName = "Some test with empty subject";
        var seventhDataverseSearchItem = new DataverseSearchItem(
            searchScore: 238,
            objectId: seventhProjectId,
            entityName: "lead",
            extensionData: new Dictionary<string, DataverseSearchJsonValue>
            {
                ["companyname"] = new(JsonSerializer.SerializeToElement(seventhProjectName))
            }.ToFlatArray());

        var eighthsProjectId = Guid.Parse("07dedef2-951c-4405-8e17-4338e7408238");
        var eighthsDataverseSearchItem = new DataverseSearchItem(
            searchScore: 238,
            objectId: eighthsProjectId,
            entityName: "lead",
            extensionData: default);

        var dataverseOut = new DataverseSearchOut(
            totalRecordCount: 0,
            value: new[] 
            {
                firstDataverseSearchItem,
                secondDataverseSearchItem,
                thirdDataverseSearchItem,
                fourthDataverseSearchItem,
                fifthDataverseSearchItem, 
                sixthDataverseSearchItem,
                seventhDataverseSearchItem,
                eighthsDataverseSearchItem
            });

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);
        var func = CreateFunc(mockDataverseApiClient.Object);

        var actual = await func.InvokeAsync(SomeInput, default);

        var expected = new ProjectSetSearchOut(
            projects: new ProjectItemSearchOut[]
            {
                new(firstProjectId, string.Empty, TimesheetProjectType.Opportunity),
                new(secondProjectId, secondProjectName, TimesheetProjectType.Project),
                new(thirdProjectId, thirdProjectName, TimesheetProjectType.Incident),
                new(fifthProjectId, $"{fifthProjectName} ({fifthProjectNameCompanyName})", TimesheetProjectType.Lead),
                new(sixthProjectId, sixthProjectName, TimesheetProjectType.Lead),
                new(seventhProjectId, $"({seventhProjectName})", TimesheetProjectType.Lead),
                new(eighthsProjectId, string.Empty, TimesheetProjectType.Lead)
            });

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Throttling, ProjectSetSearchFailureCode.TooManyRequests)]
    [InlineData(DataverseFailureCode.UserNotEnabled, ProjectSetSearchFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, ProjectSetSearchFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unknown, ProjectSetSearchFailureCode.Unknown)]
    public async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, ProjectSetSearchFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some Failure message");
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseFailure);

        var func = CreateFunc(mockDataverseApiClient.Object);
        var actual = await func.InvokeAsync(SomeInput, CancellationToken.None);

        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);
        Assert.Equal(expected, actual);
    }
}
