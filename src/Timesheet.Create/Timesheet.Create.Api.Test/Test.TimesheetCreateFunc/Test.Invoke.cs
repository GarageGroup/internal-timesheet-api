using GGroupp.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Moq;

namespace GGroupp.Internal.Timesheet;

partial class TimesheetCreateGetFuncTest
{
    [Fact]
    public async Task InvokeAsync_InputIsNull_ExpectArgumentNullExcepcion()
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => func.InvokeAsync(null!, CancellationToken.None).AsTask());
        Assert.Equal("input", exception.ParamName);
    }

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
    public async Task InvokeAsync_CancellationTokenIsNotCanceled_ExpectCallDataVerseApiClientOnce(TimesheetProjectType projectType, string projectEntityName, string projectEntityPluralName)
    {
        const string ownerId = "edd9a08d-8927-ec11-b6e5-6045bd8c1b4d";
        const string date = "2021-10-07";
        const string description = "Some message!";
        const int duration = 8;
        const string projectId = "7583b4e6-23f5-eb11-94ef-00224884a588";

        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(null);

        var mockDataverseApiClient = CreateMockDataverseApiClient(success, IsMatchDataverseInput);

        var token = new CancellationToken(false);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var _input = new TimeSheetCreateIn(Guid.Parse(ownerId), DateOnly.Parse(date), description, duration, Guid.Parse(projectId), projectType);

        _ = await func.InvokeAsync(_input, token);

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
        }
    }

    [Fact]
    public async Task InvokeAsync_SuccessResultIsGiven_ExpectSuccessResult()
    {
        var incidentId = Guid.Parse("1203c0e2-3648-4596-80dd-127fdd2610b6");

        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(new() { TimesheetId = incidentId });
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var func = CreateFunc(mockDataverseApiClient.Object);
        var actualResult = await func.InvokeAsync(SomeInput, default);

        var expectedSuccess = new TimesheetCreateOut(incidentId);
        Assert.Equal(expectedSuccess, actualResult);
    }

    [Fact]
    public async Task InvokeAsync_SuccessResultIsNull_ExpectSuccessResult()
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(null);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var func = CreateFunc(mockDataverseApiClient.Object);
        var actualResult = await func.InvokeAsync(SomeInput, default);

        var expected = new TimesheetCreateOut(default);
        Assert.Equal(expected, actualResult);
    }
}