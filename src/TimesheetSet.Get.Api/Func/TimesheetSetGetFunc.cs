#nullable enable

using System;
using GGroupp.Infra;

namespace GGroupp.Internal.Timesheet
{
    using ITimesheetSetGetFunc = IAsyncValueFunc<TimesheetSetGetIn, Result<TimesheetSetGetOut, Failure<Unit>>>;

    internal sealed partial class TimesheetSetGetFunc : ITimesheetSetGetFunc
    {
        public static TimesheetSetGetFunc Create(IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier)
            =>
            new(
                dataverseEntitySetGetSupplier ?? throw new ArgumentNullException(nameof(dataverseEntitySetGetSupplier)));

        private readonly IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier;

        private TimesheetSetGetFunc(IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier)
            =>
            this.dataverseEntitySetGetSupplier = dataverseEntitySetGetSupplier;
    }
}