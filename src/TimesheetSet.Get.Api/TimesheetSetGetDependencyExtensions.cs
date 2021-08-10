#nullable enable

using System;
using GGroupp.Infra;
using PrimeFuncPack;

namespace GGroupp.Internal.Timesheet
{
    using ITimesheetSetGetFunc = IAsyncValueFunc<TimesheetSetGetIn, Result<TimesheetSetGetOut, Failure<Unit>>>;

    public static class TimesheetSetGetDependencyExtensions
    {
        public static Dependency<ITimesheetSetGetFunc> UseTimesheetSetGetApi<TDataverseEntitySetGetSupplier>(
            this Dependency<TDataverseEntitySetGetSupplier> dependency)
            where TDataverseEntitySetGetSupplier : IDataverseEntitySetGetSupplier
            =>
            dependency.Map<ITimesheetSetGetFunc>(s => TimesheetSetGetFunc.Create(s));
    }
}