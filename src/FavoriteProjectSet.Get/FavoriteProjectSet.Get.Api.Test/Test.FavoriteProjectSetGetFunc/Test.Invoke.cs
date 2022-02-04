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

        var func = CreateFunc(mockDataverseApiClient.Object, new FavoriteProjectSetGetApiConfiguration(countTimesheetItems: 50, countTimesheetDays: 7));

        var input = SomeInput;
        var token = new CancellationToken(canceled: true);

        var valueTask = func.InvokeAsync(input, token);
        Assert.True(valueTask.IsCanceled);
    }

    //[Fact]
    //public async Task InvokeAsync_CancellationTokenIsNotCanceledAndInputIsDefault_ExpectCallDataVerseApiClientOnce()
    //{

    //    var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(new[] { SomeProjectItemOut });
    //    var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

    //    var token = new CancellationToken(false);

    //    const string userGuid = "c7624366-1493-47ee-9f23-ac3fde28bbe2";
    //    var input = new FavoriteProjectSetGetIn(
    //        userGuid: Guid.Parse(userGuid),
    //        top: 5);

    //    var configuration = new FavoriteProjectSetGetApiConfiguration(countTimesheetItems: 150, countTimesheetDays: 7);
    //    var func = CreateFunc(mockDataverseApiClient.Object, configuration);
    //    _ = await func.InvokeAsync(default, token);

    //    var expected = new DataverseEntitySetGetIn(
    //        entityPluralName: ApiNames.TimesheetEntityPluralName,
    //        orderBy: new[] { new DataverseOrderParameter(ApiNames.DateField, DataverseOrderDirection.Descending) },
    //        selectFields: ApiNames.AllFields,
    //        filter: $"{ApiNames.OwnerIdField} eq '{userGuid}' and {ApiNames.DateField} ge {DateTime.Today.AddDays(configuration.CountTimesheetDays.GetValueOrDefault() * -1):yyyy-MM-dd}",
    //        top: configuration.CountTimesheetItems);
    //    mockDataverseApiClient.Verify(c => c.GetEntitySetAsync<TimesheetItemJson>(expected, token), Times.Once);
    //}

    [Fact]
    public async Task InvokeAsync_DataverseResultIsSuccessNotEmpty_ExpectSuccessNotEmpty()
    {
        const string firstProjectId = "c7624366-1493-47ee-9f23-ac3fde28bbe2";
        const string firstProjectName = "X5";
        const string firstProjectType = "gg_project";
        var firstDate = DateTime.Today;
        var dataverseItem1 = new TimesheetItemJson() 
        {
            TimesheetProjectId = Guid.Parse(firstProjectId),
            TimesheetProjectName = firstProjectName,
            TimesheetProjectType = firstProjectType,
            TimesheetDate = firstDate,
        };

        var dataverseItem2 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse(firstProjectId),
            TimesheetProjectName = firstProjectName,
            TimesheetProjectType = firstProjectType,
            TimesheetDate = firstDate,
        };

        var dataverseItem3 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse(firstProjectId),
            TimesheetProjectName = firstProjectName,
            TimesheetProjectType = firstProjectType,
            TimesheetDate = firstDate,
        };

        var dataverseItem4 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse(firstProjectId),
            TimesheetProjectName = firstProjectName,
            TimesheetProjectType = firstProjectType,
            TimesheetDate = firstDate,
        };

        var dataverseItem5 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse(firstProjectId),
            TimesheetProjectName = firstProjectName,
            TimesheetProjectType = firstProjectType,
            TimesheetDate = firstDate,
        };

        var dataverseItem6 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse(firstProjectId),
            TimesheetProjectName = firstProjectName,
            TimesheetProjectType = firstProjectType,
            TimesheetDate = firstDate,
        };

        var dataverseItem7 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse(firstProjectId),
            TimesheetProjectName = firstProjectName,
            TimesheetProjectType = firstProjectType,
            TimesheetDate = firstDate,
        };

        var dataverseItem8 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse(firstProjectId),
            TimesheetProjectName = firstProjectName,
            TimesheetProjectType = firstProjectType,
            TimesheetDate = firstDate,
        };


        var dataverseItem9 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse("26c8c7e7-264e-4be8-8621-0da780663ab1"),
            TimesheetProjectName = "Timesheet",
            TimesheetProjectType = firstProjectType,
            TimesheetDate = firstDate,
        };

        var dataverseItem10 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse("26c8c7e7-264e-4be8-8621-0da780663ab1"),
            TimesheetProjectName = "Timesheet",
            TimesheetProjectType = firstProjectType,
            TimesheetDate = new DateTime(2020, 01, 01),
        };

        var dataverseItem11 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse("25c8c7e7-264e-4be8-8621-0da780663ab1"),
            TimesheetProjectName = "TestProject1",
            TimesheetProjectType = firstProjectType,
            TimesheetDate = new DateTime(2022, 01, 01),
        };

        var dataverseItem12 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse("25c8c7e7-264e-4be8-8621-0da780663ab2"),
            TimesheetProjectName = "TestProject2",
            TimesheetProjectType = firstProjectType,
            TimesheetDate = new DateTime(2022, 01, 01),
        };

        var dataverseItem13 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse("25c8c7e7-264e-4be8-8621-0da780663ab3"),
            TimesheetProjectName = "TestProject3",
            TimesheetProjectType = firstProjectType,
            TimesheetDate = new DateTime(2022, 01, 01),
        };

        var dataverseItem14 = new TimesheetItemJson()
        {
            TimesheetProjectId = Guid.Parse("25c8c7e7-264e-4be8-8621-0da780663ab4"),
            TimesheetProjectName = "TestProject4",
            TimesheetProjectType = firstProjectType,
            TimesheetDate = new DateTime(2020, 01, 01),
        };

        var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(value: new[]
            {
                dataverseItem1,
                dataverseItem2,
                dataverseItem3,
                dataverseItem4,
                dataverseItem5,
                dataverseItem6,
                dataverseItem7,
                dataverseItem8,
                dataverseItem9,
                dataverseItem10,
                dataverseItem11,
                dataverseItem12,
                dataverseItem13,
                dataverseItem14,
            });

        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);
        var func = CreateFunc(mockDataverseApiClient.Object, new FavoriteProjectSetGetApiConfiguration(countTimesheetItems: 50, countTimesheetDays: 7));

        var actualResult = await func.InvokeAsync(SomeInput, default);

        Assert.True(actualResult.IsSuccess);
        var actual = actualResult.SuccessOrThrow().Projects;

        var expected = new FavoriteProjectItemGetOut[]
        {
            new(
                id: Guid.Parse(firstProjectId),
                name: firstProjectName,
                type: TimesheetProjectType.Project),
            new(
                id: Guid.Parse("26c8c7e7-264e-4be8-8621-0da780663ab1"),
                name: "Timesheet",
                type: TimesheetProjectType.Project),
            new(
                id: Guid.Parse("25c8c7e7-264e-4be8-8621-0da780663ab1"),
                name: "TestProject1",
                type: TimesheetProjectType.Project),
            new(
                id: Guid.Parse("25c8c7e7-264e-4be8-8621-0da780663ab2"),
                name: "TestProject2",
                type: TimesheetProjectType.Project),
            new(
                id: Guid.Parse("25c8c7e7-264e-4be8-8621-0da780663ab3"),
                name: "TestProject3",
                type: TimesheetProjectType.Project),
        };
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(DataverseFailureCode.Throttling, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.UserNotEnabled, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.SearchableEntityNotFound, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.PicklistValueOutOfRange, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.RecordNotFound, FavoriteProjectSetGetFailureCode.Unknown)]
    [InlineData(DataverseFailureCode.Unknown, FavoriteProjectSetGetFailureCode.Unknown)]
    public async Task InvokeAsync_DataverseResultIsFailure_ExpectFailure(
        DataverseFailureCode sourceFailureCode, FavoriteProjectSetGetFailureCode expectedFailureCode)
    {
        var dataverseFailure = Failure.Create(sourceFailureCode, "Some Failure message");
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseFailure);

        var func = CreateFunc(mockDataverseApiClient.Object, new FavoriteProjectSetGetApiConfiguration(countTimesheetItems: 50, countTimesheetDays: 7));
        var actual = await func.InvokeAsync(SomeInput, CancellationToken.None);

        var expected = Failure.Create(expectedFailureCode, dataverseFailure.FailureMessage);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task InvokeAsync_DataverseResultIsSuccessEmpty_ExpectSuccessEmpty()
    {
        var dataverseOut = new DataverseEntitySetGetOut<TimesheetItemJson>(Array.Empty<TimesheetItemJson>());
        var mockDataverseApiClient = CreateMockDataverseApiClient(dataverseOut);

        var func = CreateFunc(mockDataverseApiClient.Object, new FavoriteProjectSetGetApiConfiguration(countTimesheetItems: 50, countTimesheetDays: 7));
        var actualResult = await func.InvokeAsync(SomeInput, CancellationToken.None);

        Assert.True(actualResult.IsSuccess);
        Assert.Empty(actualResult.SuccessOrThrow().Projects);
    }

}