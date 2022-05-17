using GGroupp.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GGroupp.Internal.Timesheet.TimesheetSet.Get.Api.Test;

partial class TimesheetDeleteFuncTest
{
    [Fact]
    public void InvokeAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var dataverseOut = Unit.Value;
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = new TimesheetDeleteIn(guid1);
        var token = new CancellationToken(canceled: true);

        var valueTask = func.InvokeAsync(input, token);
        Assert.True(valueTask.IsCanceled);
    }

    [Fact]
    public async Task InvokeAsync_CancellationTokenIsNotCanceled_ExpectCallDataVerseApiClientOnce()
    {
        var dataverseOut = Unit.Value;
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var token = new CancellationToken(canceled: false);
        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = new TimesheetDeleteIn(guid2);
        _ = await func.InvokeAsync(input, token);

        var expected = new DataverseEntityDeleteIn(
            "gg_timesheetactivities",
            new DataversePrimaryKey(guid2));
        mockDataverseApiClient.Verify(c => c.DeleteEntityAsync(expected, token), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var mockDataverseApiClient = CreateMockDataverseApiClient(Unit.Value);
        var func = CreateFunc(mockDataverseApiClient.Object);

        var actualResult = await func.InvokeAsync(new TimesheetDeleteIn(guid3), default);

        Assert.True(actualResult.IsSuccess);
        var actual = actualResult.SuccessOrThrow();

        Assert.Equal(Unit.Value, actual);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, TimesheetDeleteFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, TimesheetDeleteFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, TimesheetDeleteFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, TimesheetDeleteFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, TimesheetDeleteFailureCode.NotFound)]
    [InlineData(DataverseFailureCode.UserNotEnabled, TimesheetDeleteFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, TimesheetDeleteFailureCode.NotAllowed)]
    public async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, TimesheetDeleteFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some Failure message");
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseFailure);

        var func = CreateFunc(mockDataverseApiClient.Object);
        var actual = await func.InvokeAsync(new(guid4), CancellationToken.None);

        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);
        Assert.Equal(expected, actual);
    }
}
