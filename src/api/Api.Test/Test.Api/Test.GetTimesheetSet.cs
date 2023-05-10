using System;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;
using Moq;
using Xunit;

namespace GarageGroup.Internal.Timesheet.Api.Test;

partial class TimesheetApiTest
{
    [Fact]
    public static void GetTimesheetSetAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(SomeTimesheetItemJson.AsFlatArray());
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var cancellationToken = new CancellationToken(canceled: true);
        var valueTask = api.GetTimesheetSetAsync(SomeTimesheetSetGetInput, cancellationToken);

        Assert.True(valueTask.IsCanceled);
    }

    [Fact]
    public static async Task GetTimesheetSetAsync_CancellationTokenIsNotCanceled_ExpectCallDataverseApiClientOnce()
    {
        var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(SomeTimesheetItemJson.AsFlatArray());
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var input = new TimesheetSetGetIn(
            userId: Guid.Parse("bd8b8e33-554e-e611-80dc-c4346bad0190"),
            date: new(2022, 02, 05));

        var cancellationToken = new CancellationToken(false);
        _ = await api.GetTimesheetSetAsync(input, cancellationToken);

        var expected = new DataverseEntitySetGetIn(
            entityPluralName: "gg_timesheetactivities",
            selectFields: new("gg_date", "gg_duration", "gg_description"),
            expandFields: new(
                new("regardingobjectid_incident", new("title")),
                new("regardingobjectid_lead", new("subject", "companyname")),
                new("regardingobjectid_opportunity", new("name")),
                new("regardingobjectid_gg_project", new("gg_name"))),
            filter: "_ownerid_value eq 'bd8b8e33-554e-e611-80dc-c4346bad0190' and gg_date eq 2022-02-05",
            orderBy: new DataverseOrderParameter("createdon", DataverseOrderDirection.Ascending).AsFlatArray());

        mockDataverseApiClient.Verify(c => c.GetEntitySetAsync<TimesheetItemJson>(expected, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task GetTimesheetSetAsync_CallerIdIsNotNull_ExpectCallDataverseImpersonateOnce()
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonCreateOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);
        var callerId = Guid.Parse("8d080158-9528-49e8-ac53-ea4a1d81d497");

        var input = SomeTimesheetSetGetInput with
        {
            CallerUserId = callerId
        };

        var cancellationToken = new CancellationToken(false);
        _ = await api.GetTimesheetSetAsync(input, cancellationToken);

        mockDataverseApiClient.Verify(c => c.Impersonate(callerId), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, TimesheetSetGetFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, TimesheetSetGetFailureCode.NotAllowed)]
    public static async Task GetTimesheetSetAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, TimesheetSetGetFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some Failure message");
        var dataverseResult = Result.Failure(dataverseFailure).With<DataverseEntitySetGetOut<TimesheetItemJson>>();

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseResult);
        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var actual = await api.GetTimesheetSetAsync(SomeTimesheetSetGetInput, CancellationToken.None);
        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task GetTimesheetSetAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(
            value: new(
                new()
                {
                    TimesheetId = Guid.Parse("2f65e056-01d5-ec11-a7b5-0022487fa37b"),
                    Date = new(2022, 02, 15, 08, 05, 56, TimeSpan.FromHours(1)),
                    Duration = 1,
                    Description = "Some description",
                    Project = new()
                    {
                        Name = "SomeFirstProjectName"
                    }
                },
                new()
                {
                    TimesheetId = Guid.Parse("a5768c20-04d5-ec11-a7b5-0022487fa37b"),
                    Date = new(2022, 03, 05, 10, 51, 24, TimeSpan.FromHours(3)),
                    Duration = 5,
                    Description = null,
                    Lead = new()
                    {
                        Subject = "Some Lead",
                        CompanyName = "First Company"
                    }
                },
                new()
                {
                    TimesheetId = Guid.Parse("ab3b5ae8-e0ae-47b6-93c7-94a28486135b"),
                    Date = new(2023, 01, 17, default, default, default, default),
                    Duration = 2.5m,
                    Description = null,
                    Lead = new()
                    {
                        Subject = "Some Second Lead",
                        CompanyName = string.Empty
                    }
                },
                new()
                {
                    TimesheetId = Guid.Parse("2ca1400a-0ae9-494b-b96c-315cdc3b0d59"),
                    Date = new(2023, 02, 21, default, default, default, default),
                    Duration = 7,
                    Description = "Some lead description",
                    Lead = new()
                    {
                        Subject = string.Empty,
                        CompanyName = "Third company"
                    }
                },
                new()
                {
                    TimesheetId = Guid.Parse("89ca0e70-9fba-44ad-8e55-36e8eb223d3f"),
                    Date = new(2023, 01, 15, default, default, default, default),
                    Duration = 0,
                    Description = string.Empty
                }));

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);
        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var actual = await api.GetTimesheetSetAsync(SomeTimesheetSetGetInput, default);

        var expected = new TimesheetSetGetOut
        {
            Timesheets = new(
                new(
                    timesheetId : Guid.Parse("2f65e056-01d5-ec11-a7b5-0022487fa37b"),
                    date : new(2022, 02, 15),
                    duration : 1,
                    projectName : "SomeFirstProjectName",
                    description : "Some description"),
                new(
                    timesheetId : Guid.Parse("a5768c20-04d5-ec11-a7b5-0022487fa37b"),
                    date : new(2022, 03, 05),
                    duration : 5,
                    projectName : "Some Lead (First Company)",
                    description : null),
                new(
                    timesheetId : Guid.Parse("ab3b5ae8-e0ae-47b6-93c7-94a28486135b"),
                    date : new(2023, 01, 17),
                    duration : 2.5m,
                    projectName : "Some Second Lead",
                    description : null),
                new(
                    timesheetId : Guid.Parse("2ca1400a-0ae9-494b-b96c-315cdc3b0d59"),
                    date : new(2023, 02, 21),
                    duration : 7,
                    projectName : "(Third company)",
                    description : "Some lead description"),
                new(
                    timesheetId : Guid.Parse("89ca0e70-9fba-44ad-8e55-36e8eb223d3f"),
                    date : new(2023, 01, 15),
                    duration : 0,
                    projectName : null,
                    description : null))
        };

        Assert.StrictEqual(expected, actual);
    }
}