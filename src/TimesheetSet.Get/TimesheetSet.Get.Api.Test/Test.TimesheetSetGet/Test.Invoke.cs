using GGroupp.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GGroupp.Internal.Timesheet.TimesheetSet.Get.Api.Test;

partial class TimesheetSetGetFuncTest
{
    [Fact]
    public void InvokeAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var dataverseOut = new DataverseEntitySetGetOut<TimesheetJsonOut>(new[] { SomeTimesheetJsonOut });
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = CreateFunc(mockDataverseApiClient.Object);

        var input = SomeInput;
        var token = new CancellationToken(canceled: true);

        var valueTask = func.InvokeAsync(input, token);
        Assert.True(valueTask.IsCanceled);
    }

    [Fact]
    public async Task InvokeAsync_CancellationTokenIsNotCanceled_ExpectCallDataVerseApiClientOnce()
    {
        var dataverseOut = new DataverseEntitySetGetOut<TimesheetJsonOut>(new[] { SomeTimesheetJsonOut });
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var token = new CancellationToken(false);

        var func = CreateFunc(mockDataverseApiClient.Object);

        const string userId = "bd8b8e33-554e-e611-80dc-c4346bad0190";

        var input = new TimesheetSetGetIn(
            userId: Guid.Parse(userId),
            date: new(2022, 02, 05));

        _ = await func.InvokeAsync(input, token);

        var expected = new DataverseEntitySetGetIn(
            entityPluralName: "gg_timesheetactivities",
            selectFields: ApiNames.SelectedFields,
            filter: $"_ownerid_value eq '{userId}' and createdon gt '2022-02-05' and createdon lt '2022-02-06'",
            orderBy: ApiNames.OrderBy);
        mockDataverseApiClient.Verify(c => c.GetEntitySetAsync<TimesheetJsonOut>(expected, token), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_DataverseResultIsSuccess_ExpectSuccess()
    {
        var firstSomeTimesheetJsonOut = new TimesheetJsonOut()
        {
            Date = new(2022, 02, 15, 08, 05, 56, TimeSpan.FromHours(1)),
            Duration = 1,
            ProjectName = "SomeFirstProjectName",
            Description = "Some description"
        };

        var secondSomeTimesheetJsonOut = new TimesheetJsonOut()
        {
            Date = new(2022, 03, 05, 10, 51, 24, TimeSpan.FromHours(3)),
            Duration = 5,
            ProjectName = "SomeSecondProjectName",
            Description = null
        };

        var dataverseOut = new DataverseEntitySetGetOut<TimesheetJsonOut>(value: new[]
            {
                firstSomeTimesheetJsonOut,
                secondSomeTimesheetJsonOut
            });

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);
        var func = CreateFunc(mockDataverseApiClient.Object);

        var actualResult = await func.InvokeAsync(SomeInput, default);

        Assert.True(actualResult.IsSuccess);
        var actual = actualResult.SuccessOrThrow().Timesheets;

        var expected = new TimesheetSetItemGetOut[]
        {
            new(
                date : new(2022, 02, 15),
                duration : 1,
                projectName : "SomeFirstProjectName",
                description : "Some description"),
            new(
                date : new(2022, 03, 05),
                duration : 5,
                projectName : "SomeSecondProjectName",
                description : null),
        };
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Unknown, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Throttling, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, TimesheetSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, TimesheetSetGetFailureCode.NotAllowed)]
    [InlineData(DataverseFailureCode.PrivilegeDenied, TimesheetSetGetFailureCode.NotAllowed)]
    public async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, TimesheetSetGetFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some Failure message");
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseFailure);

        var func = CreateFunc(mockDataverseApiClient.Object);
        var actual = await func.InvokeAsync(SomeInput, CancellationToken.None);

        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);
        Assert.Equal(expected, actual);
    }
}
