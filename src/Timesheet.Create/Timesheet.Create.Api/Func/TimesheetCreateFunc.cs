using GGroupp.Infra;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

using ITimesheetCreateFunc = IAsyncValueFunc<TimesheetCreateIn, Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>>;

internal sealed partial class TimesheetCreateFunc : ITimesheetCreateFunc
{
    public static TimesheetCreateFunc Create(
        IDataverseEntityCreateSupplier entityCreateSupplier,
        [AllowNull] TimesheetCreateApiOption option)
        =>
        new(
            entityCreateSupplier ?? throw new ArgumentNullException(nameof(entityCreateSupplier)),
            option ?? new(default));

    private static readonly ReadOnlyCollection<string> selectedFields;

    static TimesheetCreateFunc()
        =>
        selectedFields = new(new[] { TimesheetJsonFieldName.Id });

    private readonly IDataverseEntityCreateSupplier entityCreateSupplier;

    private readonly TimesheetCreateApiOption option;

    private TimesheetCreateFunc(IDataverseEntityCreateSupplier entityCreateSupplier, TimesheetCreateApiOption option)
    {
        this.entityCreateSupplier = entityCreateSupplier;
        this.option = option;
    }
}
