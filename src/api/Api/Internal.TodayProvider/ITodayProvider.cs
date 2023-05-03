using System;

namespace GGroupp.Internal.Timesheet;

internal interface ITodayProvider
{
    DateOnly GetToday();
}