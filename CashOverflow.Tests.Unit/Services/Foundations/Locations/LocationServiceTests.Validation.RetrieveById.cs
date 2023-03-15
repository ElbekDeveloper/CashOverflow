// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System;
using System.Threading.Tasks;
using CashOverflow.Models.Locations;
using CashOverflow.Models.Locations.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CashOverflow.Tests.Unit.Services.Foundations.Locations
{
    public partial class LocationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            var invalidLocationId = Guid.Empty;
            var invalidLocationException = new InvalidLocationException();

            invalidLocationException.AddData(
                key: nameof(Location.Id),
                values: "Id is required");

            var expectedLocationValidationException = new
                LocationValidationException(invalidLocationException);

            //when
            ValueTask<Location> retrieveLocationByIdTask =
                this.locationService.RetrieveLocationByIdAsync(invalidLocationId);

            LocationValidationException actualLocationValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(
                    retrieveLocationByIdTask.AsTask);

            //then
            actualLocationValidationException.Should().BeEquivalentTo(expectedLocationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLocationValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfLocationNotFoundAndLogItAsync()
        {
            //given
            Guid someLocationId = Guid.NewGuid();
            Location noLocation = null;

            var notFoundLocationException =
                new NotFoundLocationException(someLocationId);

            var expectedValidationException =
                new LocationValidationException(notFoundLocationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noLocation);

            //when
            ValueTask<Location> retrieveByIdLocationTask =
                this.locationService.RetrieveLocationByIdAsync(someLocationId);

            LocationValidationException actualValidationException =
                await Assert.ThrowsAsync<LocationValidationException>(
                    retrieveByIdLocationTask.AsTask);

            //then 
            actualValidationException.Should().BeEquivalentTo(expectedValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLocationByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}