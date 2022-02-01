using GGroupp.Infra;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

using ITimesheetCreateFunc = IAsyncValueFunc<TimesheetCreateIn, Result<TimesheetCreateOut, Failure<TimesheetCreateFailureCode>>>;

internal sealed partial class TimesheetCreateGetFunc : ITimesheetCreateFunc
{
    public static TimesheetCreateGetFunc Create(
        IDataverseEntityCreateSupplier entityCreateSupplier,
        [AllowNull] TimesheetCreateApiConfiguration configuration)
        =>
        new(
            entityCreateSupplier ?? throw new ArgumentNullException(nameof(entityCreateSupplier)),
            configuration ?? new(default));

    private static readonly ReadOnlyCollection<string> selectedFields;

    static TimesheetCreateGetFunc()
        =>
        selectedFields = new(new[] { TimesheetJsonFieldName.Id });

    private readonly IDataverseEntityCreateSupplier entityCreateSupplier;

    private readonly TimesheetCreateApiConfiguration configuration;

    private TimesheetCreateGetFunc(IDataverseEntityCreateSupplier entityCreateSupplier, TimesheetCreateApiConfiguration configuration)
    {
        this.entityCreateSupplier = entityCreateSupplier;
        this.configuration = configuration;
    }
}
