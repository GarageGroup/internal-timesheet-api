using GGroupp.Infra;
using System;
using System.Collections.ObjectModel;

namespace GGroupp.Internal.Timesheet;

using IFavoriteProjectSetGetFunc = IAsyncValueFunc<FavoriteProjectSetGetIn, Result<FavoriteProjectSetGetOut, Failure<FavoriteProjectSetGetFailureCode>>>;

internal sealed partial class FavoriteProjectSetGetGetFunc : IFavoriteProjectSetGetFunc
{
    public static FavoriteProjectSetGetGetFunc Create(IDataverseEntityCreateSupplier entityCreateSupplier)
        =>
        new(entityCreateSupplier ?? throw new ArgumentNullException(nameof(entityCreateSupplier)));

    private const int NotFoundFailureCode = -2147220969;

    private static readonly ReadOnlyCollection<string> selectedFields;

    static FavoriteProjectSetGetGetFunc()
        =>
        selectedFields = new(new[] { FavoriteProjectSetJsonFieldName.Id });

    private readonly IDataverseEntityCreateSupplier entityCreateSupplier;

    private FavoriteProjectSetGetGetFunc(IDataverseEntityCreateSupplier entityCreateSupplier)
        =>
        this.entityCreateSupplier = entityCreateSupplier;
}
