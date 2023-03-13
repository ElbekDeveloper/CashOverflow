// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;

namespace CashOverflow.Brokers.DateTimes {
    public interface IDateTimeBroker {
        DateTimeOffset GetCurrentDateTimeOffset();
    }
}
