using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeepEqual.Syntax;
using GGroupp.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Api.Test;

partial class TimesheetApiTest
{
    [Fact]
    public static void CreateTimesheetAsync_CancellationTokenIsCanceled_ExpectValueTaskIsCanceled()
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonCreateOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var input = SomeTimesheetCreateInput;
        var cancellationToken = new CancellationToken(canceled: true);

        var actual = api.CreateTimesheetAsync(input, cancellationToken);
        Assert.True(actual.IsCanceled);
    }

    [Theory]
    [InlineData(TimesheetProjectType.Lead, "lead", "/leads({0})")]
    [InlineData(TimesheetProjectType.Opportunity, "opportunity", "/opportunities({0})")]
    [InlineData(TimesheetProjectType.Project, "gg_project", "/gg_projects({0})")]
    [InlineData(TimesheetProjectType.Incident, "incident", "/incidents({0})")]
    public static async Task CreateTimesheetAsync_CancellationTokenIsNotCanceled_ExpectCallDataverseApiClientOnce(
        TimesheetProjectType projectType, string projectEntityName, string projectIdTemplate)
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonCreateOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success, IsMatchDataverseInput);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var input = new TimesheetCreateIn(
            date: DateOnly.Parse("2021-10-07"),
            projectId: Guid.Parse("7583b4e6-23f5-eb11-94ef-00224884a588"),
            projectType: projectType,
            duration: 8,
            description: "Some message!");

        var cancellationToken = new CancellationToken(false);
        _ = await api.CreateTimesheetAsync(input, cancellationToken);

        mockDataverseApiClient.Verify(
            c => c.CreateEntityAsync<IReadOnlyDictionary<string, object?>, TimesheetJsonCreateOut>(
                It.IsAny<DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>>>(), cancellationToken),
            Times.Once);

        void IsMatchDataverseInput(DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>> actual)
        {
            var expected = new DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>>(
                entityPluralName: "gg_timesheetactivities",
                selectFields: new("activityid"),
                entityData: new Dictionary<string, object?>
                {
                    ["gg_date"] = "2021-10-07",
                    ["gg_description"] = "Some message!",
                    ["gg_duration"] = 8,
                    [$"regardingobjectid_{projectEntityName}@odata.bind"] = string.Format(projectIdTemplate, "7583b4e6-23f5-eb11-94ef-00224884a588")
                });

            actual.ShouldDeepEqual(expected);
        }
    }

    [Fact]
    public static async Task CreateTimesheetAsync_CallerIdIsNotNull_ExpectCallDataverseImpersonateOnce()
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonCreateOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);
        var callerId = Guid.Parse("b0176902-fd7c-4626-b760-09896592873f");

        var input = SomeTimesheetCreateInput with
        {
            CallerUserId = callerId
        };

        var cancellationToken = new CancellationToken(false);
        _ = await api.CreateTimesheetAsync(input, cancellationToken);

        mockDataverseApiClient.Verify(c => c.Impersonate(callerId), Times.Once);
    }

    [Theory]
    [InlineData(TimesheetChannel.Unknown, 150)]
    [InlineData(TimesheetChannel.Teams, 0)]
    [InlineData(TimesheetChannel.Teams, 100201)]
    [InlineData(TimesheetChannel.Telegram, -571)]
    [InlineData(TimesheetChannel.Telegram, 100205)]
    public static async Task CreateTimesheetAsync_ChannelCodeIsFromConfigurationNotNull_ExpectCallDataverseApiClientWithChannelCodeOnce(
        TimesheetChannel channel, int channelCode)
    {
        var option = new TimesheetApiOption
        {
            ChannelCodes = new KeyValuePair<TimesheetChannel, int?>(channel, channelCode).AsFlatArray()
        };

        var success = new DataverseEntityCreateOut<TimesheetJsonCreateOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success, IsMatchDataverseInput);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, option);

        var input = new TimesheetCreateIn(
            date: SomeTimesheetCreateInput.Date,
            projectId: SomeTimesheetCreateInput.ProjectId,
            projectType: SomeTimesheetCreateInput.ProjectType,
            duration: SomeTimesheetCreateInput.Duration,
            description: SomeTimesheetCreateInput.Description,
            channel: channel);

        var cancellationToken = new CancellationToken(false);
        _ = await api.CreateTimesheetAsync(input, cancellationToken);

        mockDataverseApiClient.Verify(
            c => c.CreateEntityAsync<IReadOnlyDictionary<string, object?>, TimesheetJsonCreateOut>(
                It.IsAny<DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>>>(), cancellationToken),
            Times.Once);

        void IsMatchDataverseInput(DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>> actual)
        {
            var actualCode = actual.EntityData.GetValueOrDefault("gg_timesheetactivity_channel");
            Assert.Equal(channelCode, actualCode);
        }
    }

    [Fact]
    public static async Task CreateTimesheetAsync_ChannelCodeIsFromConfigurationNull_ExpectCallDataverseApiClientWithNoChannelCodeOnce()
    {
        var channel = TimesheetChannel.Teams;

        var option = new TimesheetApiOption
        {
            ChannelCodes = new(
                new(channel, null),
                new(TimesheetChannel.Telegram, 100))
        };

        var success = new DataverseEntityCreateOut<TimesheetJsonCreateOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success, IsMatchDataverseInput);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, option);

        var input = new TimesheetCreateIn(
            date: SomeTimesheetCreateInput.Date,
            projectId: SomeTimesheetCreateInput.ProjectId,
            projectType: SomeTimesheetCreateInput.ProjectType,
            duration: SomeTimesheetCreateInput.Duration,
            description: SomeTimesheetCreateInput.Description,
            channel: channel);

        var cancellationToken = new CancellationToken(false);
        _ = await api.CreateTimesheetAsync(input, cancellationToken);

        mockDataverseApiClient.Verify(
            c => c.CreateEntityAsync<IReadOnlyDictionary<string, object?>, TimesheetJsonCreateOut>(
                It.IsAny<DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>>>(), cancellationToken),
            Times.Once);

        static void IsMatchDataverseInput(DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>> actual)
            =>
            Assert.False(actual.EntityData.ContainsKey("gg_timesheetactivity_channel"));
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, TimesheetCreateFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, TimesheetCreateFailureCode.NotFound)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, TimesheetCreateFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.UserNotEnabled, TimesheetCreateFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.Throttling, TimesheetCreateFailureCode.TooManyRequests)]
    public static async Task CreateTimesheetAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode dataverseFailureCode, TimesheetCreateFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(dataverseFailureCode, "Some failure message");
        var dataverseResult = Result.Failure(dataverseFailure).With<DataverseEntityCreateOut<TimesheetJsonCreateOut>>();

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseResult);
        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var actual = await api.CreateTimesheetAsync(SomeTimesheetCreateInput, CancellationToken.None);
        var expected = Failure.Create(expectedFailureCode, "Some failure message");

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task CreateTimesheetAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var timesheetJsonOut = new TimesheetJsonCreateOut
        {
            TimesheetId = Guid.Parse("1203c0e2-3648-4596-80dd-127fdd2610b6")
        };

        var success = new DataverseEntityCreateOut<TimesheetJsonCreateOut>(timesheetJsonOut);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);
        var actual = await api.CreateTimesheetAsync(SomeTimesheetCreateInput, default);

        var expected = new TimesheetCreateOut
        {
            TimesheetId = Guid.Parse("1203c0e2-3648-4596-80dd-127fdd2610b6")
        };

        Assert.StrictEqual(expected, actual);
    }
}