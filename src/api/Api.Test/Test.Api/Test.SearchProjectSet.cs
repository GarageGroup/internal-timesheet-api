using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;
using Moq;
using Xunit;

namespace GGroupp.Internal.Timesheet.Api.Test;

partial class TimesheetApiTest
{
    [Fact]
    public static void SearchProjectSetAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var dataverseOut = new DataverseSearchOut(1, SomeDataverseSearchItem.AsFlatArray());
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var input = SomeProjectSetSearchInput;
        var cancellationToken = new CancellationToken(canceled: true);

        var valueTask = api.SearchProjectSetAsync(input, cancellationToken);
        Assert.True(valueTask.IsCanceled);
    }

    [Theory]
    [InlineData(null, null, "**")]
    [InlineData(Strings.Empty, 15, "**")]
    [InlineData("Some text", -10, "*Some text*")]
    public static async Task SearchProjectSetAsync_CancellationTokenIsNotCanceled_ExpectCallDataverseApiClientOnce(
        string? searchText, int? top, string expectedText)
    {
        var dataverseOut = new DataverseSearchOut(1, SomeDataverseSearchItem.AsFlatArray());
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var input = new ProjectSetSearchIn(searchText)
        {
            Top = top
        };

        var cancellationToken = new CancellationToken(false);
        _ = await api.SearchProjectSetAsync(input, cancellationToken);

        var expected = new DataverseSearchIn(expectedText)
        {
            Entities = new("gg_project", "lead", "opportunity", "incident"),
            Top = top
        };

        mockDataverseApiClient.Verify(c => c.SearchAsync(expected, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task SearchProjectSetAsync_CallerIdIsNotNull_ExpectCallDataverseImpersonateOnce()
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonCreateOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);
        var callerId = Guid.Parse("3589bc60-227f-4aa6-a5c3-4248304a1b49");

        var input = SomeProjectSetSearchInput with
        {
            CallerUserId = callerId
        };

        var cancellationToken = new CancellationToken(false);
        _ = await api.SearchProjectSetAsync(input, cancellationToken);

        mockDataverseApiClient.Verify(c => c.Impersonate(callerId), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Throttling, ProjectSetSearchFailureCode.TooManyRequests)]
    [InlineData(DataverseFailureCode.UserNotEnabled, ProjectSetSearchFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, ProjectSetSearchFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, ProjectSetSearchFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unknown, ProjectSetSearchFailureCode.Unknown)]
    public static async Task SearchProjectSetAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, ProjectSetSearchFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some Failure message");
        var dataverseResult = Result.Failure(dataverseFailure).With<DataverseSearchOut>();

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseResult);
        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var actual = await api.SearchProjectSetAsync(SomeProjectSetSearchInput, CancellationToken.None);
        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task SearchProjectSetAsync_DataverseResultIsSuccessNotEmpty_ExpectSuccessNotEmpty()
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
        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var actual = await api.SearchProjectSetAsync(SomeProjectSetSearchInput, default);

        var expected = new ProjectSetSearchOut
        {
            Projects = new(
                new(firstProjectId, string.Empty, TimesheetProjectType.Opportunity),
                new(secondProjectId, secondProjectName, TimesheetProjectType.Project),
                new(thirdProjectId, thirdProjectName, TimesheetProjectType.Incident),
                new(fifthProjectId, $"{fifthProjectName} ({fifthProjectNameCompanyName})", TimesheetProjectType.Lead),
                new(sixthProjectId, sixthProjectName, TimesheetProjectType.Lead),
                new(seventhProjectId, $"({seventhProjectName})", TimesheetProjectType.Lead),
                new(eighthsProjectId, string.Empty, TimesheetProjectType.Lead))
        };

        Assert.StrictEqual(expected, actual);
    }
}