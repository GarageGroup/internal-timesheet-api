using DeepEqual.Syntax;
using GGroupp.Infra;
using Moq;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GGroupp.Internal.Timesheet;

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

    [Fact]
    public async Task InvokeAsync_CancellationTokenIsNotCanceledAndInputIsDefault_ExpectCallDataVerseApiClientOnce()
    {
        var dataverseOut = new DataverseSearchOut(1, new[] { SomeDataverseSearchItem });
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut, IsMatchDataverseInput);

        var token = new CancellationToken(false);

        var func = CreateFunc(mockDataverseApiClient.Object);
        _ = await func.InvokeAsync(default, token);

        mockDataverseApiClient.Verify(c => c.SearchAsync(It.IsAny<DataverseSearchIn>(), token), Times.Once);

        static void IsMatchDataverseInput(DataverseSearchIn actual)
        {
            var expected = new DataverseSearchIn($"**")
            {
                Entities = new[] { "gg_project", "lead", "opportunity" }
            };
            actual.ShouldDeepEqual(expected);
        }
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
                ["name"] = new(JsonSerializer.SerializeToElement(secondProjectName))
            });

        var thirdProjectId = Guid.Parse("5660cb5b-e3de-465a-9c2a-5a445c1faa1a");

        var thirdDataverseSearchItem = new DataverseSearchItem(
            searchScore: 1000,
            objectId: thirdProjectId,
            entityName: "some",
            extensionData: default);

        var fourthProjectId = Guid.Parse("308891d9-cdca-eb11-bacc-000d3a47050c");

        var fourthDataverseSearchItem = new DataverseSearchItem(
            searchScore: 1001,
            objectId: fourthProjectId,
            entityName: "Some",
            extensionData: default);

        var fifthProjectId = Guid.Parse("07dedef2-951c-4405-8e17-4338e7408690");
        var fifthProjectName = "Some test";

        var fifthDataverseSearchItem = new DataverseSearchItem(
            searchScore: 2000,
            objectId: fifthProjectId,
            entityName: "lead",
            extensionData: new Dictionary<string, DataverseSearchJsonValue>
            {
                ["name"] = new(JsonSerializer.SerializeToElement(fifthProjectName))
            });

        var dataverseOut = new DataverseSearchOut(
            totalRecordCount: 0,
            value: new[] 
            {
                firstDataverseSearchItem,
                secondDataverseSearchItem,
                thirdDataverseSearchItem,
                fourthDataverseSearchItem,
                fifthDataverseSearchItem
            });

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);
        var func = CreateFunc(mockDataverseApiClient.Object);

        var actualResult = await func.InvokeAsync(SomeInput, default);

        Assert.True(actualResult.IsSuccess);
        var actual = actualResult.SuccessOrThrow().Projects;

        var expected = new ProjectsItemSearchOut[]
        {
            new(firstProjectId, string.Empty, ProjectTypeSearchOut.Opportunity),
            new(secondProjectId, secondProjectName, ProjectTypeSearchOut.Project),
            new(fifthProjectId, fifthProjectName, ProjectTypeSearchOut.Lead)
        };
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(404)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    [InlineData(0)]
    [InlineData(-2147220969)]
    public async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(int failureCode)
    {
        var dataverseFailure = Failure.Create(failureCode, "Some Failure message");
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseFailure);

        var func = CreateFunc(mockDataverseApiClient.Object);
        var actual = await func.InvokeAsync(SomeInput, CancellationToken.None);

        var expected = Failure.Create(ProjectSetSearchFailureCode.Unknown, dataverseFailure.FailureMessage);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task InvokeAsync_DataverseResultIsSuccessEmpty_ExpectSuccessEmpty()
    {
        var dataverseOut = new DataverseSearchOut(4, Array.Empty<DataverseSearchItem>());
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = CreateFunc(mockDataverseApiClient.Object);
        var actualResult = await func.InvokeAsync(SomeInput, CancellationToken.None);

        Assert.True(actualResult.IsSuccess);
        Assert.Empty(actualResult.SuccessOrThrow().Projects);
    }
}
