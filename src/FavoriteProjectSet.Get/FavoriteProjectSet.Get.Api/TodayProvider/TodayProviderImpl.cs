using System;

namespace GGroupp.Internal.Timesheet;

internal sealed class TodayProviderImpl : ITodayProvider
{
    public static readonly TodayProviderImpl Instance;

    static TodayProviderImpl() => Instance = new();

    private TodayProviderImpl()
    {
    }

    public DateOnly GetToday() => DateOnly.FromDateTime(DateTime.Today);
}