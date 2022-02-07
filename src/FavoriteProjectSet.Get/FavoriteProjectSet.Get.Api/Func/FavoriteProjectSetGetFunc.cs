using GGroupp.Infra;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

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
}