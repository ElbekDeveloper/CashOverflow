// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Locations;
using CashOverflow.Services.Foundations.Locations;
using Moq;
using Tynamix.ObjectFiller;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ILocationService locationService;

        private static Location CreateRandomLocation() =>        
            CreateLocationFiller(date: GetRandomDateTimeOffset()).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<Location> CreateLocationFiller(DateTimeOffset date)
        {
            var filler = new Filler<Location>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(date);

            return filler;
        }
    }
}
