#nullable enable

using System;

namespace GGroupp.Internal.Timesheet
{
    public sealed record TimesheetCollectionGetIn
    {
        public TimesheetCollectionGetIn(Guid userId, DateTimeOffset dateFrom, DateTimeOffset dateTo)
        {
            UserId = userId;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public Guid UserId { get; }

        public DateTimeOffset DateFrom { get; }

        public DateTimeOffset DateTo { get; }
    }
}