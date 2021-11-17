﻿using GGroupp.Infra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Internal.Timesheet;

using IProjectSetSearch = IAsyncValueFunc<ProjectSetSearchIn, Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>>;
internal sealed partial class ProjectSetSearchFunc : IProjectSetSearch
{
    private static readonly ReadOnlyDictionary<string, ProjectTypeSearchOut> entityTypes;
    private static readonly ReadOnlyCollection<string> entityNames;

    static ProjectSetSearchFunc()
    {
        entityTypes = new(new Dictionary<string, ProjectTypeSearchOut>
        {
            ["gg_project"] = ProjectTypeSearchOut.Project,
            ["lead"] = ProjectTypeSearchOut.Lead,
            ["opportunity"] = ProjectTypeSearchOut.Opportunity
        });

        entityNames = new(entityTypes.Keys.ToArray());
    }

    private readonly IDataverseSearchSupplier dataverseSearchSupplier;

    private ProjectSetSearchFunc(IDataverseSearchSupplier dataverseEntityCreateSupplier)
        =>
        this.dataverseSearchSupplier = dataverseEntityCreateSupplier;

    public static ProjectSetSearchFunc Create(IDataverseSearchSupplier dataverseEntitySearchSupplier)
        =>
        new(
            dataverseEntitySearchSupplier ?? throw new ArgumentNullException(nameof(dataverseEntitySearchSupplier)));

    public partial ValueTask<Result<ProjectSetSearchOut, Failure<ProjectSetSearchFailureCode>>> InvokeAsync(
        ProjectSetSearchIn input, CancellationToken cancellationToken = default);
}
