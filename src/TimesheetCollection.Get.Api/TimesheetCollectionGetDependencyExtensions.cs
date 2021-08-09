#nullable enable

using System;
using GGroupp.Infra;
using PrimeFuncPack;

namespace GGroupp.Internal.Timesheet
{
    using ITimesheetCollectionGetFunc = IAsyncValueFunc<TimesheetCollectionGetIn, Result<TimesheetCollectionGetOut, Failure<Unit>>>;

    public static class TimesheetCollectionGetDependencyExtensions
    {
        public static Dependency<ITimesheetCollectionGetFunc> UseTimesheetCollectionGetApi<TDataverseEntitiesGetSupplier>(
            this Dependency<TDataverseEntitiesGetSupplier> dependency)
            where TDataverseEntitiesGetSupplier : IDataverseEntitiesGetSupplier
            =>
            dependency.Map<ITimesheetCollectionGetFunc>(s => TimesheetCollectionGetFunc.Create(s));
    }
}