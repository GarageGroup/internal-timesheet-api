using GGroupp.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Internal.Timesheet.Timesheet.Create.Api.Test")]

namespace GGroupp.Internal.Timesheet;

using ITimesheetCreateFunc = IAsyncValueFunc<TimesheetCreateIn, Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>>;

public static class TimesheetCreateFuncDependency
{
    public static Dependency<ITimesheetCreateFunc> UseTimesheetCreateApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient> dependency,
        Func<IServiceProvider, TimesheetCreateApiOption> optionResolver)
        where TDataverseApiClient : IDataverseEntityCreateSupplier
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).Fold<ITimesheetCreateFunc>(CreateTimesheetCreateFunc);
    }

    public static Dependency<ITimesheetCreateFunc> UseTimesheetCreateApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient, TimesheetCreateApiOption> dependency)
        where TDataverseApiClient : IDataverseEntityCreateSupplier
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold<ITimesheetCreateFunc>(CreateTimesheetCreateFunc);
    }

    private static TimesheetCreateFunc CreateTimesheetCreateFunc<TDataverseApiClient>(
        TDataverseApiClient apiClient, TimesheetCreateApiOption option)
        where TDataverseApiClient : IDataverseEntityCreateSupplier
        =>
        TimesheetCreateFunc.Create(apiClient, option);
}