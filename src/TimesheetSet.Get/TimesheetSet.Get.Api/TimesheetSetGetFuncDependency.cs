using GGroupp.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Internal.Timesheet.TimesheetSet.Get.Api.Test")]

namespace GGroupp.Internal.Timesheet;

using ITimesheetSetGetFunc = IAsyncValueFunc<TimesheetSetGetIn, Result<TimesheetSetGetOut, Failure<TimesheetSetGetFailureCode>>>;

public static class TimesheetSetGetFuncDependency
{
    public static Dependency<ITimesheetSetGetFunc> UseTimesheetSetGetApi<TDataverseApiClient>(this Dependency<TDataverseApiClient> dependency)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Map<ITimesheetSetGetFunc>(CreateFunc);

        static TimesheetSetGetFunc CreateFunc(TDataverseApiClient apiClient)
            =>
            TimesheetSetGetFunc.Create(apiClient);
    }
}