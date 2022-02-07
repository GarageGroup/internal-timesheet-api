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
        =>
        dependency.Map<ITimesheetSetGetFunc>(apiClient => TimesheetSetGetFunc.Create(apiClient));
}