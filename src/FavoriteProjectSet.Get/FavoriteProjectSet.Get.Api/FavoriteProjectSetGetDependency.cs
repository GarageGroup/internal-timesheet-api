using GGroupp.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Internal.Timesheet.FavoriteProjectSet.Get.Api.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GGroupp.Internal.Timesheet;

using IFunc = IAsyncValueFunc<FavoriteProjectSetGetIn, Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>>;

public static class FavoriteProjectSetGetDependency
{
    public static Dependency<IFunc> UseFavoriteProjectSetGetApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient> dependency,
        Func<IServiceProvider, FavoriteProjectSetGetApiOption> optionResolver)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return dependency.With(optionResolver).Fold(CreateFavoriteProjectSetGetFunc);
    }

    public static Dependency<IFunc> UseFavoriteProjectSetGetApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient, FavoriteProjectSetGetApiOption> dependency)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Fold(CreateFavoriteProjectSetGetFunc);
    }

    private static IFunc CreateFavoriteProjectSetGetFunc<TDataverseApiClient>(
        TDataverseApiClient dataverseApiClient, FavoriteProjectSetGetApiOption apiConfiguration)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
    {
        _ = dataverseApiClient ?? throw new ArgumentNullException(nameof(dataverseApiClient));

        return new FavoriteProjectSetGetFunc(dataverseApiClient, TodayProviderImpl.Instance, apiConfiguration);
    }
}