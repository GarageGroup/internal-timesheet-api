using System;

namespace GarageGroup.Internal.Timesheet;

internal sealed class TodayProvider : ITodayProvider
{
    public static readonly TodayProvider Instance;

    static TodayProvider() => Instance = new();

    private TodayProvider()
    {
    }

    public DateOnly GetToday() => DateOnly.FromDateTime(DateTime.Today);
}