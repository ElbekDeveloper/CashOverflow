// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using FluentAssertions;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidLocationId = Guid.Empty;

            var invalidLocationException =
                new InvalidLocationException();

            invalidLocationException.AddData(
                key: nameof(Location.Id),
                values: "Id is required");

            var expectedLocationValidationException =
                new LocationValidationException(invalidLocationException);

            // when
            ValueTask<Location> removeLocationByIdTask =
                this.locationService.RemoveLocationByIdAsync(invalidLocationId);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(() =>
                    removeLocationByIdTask.AsTask());

            // then
            actualLocationValidationException.Should()
                .BeEquivalentTo(expectedLocationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLocationAsync(It.IsAny<Location>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveLocationByIdIsNotFoundAndLogItAsync()
        {
            // given
            Guid inputLocationId = Guid.NewGuid();
            Location noLocation = null;

            var notFoundLocationException =
                new NotFoundLocationException(inputLocationId);

            var expectedLocationValidationException =
                new LocationValidationException(notFoundLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noLocation);

            // when
            ValueTask<Location> removeLocationByIdTask =
                this.locationService.RemoveLocationByIdAsync(inputLocationId);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(() =>
                    removeLocationByIdTask.AsTask());

            // then
            actualLocationValidationException.Should()
                .BeEquivalentTo(expectedLocationValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLocationAsync(It.IsAny<Location>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
