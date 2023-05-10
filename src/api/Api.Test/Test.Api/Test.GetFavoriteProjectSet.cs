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
    public static void GetFavoriteProjectSetAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var dataverseOut = new DataverseEntitySetGetOut<LastTimesheetItemJson>(SomeLastTimesheetItemJson.AsFlatArray());
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);
        var cancellationToken = new CancellationToken(canceled: true);

        var valueTask = api.GetFavoriteProjectSetAsync(SomeFavoriteProjectSetGetInput, cancellationToken);
        Assert.True(valueTask.IsCanceled);
    }

    [Theory]
    [InlineData(
        "bef33be0-99f5-4018-ba80-3366ec9ec1fd", "2022-02-01", 5,
        "_ownerid_value eq 'bef33be0-99f5-4018-ba80-3366ec9ec1fd' " +
        "and _regardingobjectid_value ne null and gg_date lt 2022-02-02 and gg_date gt 2022-01-27")]
    [InlineData(
        "e0ede566-276c-4d56-b8d7-aed2f411463e", "2022-01-17", null,
        "_ownerid_value eq 'e0ede566-276c-4d56-b8d7-aed2f411463e' and _regardingobjectid_value ne null and gg_date lt 2022-01-18")]
    public static async Task GetFavoriteProjectSetAsync_CancellationTokenIsNotCanceled_ExpectCallDataverseApiClientOnce(
        string userId, string today, int? countTimesheetDays, string expectedFilter)
    {
        var dataverseOut = new DataverseEntitySetGetOut<LastTimesheetItemJson>(SomeLastTimesheetItemJson.AsFlatArray());
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var cancellationToken = new CancellationToken(false);
        var input = new FavoriteProjectSetGetIn
        {
            UserId = Guid.Parse(userId),
            Top = 5
        };

        var todayValue = DateOnly.Parse(today);

        const int countTimesheetItems = 151;
        var option = new TimesheetApiOption
        {
            ChannelCodes = default,
            FavoriteProjectItemsCount = countTimesheetItems,
            FavoriteProjectDaysCount = countTimesheetDays
        };

        var api = CreateApi(mockDataverseApiClient.Object, todayValue, option);
        _ = await api.GetFavoriteProjectSetAsync(input, cancellationToken);

        var expected = new DataverseEntitySetGetIn(
            entityPluralName: "gg_timesheetactivities",
            selectFields: new("gg_date"),
            expandFields: new(
                new("regardingobjectid_incident", new("title")),
                new("regardingobjectid_lead", new("subject", "companyname")),
                new("regardingobjectid_opportunity", new("name")),
                new("regardingobjectid_gg_project", new("gg_name"))),
            filter: expectedFilter,
            orderBy: new(
                new("gg_date", DataverseOrderDirection.Descending),
                new("createdon", DataverseOrderDirection.Descending)),
            top: countTimesheetItems);

        mockDataverseApiClient.Verify(c => c.GetEntitySetAsync<LastTimesheetItemJson>(expected, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task GetFavoriteProjectSetAsync_CallerIdIsNotNull_ExpectCallDataverseImpersonateOnce()
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonCreateOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);
        var callerId = Guid.Parse("5f92ef56-a611-4faa-8ef4-92687038d92a");

        var input = SomeFavoriteProjectSetGetInput with
        {
            CallerUserId = callerId
        };

        var cancellationToken = new CancellationToken(false);
        _ = await api.GetFavoriteProjectSetAsync(input, cancellationToken);

        mockDataverseApiClient.Verify(c => c.Impersonate(callerId), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Throttling, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, FavoriteProjectSetGetFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, FavoriteProjectSetGetFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unknown, FavoriteProjectSetGetFailureCode.Unknown)]
    public static async Task GetFavoriteProjectSetAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, FavoriteProjectSetGetFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some Failure message");
        var dataverseResult = Result.Failure(dataverseFailure).With<DataverseEntitySetGetOut<LastTimesheetItemJson>>();

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseResult);

        var option = SomeOption with
        {
            FavoriteProjectItemsCount = 150,
            FavoriteProjectDaysCount = 7
        };

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, option);

        var actual = await api.GetFavoriteProjectSetAsync(SomeFavoriteProjectSetGetInput, CancellationToken.None);
        var expected = Failure.Create(expectedFailureCode, "Some Failure message");

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(TimesheetApiTestSource.FavoriteProjectSetGetOutputTestData), MemberType = typeof(TimesheetApiTestSource))]
    internal static async Task GetFavoriteProjectSetAsync_DataverseResultIsSuccess_ExpectSuccess(
        DataverseEntitySetGetOut<LastTimesheetItemJson> dataverseOut, int? top, FavoriteProjectSetGetOut expected)
    {
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var option = new TimesheetApiOption
        {
            ChannelCodes = default,
            FavoriteProjectItemsCount = 120,
            FavoriteProjectDaysCount = 15
        };

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, option);

        var input = new FavoriteProjectSetGetIn
        {
            UserId = Guid.Parse("a6de79a6-6a0c-481c-9f5b-c4e7af5ea1be"),
            Top = top
        };

        var actual = await api.GetFavoriteProjectSetAsync(input, default);
        Assert.StrictEqual(expected, actual);
    }
}