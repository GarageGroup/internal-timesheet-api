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
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Map<ITimesheetDeleteFunc>(CreateFunc);

        static TimesheetDeleteFunc CreateFunc(TDataverseApiClient apiClient)
            =>
            TimesheetDeleteFunc.Create(apiClient);
    }
}