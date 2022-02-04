using GGroupp.Infra;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

using IFavoriteProjectSetGet = IAsyncValueFunc<FavoriteProjectSetGetIn, Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>>;

internal sealed partial class FavoriteProjectSetGetFunc : IFavoriteProjectSetGet
{
    private readonly IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier;

    private readonly FavoriteProjectSetGetApiConfiguration configuration;

    private FavoriteProjectSetGetFunc(IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier, FavoriteProjectSetGetApiConfiguration configuration)
    {
        this.dataverseEntitySetGetSupplier = dataverseEntitySetGetSupplier;
        this.configuration = configuration;
    }

    public static FavoriteProjectSetGetFunc Create(
        IDataverseEntitySetGetSupplier dataverseEntitySetGetSupplier, 
        [AllowNull] FavoriteProjectSetGetApiConfiguration configuration)
        =>
        new(
            dataverseEntitySetGetSupplier ?? throw new ArgumentNullException(nameof(dataverseEntitySetGetSupplier)),
            configuration);

    //public partial ValueTask<Result<ProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>> InvokeAsync(
    //    FavoriteProjectSetGetIn input, CancellationToken cancellationToken = default);
}