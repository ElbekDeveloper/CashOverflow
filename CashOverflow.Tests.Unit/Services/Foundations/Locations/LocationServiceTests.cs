// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Locations;
using CashOverflow.Services.Foundations.Locations;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using Tynamix.ObjectFiller;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
		private ILocationService locationService;

        public LocationServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.locationService = new LocationService(
				storageBroker:this.storageBrokerMock.Object);

        }

        private DateTimeOffset GetRandomDatetimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private Location CreateRandomLocation() =>
            CreateLocationFiller(dates: GetRandomDatetimeOffset()).Create();

        private Filler<Location> CreateLocationFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Location>();

			filler.Setup().
				OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
