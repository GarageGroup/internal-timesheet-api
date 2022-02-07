using GGroupp.Infra;
using Moq;
using PrimeFuncPack;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet.TimesheetSet.Get.Api.Test.Test;

using ITimesheetSetGetFunc = IAsyncValueFunc<TimesheetSetGetIn, Result<TimesheetSetGetOut, Failure<TimesheetSetGetFailureCode>>>;

public sealed partial class TimesheetSetGetFuncTest
{
    private static readonly TimesheetJsonOut SomeTimesheetJsonOut
        =
        new() 
        {
            Date = new(2022, 02, 07, 01, 01, 01, default),
            Duration =  8,
            ProjectName = "Lukoil",
            Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit."
        };

    private static readonly TimesheetSetGetIn SomeInput
        =
        new(
            userId: Guid.Parse("bd8b8e33-554e-e611-80dc-c4346bad0190"),
            date: new(2022, 02, 07));

    private static ITimesheetSetGetFunc CreateFunc(IDataverseEntitySetGetSupplier dataverseSearchSupplier)
        =>
        Dependency.Of(dataverseSearchSupplier)
        .UseTimesheetSetGetApi()
        .Resolve(Mock.Of<IServiceProvider>());

    private static Mock<IDataverseEntitySetGetSupplier> CreateMockDataverseApiClient(
        Result<DataverseEntitySetGetOut<TimesheetJsonOut>, Failure<DataverseFailureCode>> result,
        Action<DataverseEntitySetGetIn>? callback = default)
    {
        var mock = new Mock<IDataverseEntitySetGetSupplier>();

        var m = mock
            .Setup(s => s.GetEntitySetAsync<TimesheetJsonOut>(It.IsAny<DataverseEntitySetGetIn>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Result<DataverseEntitySetGetOut<TimesheetJsonOut>, Failure<DataverseFailureCode>>>(result));

        if (callback is not null)
        {
            m.Callback<DataverseEntitySetGetIn, CancellationToken>(
                (@in, _) => callback.Invoke(@in));
        }

        return mock;
    }
}
