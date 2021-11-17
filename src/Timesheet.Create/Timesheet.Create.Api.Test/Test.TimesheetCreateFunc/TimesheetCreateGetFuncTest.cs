using GGroupp.Infra;
using Moq;
using PrimeFuncPack;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet.Create.Api.Test;

using ITimesheetCreateFunc = IAsyncValueFunc<TimesheetCreateIn, Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>>;

public sealed partial class TimesheetCreateGetFuncTest
{
    private static readonly TimesheetCreateIn SomeInput
        =
        new(
            ownerId: Guid.Parse("edd9a08d-8927-ec11-b6e5-6045bd8c1b4d"),
            date: new(2021, 10, 07),
            projectId: Guid.Parse("7583b4e6-23f5-eb11-94ef-00224884a588"),
            projectType: TimesheetProjectType.Project,
            duration: 8,
            description: "Some message!");

    private static ITimesheetCreateFunc CreateFunc(IDataverseEntityCreateSupplier dataverseEntityCreateSupplier)
        =>
        Dependency.Of(dataverseEntityCreateSupplier)
        .UseTimesheetCreateApi()
        .Resolve(Mock.Of<IServiceProvider>());

    private static Mock<IDataverseEntityCreateSupplier> CreateMockDataverseApiClient(
        Result<DataverseEntityCreateOut<TimesheetJsonOut>, Failure<int>> result,
        Action<DataverseEntityCreateIn<Dictionary<string, object?>>>? callback = null)
    {
        var mock = new Mock<IDataverseEntityCreateSupplier>();

        var m = mock.Setup(
            s => s.CreateEntityAsync<Dictionary<string, object?>, TimesheetJsonOut>(
                It.IsAny<DataverseEntityCreateIn<Dictionary<string, object?>>>(), It.IsAny<CancellationToken>()))
            .Returns(result.Pipe(ValueTask.FromResult));

        if (callback is not null)
        {
            m.Callback<DataverseEntityCreateIn<Dictionary<string, object?>>, CancellationToken>(
                (@in, _) => callback.Invoke(@in));
        }

        return mock;
    }
}
