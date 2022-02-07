using GGroupp.Infra;
using System;

namespace GGroupp.Internal.Timesheet;

using IProjectSetSearch = IAsyncValueFunc<ProjectSetSearchIn, Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>>;

internal sealed partial class ProjectSetSearchFunc : IProjectSetSearch
{
    public static ProjectSetSearchFunc Create(IDataverseSearchSupplier dataverseSearchSupplier)
        =>
        new(
            dataverseSearchSupplier ?? throw new ArgumentNullException(nameof(dataverseSearchSupplier)));

    private readonly IDataverseSearchSupplier dataverseSearchSupplier;

    private ProjectSetSearchFunc(IDataverseSearchSupplier dataverseSearchSupplier)
        =>
        this.dataverseSearchSupplier = dataverseSearchSupplier;
}
