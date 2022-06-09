using GGroupp.Infra;
using System;

namespace GGroupp.Internal.Timesheet;

using IGetFunc = IAsyncValueFunc<FavoriteProjectSetGetIn, Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>>;

internal sealed partial class FavoriteProjectSetGetFunc : IGetFunc
{
    private readonly IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier;

    private readonly ITodayProvider todayProvider;

    private readonly FavoriteProjectSetGetApiOption option;

    internal FavoriteProjectSetGetFunc(
        IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier,
        ITodayProvider todayProvider,
        FavoriteProjectSetGetApiOption option)
    {
        this.dataverseEntitySetGetSupplier = dataverseEntitySetGetSupplier;
        this.todayProvider = todayProvider;
        this.option = option;
    }
}