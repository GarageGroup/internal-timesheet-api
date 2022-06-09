using GGroupp.Infra;
using PrimeFuncPack;
using System;

namespace GGroupp.Internal.Timesheet;

using IProjectSetSearchFunc = IAsyncValueFunc<ProjectSetSearchIn, Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>>;

public static class ProjectSetSearchDependency
{
    public static Dependency<IProjectSetSearchFunc> UseProjectSetSearchApi<TDataVerseApiClient>(
        this Dependency<TDataVerseApiClient> dependency)
        where TDataVerseApiClient : IDataverseSearchSupplier
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Map<IProjectSetSearchFunc>(CreateFunc);

        static ProjectSetSearchFunc CreateFunc(TDataVerseApiClient apiClient)
            =>
            ProjectSetSearchFunc.Create(apiClient);
    }
}