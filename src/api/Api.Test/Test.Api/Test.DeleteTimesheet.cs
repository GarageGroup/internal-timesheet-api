using System;
using System.Threading;
using System.Threading.Tasks;
using GGroupp.Infra;
using Moq;
using Xunit;

namespace GGroupp.Internal.Timesheet.Api.Test;

partial class TimesheetApiTest
{
    [Fact]
    public static void DeleteTimesheetAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockDataverseApiClient = CreateMockDataverseApiClient(Unit.Value);
        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var cancellationToken = new CancellationToken(canceled: true);
        var valueTask = api.DeleteTimesheetAsync(SomeTimesheetDeleteInput, cancellationToken);

        Assert.True(valueTask.IsCanceled);
    }

    [Fact]
    public static async Task DeleteTimesheetAsync_CancellationTokenIsNotCanceled_ExpectCallDataverseApiClientOnce()
    {
        var mockDataverseApiClient = CreateMockDataverseApiClient(Unit.Value);

        var cancellationToken = new CancellationToken(canceled: false);
        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var input = new TimesheetDeleteIn
        {
            TimesheetId = Guid.Parse("6dc5d4b0-1cce-4c19-bb35-399a37ec492b")
        };

        _ = await api.DeleteTimesheetAsync(input, cancellationToken);

        var expected = new DataverseEntityDeleteIn(
            "gg_timesheetactivities",
            new DataversePrimaryKey(Guid.Parse("6dc5d4b0-1cce-4c19-bb35-399a37ec492b")));

        mockDataverseApiClient.Verify(c => c.DeleteEntityAsync(expected, cancellationToken), Times.Once);
    }

    [Fact]
    public static async Task DeleteTimesheetAsync_CallerIdIsNotNull_ExpectCallDataverseImpersonateOnce()
    {
        var success = new DataverseEntityCreateOut<TimesheetJsonCreateOut>(default);
        var mockDataverseApiClient = CreateMockDataverseApiClient(success);

        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);
        var callerId = Guid.Parse("a9ab2df8-c7fc-48a0-a2ed-2c6f47e51c82");

        var input = SomeTimesheetDeleteInput with
        {
            CallerUserId = callerId
        };

        var cancellationToken = new CancellationToken(false);
        _ = await api.DeleteTimesheetAsync(input, cancellationToken);

        mockDataverseApiClient.Verify(c => c.Impersonate(callerId), Times.Once);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, TimesheetDeleteFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.DuplicateRecord, TimesheetDeleteFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unauthorized, TimesheetDeleteFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, TimesheetDeleteFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, TimesheetDeleteFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, TimesheetDeleteFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, TimesheetDeleteFailureCode.NotFound)]
    [InlineData(DataverseFailureCode.UserNotEnabled, TimesheetDeleteFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, TimesheetDeleteFailureCode.NotAllowed)]
    public static async Task DeleteTimesheetAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, TimesheetDeleteFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some Failure message");
        var dataverseResult = Result.Failure(dataverseFailure).With<Unit>();

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseResult);
        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var actual = await api.DeleteTimesheetAsync(SomeTimesheetDeleteInput, CancellationToken.None);
        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task DeleteTimesheetAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var mockDataverseApiClient = CreateMockDataverseApiClient(Unit.Value);
        var api = CreateApi(mockDataverseApiClient.Object, SomeDate, SomeOption);

        var actual = await api.DeleteTimesheetAsync(SomeTimesheetDeleteInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}