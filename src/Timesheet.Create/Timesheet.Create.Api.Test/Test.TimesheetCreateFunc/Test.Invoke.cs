using GGroupp.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Moq;
using DeepEqual.Syntax;

namespace GGroupp.Internal.Timesheet.Create.Api.Test;

partial class TimesheetCreateGetFuncTest
{
    [Fact]
    public void InvokeAsync_CancellationTokenIsCanceled_ExpectValueTaskIsCanceled()
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = SomeInput;
        var token = new CancellationToken(canceled: true);

        var actual = func.InvokeAsync(input, token);
        Assert.True(actual.IsCanceled);
    }

    [Theory]
    [InlineData(TimesheetProjectType.Lead, "lead", "leads")]
    [InlineData(TimesheetProjectType.Opportunity, "opportunity", "opportunities")]
    [InlineData(TimesheetProjectType.Project, "gg_project", "gg_projects")]
    public async Task InvokeAsync_CancellationTokenIsNotCanceled_ExpectCallDataVerseApiClientOnce(
        TimesheetProjectType projectType, string projectEntityName, string projectEntityPluralName)
    {
        const string ownerId = "edd9a08d-8927-ec11-b6e5-6045bd8c1b4d";
        const string date = "2021-10-07";
        const string description = "Some message!";
        const int duration = 8;
        const string projectId = "7583b4e6-23f5-eb11-94ef-00224884a588";

        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(null);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success, IsMatchDataverseInput);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = new TimesheetCreateIn(
            ownerId: Guid.Parse(ownerId),
            date: DateOnly.Parse(date),
            projectId: Guid.Parse(projectId),
            projectType: projectType,
            duration: duration,
            description: description);

        var token = new CancellationToken(false);
        _ = await func.InvokeAsync(input, token);

        mockDataverseApiClient.Verify(
            c => c.CreateEntityAsync<Dictionary<string, object?>, TimesheetJsonOut>(
                It.IsAny<DataverseEntityCreateIn<Dictionary<string, object?>>>(), token),
            Times.Once);

        void IsMatchDataverseInput(DataverseEntityCreateIn<Dictionary<string, object?>> actual)
        {
            var expected = new DataverseEntityCreateIn<Dictionary<string, object?>>(
                entityPluralName: "gg_timesheetactivities",
                selectFields: new[] { "activityid" },
                entityData: new()
                {
                    ["ownerid@odata.bind"] = $"/systemusers({ownerId})",
                    ["gg_date"] = date,
                    ["gg_description"] = description,
                    ["gg_duration"] = duration,
                    [$"regardingobjectid_{projectEntityName}@odata.bind"] = $"/{projectEntityPluralName}({projectId})"
                });

            actual.ShouldDeepEqual(expected);
        }
    }

    [Theory]
    [InlineData(-2147220969, TimesheetCreateFailureCode.NotFound)]
    [InlineData(404, TimesheetCreateFailureCode.Unknown)]
    [InlineData(0, TimesheetCreateFailureCode.Unknown)]
    [InlineData(-2147204326, TimesheetCreateFailureCode.Unknown)]
    public async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        int dataverseFailureCode, TimesheetCreateFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(dataverseFailureCode, "Some failure message");
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseFailure);

        var func = CreateFunc(mockDataverseApiClient.Object);
        var actual = await func.InvokeAsync(SomeInput, CancellationToken.None);

        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task InvokeAsync_DataverseResultIsSuccessNotNullValue_ExpectSuccess()
    {
        var incidentId = Guid.Parse("1203c0e2-3648-4596-80dd-127fdd2610b6");

        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(new() { TimesheetId = incidentId });
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var func = CreateFunc(mockDataverseApiClient.Object);
        var actual = await func.InvokeAsync(SomeInput, default);

        var expected = new TimesheetCreateOut(incidentId);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task InvokeAsync_DataverseResultIsSuccessNullValue_ExpectSuccess()
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(null);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var func = CreateFunc(mockDataverseApiClient.Object);
        var actual = await func.InvokeAsync(SomeInput, CancellationToken.None);

        var expected = new TimesheetCreateOut(default);
        Assert.Equal(expected, actual);
    }
}