// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Locations;
using CashOverflow.Services.Foundations.Locations;
using Moq;
using Tynamix.ObjectFiller;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        private readonly ILocationService locationSerivce;
        private readonly Mock<IStorageBroker> storageBrokerMock;

        public LocationServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.locationSerivce = new LocationService(
                storageBroker: storageBrokerMock.Object);
        }

        private Location CreateRandomLocation() =>
            CreateLocationFiller(GetRandomDatetimeOffset()).Create();

        private Filler<Location> CreateLocationFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Location>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);
            return filler;
        }

        private DateTimeOffset GetRandomDatetimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();
    }
}