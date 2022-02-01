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
        Func<IServiceProvider, TimesheetCreateApiConfiguration> configurationResolver)
        where TDataverseApiClient : IDataverseEntityCreateSupplier
        =>
        InnerUseTimesheetCreateApi(
            dependency ?? throw new ArgumentNullException(nameof(dependency)),
            configurationResolver ?? throw new ArgumentNullException(nameof(configurationResolver)));

    public static Dependency<ITimesheetCreateFunc> UseTimesheetCreateApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient, TimesheetCreateApiConfiguration> dependency)
        where TDataverseApiClient : IDataverseEntityCreateSupplier
        =>
        InnerUseTimesheetCreateApi(
            dependency ?? throw new ArgumentNullException(nameof(dependency)));

    private static Dependency<ITimesheetCreateFunc> InnerUseTimesheetCreateApi<TDataverseApiClient>(
        Dependency<TDataverseApiClient> dependency,
        Func<IServiceProvider, TimesheetCreateApiConfiguration> configurationResolver)
        where TDataverseApiClient : IDataverseEntityCreateSupplier
        =>
        dependency.With(configurationResolver).InnerUseTimesheetCreateApi();

    private static Dependency<ITimesheetCreateFunc> InnerUseTimesheetCreateApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient, TimesheetCreateApiConfiguration> dependency)
        where TDataverseApiClient : IDataverseEntityCreateSupplier
        =>
        dependency.Fold<ITimesheetCreateFunc>(
            (apiClient, configuration) => TimesheetCreateGetFunc.Create(apiClient, configuration));
}