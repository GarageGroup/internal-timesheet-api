using System;
using System.Runtime.CompilerServices;
using GGroupp.Infra;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("GGroupp.Internal.Timesheet.Api.Test")]

namespace GGroupp.Internal.Timesheet;

public static class TimesheetApiDependency
{
    public static Dependency<ITimesheetApi> UseTimesheetApi(this Dependency<IDataverseApiClient, TimesheetApiOption> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<ITimesheetApi>(CreateApi);

        static TimesheetApi CreateApi(IDataverseApiClient apiClient, TimesheetApiOption option)
        {
            ArgumentNullException.ThrowIfNull(apiClient);
            ArgumentNullException.ThrowIfNull(option);

            return new(apiClient, TodayProvider.Instance, option);
        }
    }
}