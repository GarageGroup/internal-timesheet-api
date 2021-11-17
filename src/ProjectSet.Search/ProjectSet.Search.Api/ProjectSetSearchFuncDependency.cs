using GGroupp.Infra;
using PrimeFuncPack;
using System;

namespace GGroupp.Internal.Timesheet;

using IProjectSearch = IAsyncValueFunc<ProjectSetSearchIn, Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>>;

public static class ProjectSetSearchFuncDependency
{
    public static Dependency<IProjectSearch> UseProjectSetSearchApi<IDataVerseApiClient>(
        this Dependency<IDataVerseApiClient> dependency)
        where IDataVerseApiClient : IDataverseSearchSupplier
        =>
        dependency.Map<IProjectSearch>(apiClient => ProjectSetSearchFunc.Create(apiClient));
}
