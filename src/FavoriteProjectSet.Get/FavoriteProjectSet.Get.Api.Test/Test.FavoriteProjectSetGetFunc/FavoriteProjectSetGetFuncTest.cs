using GGroupp.Infra;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet.FavoriteProjectSet.Search.Api.Test;

public sealed partial class FavoriteProjectSetGetFuncTest
{
    private static readonly TimesheetItemJson SomeProjectItemOut
        =
        new()
        {
            TimesheetProjectId = Guid.Parse("63d9e1b7-706b-ea11-a813-000d3a44ad35"),
            TimesheetProjectName = "X5",
            TimesheetProjectType = "gg_project",
            TimesheetDate = new(2022, 01, 15)
        };

    private static readonly DateOnly SomeDate
        =
        new(2021, 11, 21);

    private static readonly FavoriteProjectSetGetIn SomeInput
        =
        new(
            userId: Guid.Parse("a93ca280-e910-474a-a6a4-e50b5d38ade7"),
            top: 5);

    private static FavoriteProjectSetGetFunc CreateFunc(
        IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier,
        ITodayProvider todayProvider,
        FavoriteProjectSetGetApiConfiguration apiConfiguration)
        =>
        FavoriteProjectSetGetFunc.InternalCreate(
            dataverseEntitySetGetSupplier, todayProvider, apiConfiguration);

    private static Mock<IDataverseEntitySetGetSupplier> CreateMockDataverseApiClient(
        Result<DataverseEntitySetGetOut<TimesheetItemJson>, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntitySetGetSupplier>();

        _ = mock
            .Setup(s => s.GetEntitySetAsync<TimesheetItemJson>(It.IsAny<DataverseEntitySetGetIn>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Result<DataverseEntitySetGetOut<TimesheetItemJson>, Failure<DataverseFailureCode>>>(result));

        return mock;
    }

    private static Mock<ITodayProvider> CreateMockTodayProvider(DateOnly today)
    {
        var mock = new Mock<ITodayProvider>();

        _ = mock.Setup(p => p.GetToday()).Returns(today);

        return mock;
    }
}