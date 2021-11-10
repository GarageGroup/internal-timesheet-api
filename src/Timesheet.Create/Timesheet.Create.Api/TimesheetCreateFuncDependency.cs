using GGroupp.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Internal.Timesheet.Test")]

namespace GGroupp.Internal.Timesheet;

using ITimesheetCreateFunc = IAsyncValueFunc<TimeSheetCreateIn, Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>>;

public static class TimesheetCreateFuncDependency
{
    public static Dependency<ITimesheetCreateFunc> UseTimesheetCreateApi<TDataverseApiClient>(this Dependency<TDataverseApiClient> dependency)
        where TDataverseApiClient : IDataverseEntityCreateSupplier
        =>
        dependency.Map<ITimesheetCreateFunc>(apiClient => TimesheetCreateGetFunc.Create(apiClient));
}
