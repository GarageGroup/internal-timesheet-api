using GGroupp.Infra;
using System;

namespace GGroupp.Internal.Timesheet;

using IFavoriteProjectSetGet = IAsyncValueFunc<FavoriteProjectSetGetIn, Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>>;

internal sealed partial class FavoriteProjectSetGetFunc : IFavoriteProjectSetGet
{
    internal static FavoriteProjectSetGetFunc InternalCreate(
        IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier,
        ITodayProvider todayProvider,
        FavoriteProjectSetGetApiConfiguration configuration)
        =>
        new(
            dataverseEntitySetGetSupplier, todayProvider, configuration);

    private readonly IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier;

    private readonly ITodayProvider todayProvider;

    private readonly FavoriteProjectSetGetApiConfiguration configuration;

    private FavoriteProjectSetGetFunc(
        IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier,
        ITodayProvider todayProvider,
        FavoriteProjectSetGetApiConfiguration configuration)
    {
        this.dataverseEntitySetGetSupplier = dataverseEntitySetGetSupplier;
        this.todayProvider = todayProvider;
        this.configuration = configuration;
    }
}