using GGroupp.Infra;
using Moq;
using PrimeFuncPack;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet.ProjectSet.Search.Api.Test;

using IProjectSetSearchFunc = IAsyncValueFunc<ProjectSetSearchIn, Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>>;

public sealed partial class ProjectSetSearchFuncTest
{
    private static readonly DataverseSearchItem SomeDataverseSearchItem
        =
        new(
            searchScore: 18.698789596557617,
            objectId: Guid.Parse("cc1efd36-ceca-eb11-bacc-000d3a47050c"),
            entityName: "opportunity",
            extensionData: default);

    private static readonly ProjectSetSearchIn SomeInput
        =
        new(
            searchText: "Some search project name",
            top: 10);

    private static IProjectSetSearchFunc CreateFunc(IDataverseSearchSupplier dataverseSearchSupplier)
        =>
        Dependency.Of(dataverseSearchSupplier)
        .UseProjectSetSearchApi()
        .Resolve(Mock.Of<IServiceProvider>());

    private static Mock<IDataverseSearchSupplier> CreateMockDataverseApiClient(
    Result<DataverseSearchOut, Failure<int>> result,
    Action<DataverseSearchIn>? callBack = default)
    {
        var mock = new Mock<IDataverseSearchSupplier>();

        var m = mock
            .Setup(s => s.SearchAsync(It.IsAny<DataverseSearchIn>(), It.IsAny<CancellationToken>()))
            .Returns(result.Pipe(ValueTask.FromResult));

        if (callBack is not null)
        {
            m.Callback<DataverseSearchIn, CancellationToken>(
                (@in, _) => callBack.Invoke(@in));
        }

        return mock;
    }
}
