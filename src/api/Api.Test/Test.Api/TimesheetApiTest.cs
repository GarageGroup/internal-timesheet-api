using System;
using System.Collections.Generic;
using System.Threading;
using GGroupp.Infra;
using Moq;

namespace GGroupp.Internal.Timesheet.Api.Test;

public static partial class TimesheetApiTest
{
    private static readonly LastTimesheetItemJson SomeLastTimesheetItemJson
        =
        new()
        {
            Project = new()
            {
                Id = Guid.Parse("63d9e1b7-706b-ea11-a813-000d3a44ad35"),
                Name = "Some project name"
            },
            TimesheetDate = new(2022, 01, 15)
        };

    private static readonly TimesheetItemJson SomeTimesheetItemJson
        =
        new() 
        {
            Date = new(2022, 02, 07, 01, 01, 01, default),
            Duration =  8,
            Opportunity = new()
            {
                Id = Guid.Parse("ba0d9b46-9a09-4196-8b1a-d69e1a28d7d2"),
                Name = "Some Opportunity"
            },
            Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit."
        };

    private static readonly DataverseSearchItem SomeDataverseSearchItem
        =
        new(
            searchScore: 18.698789596557617,
            objectId: Guid.Parse("cc1efd36-ceca-eb11-bacc-000d3a47050c"),
            entityName: "opportunity",
            extensionData: default);

    private static readonly DateOnly SomeDate
        =
        new(2021, 11, 21);

    private static readonly TimesheetApiOption SomeOption
        =
        new()
        {
            ChannelCodes = new KeyValuePair<TimesheetChannel, int?>(TimesheetChannel.Teams, 167001).AsFlatArray()
        };

    private static readonly FavoriteProjectSetGetIn SomeFavoriteProjectSetGetInput
        =
        new()
        {
            UserId = Guid.Parse("a93ca280-e910-474a-a6a4-e50b5d38ade7"),
            Top = 5
        };

    private static readonly TimesheetSetGetIn SomeTimesheetSetGetInput
        =
        new(
            userId: Guid.Parse("bd8b8e33-554e-e611-80dc-c4346bad0190"),
            date: new(2022, 02, 07));

    private static readonly ProjectSetSearchIn SomeProjectSetSearchInput
        =
        new("Some search project name")
        {
            Top = 10
        };

    private static readonly TimesheetCreateIn SomeTimesheetCreateInput
        =
        new(
            date: new(2021, 10, 07),
            projectId: Guid.Parse("7583b4e6-23f5-eb11-94ef-00224884a588"),
            projectType: TimesheetProjectType.Project,
            duration: 8,
            description: "Some message!",
            channel: TimesheetChannel.Emulator);

    private static readonly TimesheetDeleteIn SomeTimesheetDeleteInput
        =
        new()
        {
            TimesheetId = Guid.Parse("27ad04f0-c127-48ab-892f-26647745939c")
        };

    private static TimesheetApi CreateApi(IDataverseApiClient apiClient, DateOnly todayDate, TimesheetApiOption option)
    {
        var todayProvider = Mock.Of<ITodayProvider>(p => p.GetToday() == todayDate);
        return new(apiClient, todayProvider, option);
    }

    private static Mock<IDataverseApiClient> CreateMockDataverseApiClient(
        Result<DataverseEntitySetGetOut<LastTimesheetItemJson>, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseApiClient>();

        _ = mock
            .Setup(s => s.GetEntitySetAsync<LastTimesheetItemJson>(It.IsAny<DataverseEntitySetGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock.SetupImpersonation();
    }

    private static Mock<IDataverseApiClient> CreateMockDataverseApiClient(
        Result<DataverseEntitySetGetOut<TimesheetItemJson>, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseApiClient>();

        _ = mock
            .Setup(s => s.GetEntitySetAsync<TimesheetItemJson>(It.IsAny<DataverseEntitySetGetIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock.SetupImpersonation();
    }

    private static Mock<IDataverseApiClient> CreateMockDataverseApiClient(
        Result<DataverseSearchOut, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseApiClient>();

        _ = mock
            .Setup(s => s.SearchAsync(It.IsAny<DataverseSearchIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock.SetupImpersonation();
    }

    private static Mock<IDataverseApiClient> CreateMockDataverseApiClient(
        Result<DataverseEntityCreateOut<TimesheetJsonCreateOut>, Failure<DataverseFailureCode>> result,
        Action<DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>>>? callback = null)
    {
        var mock = new Mock<IDataverseApiClient>();

        var m = mock.Setup(
            s => s.CreateEntityAsync<IReadOnlyDictionary<string, object?>, TimesheetJsonCreateOut>(
                It.IsAny<DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        if (callback is not null)
        {
            m.Callback<DataverseEntityCreateIn<IReadOnlyDictionary<string, object?>>, CancellationToken>(
                (@in, _) => callback.Invoke(@in));
        }

        return mock.SetupImpersonation();
    }

    private static Mock<IDataverseApiClient> CreateMockDataverseApiClient(
        Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseApiClient>();

        var m = mock
            .Setup(s => s.DeleteEntityAsync(It.IsAny<DataverseEntityDeleteIn>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        return mock.SetupImpersonation();
    }

    private static Mock<IDataverseApiClient> SetupImpersonation(this Mock<IDataverseApiClient> mockDataverseApiClient)
    {
        _ = mockDataverseApiClient.Setup(s => s.Impersonate(It.IsAny<Guid>())).Returns(mockDataverseApiClient.Object);
        return mockDataverseApiClient;
    }
}