using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class TimesheetCreateApiOption
{
    public TimesheetCreateApiOption([AllowNull] FlatArray<KeyValuePair<TimesheetChannel, int?>> channelCodes)
        =>
        ChannelCodes = channelCodes ?? FlatArray.Empty<KeyValuePair<TimesheetChannel, int?>>();

    public FlatArray<KeyValuePair<TimesheetChannel, int?>> ChannelCodes { get; }
}