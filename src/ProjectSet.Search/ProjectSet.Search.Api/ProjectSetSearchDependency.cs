using GGroupp.Infra;
using PrimeFuncPack;
using System;

namespace GGroupp.Internal.Timesheet;

using IProjectSetSearchFunc = IAsyncValueFunc<ProjectSetSearchIn, Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>>;

public static class ProjectSetSearchDependency
{
    public static Dependency<IProjectSetSearchFunc> UseProjectSetSearchApi<IDataVerseApiClient>(
        this Dependency<IDataVerseApiClient> dependency)
        where IDataVerseApiClient : IDataverseSearchSupplier
        =>
        dependency.Map<IProjectSetSearchFunc>(apiClient => ProjectSetSearchFunc.Create(apiClient));
}