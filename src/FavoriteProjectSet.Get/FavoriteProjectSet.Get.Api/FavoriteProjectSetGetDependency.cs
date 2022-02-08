using GGroupp.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Internal.Timesheet.FavoriteProjectSet.Get.Api.Test")]

namespace GGroupp.Internal.Timesheet;

using IProjectSetGetFunc = IAsyncValueFunc<FavoriteProjectSetGetIn, Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>>;

public static class FavoriteProjectSetGetDependency
{
    public static Dependency<IProjectSetGetFunc> UseProjectSetGetApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient> dependency,
        Func<IServiceProvider, FavoriteProjectSetGetApiConfiguration> configurationResolver)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
        =>
        InnerUseProjectSetGetApi(
            dependency ?? throw new ArgumentNullException(nameof(dependency)),
            configurationResolver ?? throw new ArgumentNullException(nameof(configurationResolver)));

    public static Dependency<IProjectSetGetFunc> UseProjectSetGetApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient, FavoriteProjectSetGetApiConfiguration> dependency)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
        =>
        InnerUseProjectSetGetApi(
            dependency ?? throw new ArgumentNullException(nameof(dependency)));

    private static Dependency<IProjectSetGetFunc> InnerUseProjectSetGetApi<TDataverseApiClient>(
        Dependency<TDataverseApiClient> dependency,
        Func<IServiceProvider, FavoriteProjectSetGetApiConfiguration> configurationResolver)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
        =>
        dependency.With(configurationResolver).InnerUseProjectSetGetApi();

    private static Dependency<IProjectSetGetFunc> InnerUseProjectSetGetApi<TDataverseApiClient>(
        this Dependency<TDataverseApiClient, FavoriteProjectSetGetApiConfiguration> dependency)
        where TDataverseApiClient : IDataverseEntitySetGetSupplier
        =>
        dependency.Fold<IProjectSetGetFunc>(
            (apiClient, configuration) => FavoriteProjectSetGetFunc.Create(apiClient, configuration));
}