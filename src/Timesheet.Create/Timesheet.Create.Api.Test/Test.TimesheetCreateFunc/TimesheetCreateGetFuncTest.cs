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
            date: new(2021, 10, 07),
            projectId: Guid.Parse("7583b4e6-23f5-eb11-94ef-00224884a588"),
            projectType: TimesheetProjectType.Project,
            duration: 8,
            description: "Some message!",
            channel: TimesheetChannel.Emulator);

    private static ITimesheetCreateFunc CreateFunc(
        IDataverseEntityCreateSupplier dataverseEntityCreateSupplier,
        TimesheetCreateApiConfiguration apiConfiguration)
        =>
        Dependency.Of(dataverseEntityCreateSupplier)
        .With(apiConfiguration)
        .UseTimesheetCreateApi()
        .Resolve(Mock.Of<IServiceProvider>());

    private static Mock<IDataverseEntityCreateSupplier> CreateMockDataverseApiClient(
        Result<DataverseEntityCreateOut<TimesheetJsonOut>, Failure<DataverseFailureCode>> result,
        Action<DataverseEntityCreateIn<Dictionary<string, object?>>>? callback = null)
    {
        var mock = new Mock<IDataverseEntityCreateSupplier>();

        var m = mock.Setup(
            s => s.CreateEntityAsync<Dictionary<string, object?>, TimesheetJsonOut>(
                It.IsAny<DataverseEntityCreateIn<Dictionary<string, object?>>>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Result<DataverseEntityCreateOut<TimesheetJsonOut>, Failure<DataverseFailureCode>>>(result));

        if (callback is not null)
        {
            m.Callback<DataverseEntityCreateIn<Dictionary<string, object?>>, CancellationToken>(
                (@in, _) => callback.Invoke(@in));
        }

        return mock;
    }
}
