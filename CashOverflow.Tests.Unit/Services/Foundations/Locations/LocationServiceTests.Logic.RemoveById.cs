// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldRemoveLocationById()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputLocationId = randomId;
            Location randomLocation = CreateRandomLocation();
            Location storageLocation = randomLocation;
            Location expectedInputLocation = storageLocation;
            Location deletedLocation = expectedInputLocation;
            Location expectedLocation = deletedLocation.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(inputLocationId))
                    .ReturnsAsync(storageLocation);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteLocationAsync(expectedInputLocation))
                    .ReturnsAsync(deletedLocation);

            // when
            Location actualLocation = await this.locationService
                .RemoveLocationByIdAsync(inputLocationId);

            // then
            actualLocation.Should().BeEquivalentTo(expectedLocation);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(inputLocationId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLocationAsync(expectedInputLocation),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
