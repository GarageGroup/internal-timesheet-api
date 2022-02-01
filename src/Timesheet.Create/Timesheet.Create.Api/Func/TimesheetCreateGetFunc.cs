using GGroupp.Infra;
using System;
using System.Collections.ObjectModel;

namespace GGroupp.Internal.Timesheet;

using ITimesheetCreateFunc = IAsyncValueFunc<TimesheetCreateIn, Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>>;

internal sealed partial class TimesheetCreateGetFunc : ITimesheetCreateFunc
{
    public static TimesheetCreateGetFunc Create(IDataverseEntityCreateSupplier entityCreateSupplier)
        =>
        new(entityCreateSupplier ?? throw new ArgumentNullException(nameof(entityCreateSupplier)));

    private static readonly ReadOnlyCollection<string> selectedFields;

    static TimesheetCreateGetFunc()
        =>
        selectedFields = new(new[] { TimesheetJsonFieldName.Id });

    private readonly IDataverseEntityCreateSupplier entityCreateSupplier;

    private TimesheetCreateGetFunc(IDataverseEntityCreateSupplier entityCreateSupplier)
        =>
        this.entityCreateSupplier = entityCreateSupplier;
}
