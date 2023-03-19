// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Linq;
using CashOverflow.Models.Locations;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllLocations()
        {
            // given
            IQueryable<Location> randomLocations = CreateRandomLocations();
            IQueryable<Location> storageLocations = randomLocations;
            IQueryable<Location> expectedLocations = storageLocations.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllLocations())
                    .Returns(storageLocations);

            // when
            IQueryable<Location> actualLocations =
                this.locationService.RetrieveAllLocations();

            // then
            actualLocations.Should().BeEquivalentTo(expectedLocations);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllLocations(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}