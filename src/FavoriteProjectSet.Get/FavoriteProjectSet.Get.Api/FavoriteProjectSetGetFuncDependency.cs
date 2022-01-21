using GGroupp.Infra;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Internal.Timesheet.FavoriteProjectSet.Get.Api.Test")]

namespace GGroupp.Internal.Timesheet;

using IFavoriteProjectSetGetFunc = IAsyncValueFunc<FavoriteProjectSetGetIn, Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>>;

public static class FavoriteProjectSetGetFuncDependency
{
    public static Dependency<IFavoriteProjectSetGetFunc> UseTimesheetCreateApi<TDataverseApiClient>(this Dependency<TDataverseApiClient> dependency)
        where TDataverseApiClient : IDataverseEntityCreateSupplier
        =>
        dependency.Map<IFavoriteProjectSetGetFunc>(apiClient => FavoriteProjectSetGetFunc.Create(apiClient));
}