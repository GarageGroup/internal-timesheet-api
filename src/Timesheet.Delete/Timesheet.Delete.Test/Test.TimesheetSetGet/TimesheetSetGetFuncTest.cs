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
    private static readonly Guid guid1 = Guid.Parse("27ad04f0-c127-48ab-892f-26647745939c");

    private static readonly Guid guid2 = Guid.Parse("6dc5d4b0-1cce-4c19-bb35-399a37ec492b");
    
    private static readonly Guid guid3 = Guid.Parse("e861401d-fae4-4ebc-a6ad-fbf98c482b19");
    
    private static readonly Guid guid4 = Guid.Parse("e679fa77-3d52-40a7-8faf-eb54ed25ade7");
    
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
