// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Linq.Expressions;
using CashOverflow.Brokers.DateTimes;
using CashOverflow.Brokers.Loggings;
using CashOverflow.Brokers.Storages;
using CashOverflow.Models.Locations;
using CashOverflow.Services.Foundations.Locations;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private ILocationService locationService;

        public LocationServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker> ();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.locationService = new LocationService(
				storageBroker:this.storageBrokerMock.Object ,
                dateTimeBroker:this.dateTimeBrokerMock.Object,
                loggingBroker:this.loggingBrokerMock.Object);

        }

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 9).GetValue();

        private DateTimeOffset GetRandomDatetimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private Location CreateRandomLocation(DateTimeOffset dates) =>
         CreateLocationFiller(dates).Create();

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
