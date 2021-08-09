#nullable enable

using System;
using GGroupp.Infra;

namespace GGroupp.Internal.Timesheet
{
    using ITimesheetCollectionGetFunc = IAsyncValueFunc<TimesheetCollectionGetIn, Result<TimesheetCollectionGetOut, Failure<Unit>>>;

    internal sealed partial class TimesheetCollectionGetFunc : ITimesheetCollectionGetFunc
    {
        public static TimesheetCollectionGetFunc Create(IDataverseEntitiesGetSupplier dataverseEntitiesGetSupplier)
            =>
            new(
                dataverseEntitiesGetSupplier ?? throw new ArgumentNullException(nameof(dataverseEntitiesGetSupplier)));

        private readonly IDataverseEntitiesGetSupplier dataverseEntitiesGetSupplier;

        private TimesheetCollectionGetFunc(IDataverseEntitiesGetSupplier dataverseEntitiesGetSupplier)
            =>
            this.dataverseEntitiesGetSupplier = dataverseEntitiesGetSupplier;
    }
}