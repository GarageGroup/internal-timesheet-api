using GGroupp.Infra;
using Moq;
using PrimeFuncPack;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet.FavoriteProjectSet.Search.Api.Test;

using IProjectSetGetFunc = IAsyncValueFunc<FavoriteProjectSetGetIn, Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>>;

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

    private static readonly FavoriteProjectSetGetIn SomeInput
        =
        new(
            userGuid: new Guid("63d9e1b7-706b-ea11-a813-000d3a44ad35"),
            top: 5);

    private static IProjectSetGetFunc CreateFunc(IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier, FavoriteProjectSetGetApiConfiguration apiConfiguration)
        =>
        Dependency.Of(dataverseEntitySetGetSupplier)
        .With(apiConfiguration)
        .UseProjectSetGetApi()
        .Resolve(Mock.Of<IServiceProvider>());

    private static Mock<IDataverseEntitySetGetSupplier> CreateMockDataverseApiClient(
        Result<DataverseEntitySetGetOut<TimesheetItemJson>, Failure<DataverseFailureCode>> result,
        Action<DataverseEntitySetGetIn>? callback = default)
    {
        var mock = new Mock<IDataverseEntitySetGetSupplier>();

        var m = mock
            .Setup(s => s.GetEntitySetAsync<TimesheetItemJson>(It.IsAny<DataverseEntitySetGetIn>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Result<DataverseEntitySetGetOut<TimesheetItemJson>, Failure<DataverseFailureCode>>>(result));

        if (callback is not null)
        {
            m.Callback<DataverseEntitySetGetIn, CancellationToken>(
                (@in, _) => callback.Invoke(@in));
        }

        return mock;
    }
}