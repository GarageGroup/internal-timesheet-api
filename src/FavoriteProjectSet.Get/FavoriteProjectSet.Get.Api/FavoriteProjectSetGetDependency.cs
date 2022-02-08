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
        Func<IServiceProvider, FavoriteProjectSetGetApiConfiguration> configurationResolver)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
        =>
        InnerUseFavoriteProjectSetGetApi(
            dependency ?? throw new ArgumentNullException(nameof(dependency)),
            configurationResolver ?? throw new ArgumentNullException(nameof(configurationResolver)));

    public static Dependency<IFunc> UseFavoriteProjectSetGetApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient, FavoriteProjectSetGetApiConfiguration> dependency)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
        =>
        InnerUseFavoriteProjectSetGetApi(
            dependency ?? throw new ArgumentNullException(nameof(dependency)));

    private static Dependency<IFunc> InnerUseFavoriteProjectSetGetApi<TDataverseApiClient>(
        Dependency<TDataverseApiClient> dependency,
        Func<IServiceProvider, FavoriteProjectSetGetApiConfiguration> configurationResolver)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
        =>
        dependency.With(configurationResolver).InnerUseFavoriteProjectSetGetApi();

    private static Dependency<IFunc> InnerUseFavoriteProjectSetGetApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient, FavoriteProjectSetGetApiConfiguration> dependency)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
        =>
        dependency.Fold(CreateFavoriteProjectSetGetFunc);

    private static IFunc CreateFavoriteProjectSetGetFunc<TDataverseApiClient>(
        TDataverseApiClient dataverseApiClient, FavoriteProjectSetGetApiConfiguration apiConfiguration)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
    {
        _ = dataverseApiClient ?? throw new ArgumentNullException(nameof(dataverseApiClient));

        return new FavoriteProjectSetGetFunc(dataverseApiClient, TodayProviderImpl.Instance, apiConfiguration);
    }
}