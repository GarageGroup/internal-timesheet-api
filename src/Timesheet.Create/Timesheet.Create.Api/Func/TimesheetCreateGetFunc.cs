using GGroupp.Infra;
using System;
using System.Collections.ObjectModel;

namespace GGroupp.Internal.Timesheet;

using ITimesheetCreateFunc = IAsyncValueFunc<TimeSheetCreateIn, Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>>;

internal sealed partial class TimesheetCreateGetFunc : ITimesheetCreateFunc
{
    private const int NotFoundFailureCode = -2147220969;

    private static readonly ReadOnlyCollection<string> selectedFields;

    static TimesheetCreateGetFunc()
        =>
        selectedFields = new(new[] { TimesheetJsonFieldName.Id });

    private readonly IDataverseEntityCreateSupplier entityCreateSupplier;

    private TimesheetCreateGetFunc(IDataverseEntityCreateSupplier entityCreateSupplier)
        =>
        this.entityCreateSupplier = entityCreateSupplier;

    public static TimesheetCreateGetFunc Create(IDataverseEntityCreateSupplier entityCreateSupplier)
        =>
        new(entityCreateSupplier ?? throw new ArgumentNullException(nameof(entityCreateSupplier)));
}
