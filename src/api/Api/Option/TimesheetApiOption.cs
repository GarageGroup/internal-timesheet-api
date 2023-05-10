using System;
using System.Collections.Generic;

namespace GarageGroup.Internal.Timesheet;

public sealed record class TimesheetApiOption
{
    public required FlatArray<KeyValuePair<TimesheetChannel, int?>> ChannelCodes { get; init; }

    public int? FavoriteProjectItemsCount { get; init; }

    public int? FavoriteProjectDaysCount { get; init; }
}