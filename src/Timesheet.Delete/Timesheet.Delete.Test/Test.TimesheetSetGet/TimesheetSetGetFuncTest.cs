using GGroupp.Infra;
using Moq;
using PrimeFuncPack;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet.TimesheetSet.Get.Api.Test;

using ITimesheetDeleteFunc = IAsyncValueFunc<TimesheetDeleteIn, Result<Unit, Failure<TimesheetDeleteFailureCode>>>;

public sealed partial class TimesheetDeleteFuncTest
{   
    private static ITimesheetDeleteFunc CreateFunc(IDataverseEntityDeleteSupplier dataverseDeleteSupplier)
        =>
        Dependency.Of(dataverseDeleteSupplier)
        .UseTimesheetDeleteApi()
        .Resolve(Mock.Of<IServiceProvider>());

    private static Mock<IDataverseEntityDeleteSupplier> CreateMockDataverseApiClient(
        Result<Unit, Failure<DataverseFailureCode>> result)
    {
        var mock = new Mock<IDataverseEntityDeleteSupplier>();

        var m = mock
            .Setup(s => s.DeleteEntityAsync(It.IsAny<DataverseEntityDeleteIn>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Result<Unit, Failure<DataverseFailureCode>>>(result));

        return mock;
    }
}
