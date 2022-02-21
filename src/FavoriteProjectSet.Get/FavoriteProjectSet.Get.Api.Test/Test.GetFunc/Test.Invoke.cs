using DeepEqual.Syntax;
using GGroupp.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GGroupp.Internal.Timesheet.FavoriteProjectSet.Search.Api.Test;

partial class FavoriteProjectSetGetFuncTest
{
    [Fact]
    public void InvokeAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(new[] { SomeProjectItemOut });
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var mockTodayProvider = CreateMockTodayProvider(SomeDate);
        var func = CreateFunc(mockDataverseApiClient.Object, mockTodayProvider.Object, new(50, 7));

        var input = SomeInput;
        var token = new CancellationToken(canceled: true);

        var valueTask = func.InvokeAsync(input, token);
        Assert.True(valueTask.IsCanceled);
    }

    [Theory]
    [InlineData(
        "bef33be0-99f5-4018-ba80-3366ec9ec1fd", "2022-02-01", 5,
        "_ownerid_value eq 'bef33be0-99f5-4018-ba80-3366ec9ec1fd' and _regardingobjectid_value ne null and gg_date lt 2022-02-02 and gg_date gt 2022-01-27")]
    [InlineData(
        "e0ede566-276c-4d56-b8d7-aed2f411463e", "2022-01-17", null,
        "_ownerid_value eq 'e0ede566-276c-4d56-b8d7-aed2f411463e' and _regardingobjectid_value ne null and gg_date lt 2022-01-18")]
    public async Task InvokeAsync_CancellationTokenIsNotCanceled_ExpectCallDataverseApiClientOnce(
        string userId, string today, int? countTimesheetDays, string expectedFilter)
    {
        var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(new[] { SomeProjectItemOut });
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var token = new CancellationToken(false);
        var input = new FavoriteProjectSetGetIn(Guid.Parse(userId), 5);

        var todayValue = DateOnly.Parse(today);
        var mockTodayProvider = CreateMockTodayProvider(todayValue);

        const int countTimesheetItems = 151;
        var configuration = new FavoriteProjectSetGetApiConfiguration(countTimesheetItems, countTimesheetDays);

        var func = CreateFunc(mockDataverseApiClient.Object, mockTodayProvider.Object, configuration);
        _ = await func.InvokeAsync(input, token);

        var expected = new DataverseEntitySetGetIn(
            entityPluralName: ApiNames.TimesheetEntityPluralName,
            orderBy: ApiNames.OrderFiels,
            selectFields: ApiNames.AllFields,
            filter: expectedFilter,
            top: countTimesheetItems)
        {
            IncludeAnnotations = "*"
        };
        mockDataverseApiClient.Verify(c => c.GetEntitySetAsync<TimesheetItemJson>(expected, token), Times.Once);
    }

    [Theory]
    [MemberData(nameof(TestDataSource.GetResponseTestData), MemberType = typeof(TestDataSource))]
    internal async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess(
        DataverseEntitySetGetOut<TimesheetItemJson> dataverseOut, int? top, FavoriteProjectSetGetOut expected)
    {
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);
        var mockTodayProvider = CreateMockTodayProvider(SomeDate);

        var configuration = new FavoriteProjectSetGetApiConfiguration(
            countTimesheetItems: 120,
            countTimesheetDays: 11);

        var func = CreateFunc(mockDataverseApiClient.Object, mockTodayProvider.Object, configuration);

        var input = new FavoriteProjectSetGetIn(Guid.Parse("a6de79a6-6a0c-481c-9f5b-c4e7af5ea1be"), top);
        var actualResult = await func.InvokeAsync(input, default);

        Assert.True(actualResult.IsSuccess);
        var actual = actualResult.SuccessOrThrow();

        expected.ShouldDeepEqual(actual);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Throttling, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, FavoriteProjectSetGetFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, FavoriteProjectSetGetFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unknown, FavoriteProjectSetGetFailureCode.Unknown)]
    public async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, FavoriteProjectSetGetFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some Failure message");
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseFailure);

        var mockTodayProvider = CreateMockTodayProvider(SomeDate);
        var configuration = new FavoriteProjectSetGetApiConfiguration(150, 7);

        var func = CreateFunc(mockDataverseApiClient.Object, mockTodayProvider.Object, configuration);
        var actual = await func.InvokeAsync(SomeInput, CancellationToken.None);

        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);
        Assert.Equal(expected, actual);
    }
}