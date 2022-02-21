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

        var func = CreateFunc(mockDataverseApiClient.Object, new(default));

        var input = SomeInput;
        var token = new CancellationToken(canceled: true);

        var actual = func.InvokeAsync(input, token);
        Assert.True(actual.IsCanceled);
    }

    [Theory]
    [InlineData(TimesheetProjectType.Lead, "lead", "leads")]
    [InlineData(TimesheetProjectType.Opportunity, "opportunity", "opportunities")]
    [InlineData(TimesheetProjectType.Project, "gg_project", "gg_projects")]
    [InlineData(TimesheetProjectType.Incident, "incident", "incidents")]
    public async Task InvokeAsync_CancellationTokenIsNotCanceled_ExpectCallDataVerseApiClientOnce(
        TimesheetProjectType projectType, string projectEntityName, string projectEntityPluralName)
    {
        const string date = "2021-10-07";
        const string description = "Some message!";
        const int duration = 8;
        const string projectId = "7583b4e6-23f5-eb11-94ef-00224884a588";

        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success, IsMatchDataverseInput);

        var func = CreateFunc(mockDataverseApiClient.Object, new(default));

        var input = new TimesheetCreateIn(
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
                    ["gg_date"] = date,
                    ["gg_description"] = description,
                    ["gg_duration"] = duration,
                    [$"regardingobjectid_{projectEntityName}@odata.bind"] = $"/{projectEntityPluralName}({projectId})"
                });

            actual.ShouldDeepEqual(expected);
        }
    }

    [Theory]
    [InlineData(TimesheetChannel.Unknown, 150)]
    [InlineData(TimesheetChannel.Teams, 0)]
    [InlineData(TimesheetChannel.Teams, 100201)]
    [InlineData(TimesheetChannel.Telegram, -571)]
    [InlineData(TimesheetChannel.Telegram, 100205)]
    public async Task InvokeAsync_ChannelCodeIsFromConfigurationNotNull_ExpectCallDataVerseApiClientWithChannelCodeOnce(
        TimesheetChannel channel, int channelCode)
    {
        var configuration = new TimesheetCreateApiConfiguration(
            new Dictionary<TimesheetChannel, int?>
            {
                [channel] = channelCode
            });

        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success, IsMatchDataverseInput);

        var func = CreateFunc(mockDataverseApiClient.Object, configuration);

        var input = new TimesheetCreateIn(
            date: SomeInput.Date,
            projectId: SomeInput.ProjectId,
            projectType: SomeInput.ProjectType,
            duration: SomeInput.Duration,
            description: SomeInput.Description,
            channel: channel);

        var token = new CancellationToken(false);
        _ = await func.InvokeAsync(input, token);

        mockDataverseApiClient.Verify(
            c => c.CreateEntityAsync<Dictionary<string, object?>, TimesheetJsonOut>(
                It.IsAny<DataverseEntityCreateIn<Dictionary<string, object?>>>(), token),
            Times.Once);

        void IsMatchDataverseInput(DataverseEntityCreateIn<Dictionary<string, object?>> actual)
        {
            var actualCode = actual.EntityData.GetValueOrDefault("gg_timesheetactivity_channel");
            Assert.Equal(channelCode, actualCode);
        }
    }

    [Fact]
    public async Task InvokeAsync_ChannelCodeIsFromConfigurationNull_ExpectCallDataVerseApiClientWithNoChannelCodeOnce()
    {
        var channel = TimesheetChannel.Teams;

        var configuration = new TimesheetCreateApiConfiguration(
            new Dictionary<TimesheetChannel, int?>
            {
                [channel] = null,
                [TimesheetChannel.Telegram] = 100
            });

        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success, IsMatchDataverseInput);

        var func = CreateFunc(mockDataverseApiClient.Object, configuration);

        var input = new TimesheetCreateIn(
            date: SomeInput.Date,
            projectId: SomeInput.ProjectId,
            projectType: SomeInput.ProjectType,
            duration: SomeInput.Duration,
            description: SomeInput.Description,
            channel: channel);

        var token = new CancellationToken(false);
        _ = await func.InvokeAsync(input, token);

        mockDataverseApiClient.Verify(
            c => c.CreateEntityAsync<Dictionary<string, object?>, TimesheetJsonOut>(
                It.IsAny<DataverseEntityCreateIn<Dictionary<string, object?>>>(), token),
            Times.Once);

        static void IsMatchDataverseInput(DataverseEntityCreateIn<Dictionary<string, object?>> actual)
        {
            Assert.False(actual.EntityData.ContainsKey("gg_timesheetactivity_channel"));
        }
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, TimesheetCreateFailureCode.NotFound)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, TimesheetCreateFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.UserNotEnabled, TimesheetCreateFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.Throttling, TimesheetCreateFailureCode.TooManyRequests)]
    public async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode dataverseFailureCode, TimesheetCreateFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(dataverseFailureCode, "Some failure message");
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseFailure);

        var func = CreateFunc(mockDataverseApiClient.Object, new(default));
        var actual = await func.InvokeAsync(SomeInput, CancellationToken.None);

        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var incidentId = Guid.Parse("1203c0e2-3648-4596-80dd-127fdd2610b6");

        var success = new DataverseEntityCreateOut<TimesheetJsonOut>(new() { TimesheetId = incidentId });
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var func = CreateFunc(mockDataverseApiClient.Object, new(default));
        var actual = await func.InvokeAsync(SomeInput, default);

        var expected = new TimesheetCreateOut(incidentId);
        Assert.Equal(expected, actual);
    }
}