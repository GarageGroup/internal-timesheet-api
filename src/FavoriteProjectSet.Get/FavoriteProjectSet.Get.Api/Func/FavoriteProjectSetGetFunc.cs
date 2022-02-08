using GGroupp.Infra;
using System;

namespace GGroupp.Internal.Timesheet;

using IGetFunc = IAsyncValueFunc<FavoriteProjectSetGetIn, Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>>;

internal sealed partial class FavoriteProjectSetGetFunc : IGetFunc
{
    private readonly IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier;

    private readonly ITodayProvider todayProvider;

    private readonly FavoriteProjectSetGetApiConfiguration configuration;

    internal FavoriteProjectSetGetFunc(
        IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier,
        ITodayProvider todayProvider,
        FavoriteProjectSetGetApiConfiguration configuration)
    {
        this.dataverseEntitySetGetSupplier = dataverseEntitySetGetSupplier;
        this.todayProvider = todayProvider;
        this.configuration = configuration;
    }
}