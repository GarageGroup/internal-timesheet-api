using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet;

public interface IFavoriteProjectSetGetSupplier
{
    ValueTask<Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>> GetFavoriteProjectSetAsync(
        FavoriteProjectSetGetIn input, CancellationToken cancellationToken);
}