using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Internal.Timesheet;

public sealed record class TimesheetCreateApiConfiguration
{
    public TimesheetCreateApiConfiguration([AllowNull] IReadOnlyCollection<KeyValuePair<TimesheetChannel, int?>> channelCodes)
        =>
        ChannelCodes = channelCodes ?? Array.Empty<KeyValuePair<TimesheetChannel, int?>>();

    public IReadOnlyCollection<KeyValuePair<TimesheetChannel, int?>> ChannelCodes { get; }
}