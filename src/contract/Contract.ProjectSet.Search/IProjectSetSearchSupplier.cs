using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

public interface IProjectSetSearchSupplier
{
    ValueTask<Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>> SearchProjectSetAsync(
        ProjectSetSearchIn input, CancellationToken cancellationToken);
}