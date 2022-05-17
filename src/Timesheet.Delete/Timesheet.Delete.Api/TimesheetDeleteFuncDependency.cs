using GGroupp.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Internal.Timesheet.TimesheetDelete.Api.Test")]

namespace GGroupp.Internal.Timesheet;

using ITimesheetDeleteFunc = IAsyncValueFunc<TimesheetDeleteIn, Result<Unit, Failure<TimesheetDeleteFailureCode>>>;

public static class TimesheetDeleteFuncDependency
{
    public static Dependency<ITimesheetDeleteFunc> UseTimesheetDeleteApi<TDataverseApiClient>(this Dependency<TDataverseApiClient> dependency)
        where TDataverseApiClient : IDataverseEntityDeleteSupplier
        =>
        dependency.Map<ITimesheetDeleteFunc>(apiClient => TimesheetDeleteFunc.Create(apiClient));
}